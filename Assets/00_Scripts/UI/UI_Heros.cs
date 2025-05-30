using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Unity.Collections.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UI_Heros : BaseUI
{
    public Transform Content;
    public GameObject Card;    

    private List<UI_Heros_Card> Cards = new List<UI_Heros_Card>();
    Dictionary<string, Hero_Scriptable> m_HeroDic = new Dictionary<string, Hero_Scriptable>();
    Hero_Scriptable m_Hero;

    [SerializeField] private UI_HeroInfo heroInfoPanel;    

    public void UpgradeButton(HeroInfo hero) 
    {
        Info info = BaseManager.Data.Infos[hero.Data.name];

        int value = (info.Level+1) * 5;
        if (info.Count >= value)
        {
            info.Count -= value;
            info.Level++;
        }

        GetHeroInfo(hero.Data);
        foreach (var card in Cards)
        {
            if(card.m_hero == hero.Data)
            {
                card.EffectStart();
                card.TextCheck();
            }
        }

        BaseManager.Firebase.WriteData();
    }

    public void AllUpgradeButton()
    {        
        StartCoroutine(AllUpgrade_Coroutine());
    }

    IEnumerator AllUpgrade_Coroutine()
    {
        bool isUpgraded = false;
        foreach (var hero in m_HeroDic)
        {
            Info info = BaseManager.Data.Infos[hero.Key];
                        
            int value = (info.Level + 1) * 5;
            if (info.Count >= value)
            {                
                info.Count -= value;
                info.Level++;
                foreach (var card in Cards)
                {
                    if (card.m_hero == hero.Value)
                    {
                        card.EffectStart();
                        card.TextCheck();
                    }                    
                }
                isUpgraded = true;
            }
            yield return new WaitForSeconds(0.3f);
        }

        if (isUpgraded)
        {
            BaseCanvas.instance.Get_Toast().Initalize("강화할 영웅이 존재하지 않습니다.");
        }

        BaseManager.Firebase.WriteData();
    }

    public override bool Init()
    {
        InitButtons();
        RenderManager.instance.Hero.InitHero();

        MainUI.instance.FadeInOut(true, true, null);

        var HeroData =BaseManager.Data.HeroInfos;
        foreach(var hero in HeroData)
        {
            if(hero.Value.Data.m_Rarity == Rarity.None) { continue; }
            m_HeroDic.Add(hero.Value.Data.m_Character_Name, hero.Value.Data);
        }        

        var sort_Dictionary = m_HeroDic.OrderByDescending(x => x.Value.m_Rarity);
        
        foreach (var data in sort_Dictionary)
        {
            var go = Instantiate(Card, Content).GetComponent<UI_Heros_Card>();            
            Cards.Add(go);            
            go.Initalize(data.Value,this);            
        }
                
        return base.Init();
    }


    public void SetHeroButton(int value)
    {
        BaseManager.Hero.GetHero(value, m_Hero.m_Character_Name);        
        HeroInitalize();
    }

    public void HeroInitalize()
    {
        RenderManager.instance.Hero.GetParticle(false);
        SetClick(null);
        RenderManager.instance.Hero.InitHero();

        for (int i = 0; i < Cards.Count; i++)
        {
            Cards[i].GetHeroCheck();
        }
        MainUI.instance.SetHeroData();
    }

    public void SetClick(UI_Heros_Card s_Card)
    {
        if(s_Card == null)
        {
            for (int i = 0; i < Cards.Count; i++)
            {
                Cards[i].LockOBJ.SetActive(false);
                Cards[i].GetComponent<Outline>().enabled = false;
            }
        }
        else
        {            
            for (int i = 0; i < BaseManager.Hero.SetHeroInfos.Length; i++)
            {
                var data = BaseManager.Hero.SetHeroInfos[i];
                if (data != null)
                {
                    if (data.Data == s_Card.m_hero)
                    {
                        BaseManager.Hero.DisableHero(i);
                        HeroInitalize();
                        return;
                    }
                }
            }

            m_Hero = s_Card.m_hero;

            for (int i = 0; i < Cards.Count; i++)
            {
                Cards[i].LockOBJ.SetActive(true);
                Cards[i].GetComponent<Outline>().enabled = false;
            }

            s_Card.LockOBJ.SetActive(false);
            s_Card.GetComponent<Outline>().enabled = true;
        }
            
    }

    public void InitButtons()
    {
        for(int i = 0; i < RenderManager.instance.Hero.circles.Length; i++)
        {
            int index = i;  
            var go = new GameObject("Button").AddComponent<Button>();            
            go.transform.SetParent(transform);
            go.onClick.AddListener(() => SetHeroButton(index));
            go.gameObject.AddComponent<Image>();
            //go.gameObject.AddComponent<RectTransform>();

            Image img = go.GetComponent<Image>();
            img.color = new Color(1.0f, 1.0f, 1.0f,0.01f);

            RectTransform rect = go.GetComponent<RectTransform>();
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;
            rect.sizeDelta = new Vector2(80, 80);

            go.transform.position = RenderManager.instance.ReturnScreenPoint(RenderManager.instance.Hero.circles[i]);
        }
    }


    public void GetHeroInfo(Hero_Scriptable data)
    {
        HeroInfo hero = BaseManager.Data.HeroInfos[data.name];
        float LevelUpCount = (BaseManager.Data.Infos[data.name].Level + 1) * 5;

        heroInfoPanel.gameObject.SetActive(true);

        heroInfoPanel.heroRarity.sprite = Utils.Get_Atlas(data.m_Rarity.ToString());
        heroInfoPanel.heroIcon.sprite = Utils.Get_Atlas(data.name);        
        heroInfoPanel.heroNameText.text = data.m_Character_Name;
        heroInfoPanel.heroDesText.text = "";
        heroInfoPanel.rarityText.text = Utils.String_Color_Rarity(data.m_Rarity) + data.m_Rarity.ToString() + "</color>";

        heroInfoPanel.fightScoreText.text = "";
        heroInfoPanel.ATKScoreText.text = StringMethod.ToCurrencyString(BaseManager.Player.Get_ATK(data.m_Rarity, hero));
        heroInfoPanel.HpScoreText.text = StringMethod.ToCurrencyString(BaseManager.Player.Get_HP(data.m_Rarity, hero));

        heroInfoPanel.heroLevelText.text = "Lv."+(BaseManager.Data.Infos[data.name].Level +1).ToString();
        heroInfoPanel.heroCountText.text = "("+BaseManager.Data.Infos[data.name].Count.ToString()+"/" + LevelUpCount.ToString()+")";
        heroInfoPanel.sliderFill.fillAmount = (float)BaseManager.Data.Infos[data.name].Count / LevelUpCount;

        heroInfoPanel.upgrade.onClick.RemoveAllListeners();
        heroInfoPanel.upgrade.onClick.AddListener(() => UpgradeButton(hero));


        Debug.Log("HeroInfos : " + BaseManager.Data.HeroInfos[data.name].info.Level + "/" + BaseManager.Data.HeroInfos[data.name].info.Count);
        Debug.Log("Infos : " + BaseManager.Data.Infos[data.name].Level + "/" + BaseManager.Data.Infos[data.name].Count);
    }

    public override void DisableOBJ()
    {
        MainUI.instance.FadeInOut(false, true, () =>
        {
            MainUI.instance.FadeInOut(true, false, null);
            base.DisableOBJ();
        });
    }
}
