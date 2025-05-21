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

    public override bool Init()
    {
        InitButtons();
        RenderManager.instance.Hero.InitHero();

        MainUI.instance.FadeInOut(true, true, null);

        var HeroData = BaseManager.Data.HeroInfos;
        foreach(var hero in HeroData)
        {
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

    public override void DisableOBJ()
    {
        MainUI.instance.FadeInOut(false, true, () =>
        {
            MainUI.instance.FadeInOut(true, false, null);
            base.DisableOBJ();
        });        
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
            go.onClick.AddListener(() => SetHeroButton(index));
            go.transform.SetParent(transform);

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

    private void SetHeroButton(int value)
    {
        BaseManager.Hero.GetHero(value, m_Hero.m_Character_Name);
        RenderManager.instance.Hero.GetParticle(false);
        SetClick(null);
        RenderManager.instance.Hero.InitHero();

        for (int i = 0; i < Cards.Count; i++)
        {
            Cards[i].GetHeroCheck();
        }
        MainUI.instance.SetHeroData();
    }

}
