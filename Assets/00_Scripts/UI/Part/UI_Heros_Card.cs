using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Heros_Card : MonoBehaviour
{
    [SerializeField] private Image m_slider;
    [SerializeField] private Image m_character;    
    [SerializeField] private Image m_rarity;    

    [SerializeField] private TextMeshProUGUI m_Level;
    [SerializeField] private TextMeshProUGUI m_Count;

    [SerializeField] public GameObject LockOBJ;
    [SerializeField] public GameObject GetOBJ;
    [SerializeField] public Button onClickButton;

    [SerializeField] private GameObject plus, minus;
    [SerializeField] public Animator Effect;

    [HideInInspector] public Hero_Scriptable m_hero;
    UI_Heros m_parent;

    public void Initalize(Hero_Scriptable data, UI_Heros parentBase)
    {        
        m_parent = parentBase;
        m_hero = data;
        
        int levelCount = BaseManager.Hero.LevelMaxCount(BaseManager.Data.HeroInfos[data.m_Character_Name]);
        m_slider.fillAmount = (float)BaseManager.Data.HeroInfos[data.m_Character_Name].Count / (float)levelCount;
        m_Count.text = BaseManager.Data.HeroInfos[data.m_Character_Name].Count.ToString() + " / " + levelCount.ToString();
        m_Level.text = "Lv." + (BaseManager.Data.HeroInfos[data.m_Character_Name].Level+1).ToString();

        m_character.sprite = Utils.Get_Atlas(data.m_Character_Name);
        m_rarity.sprite = Utils.Get_Atlas(data.m_Rarity.ToString());
               
        m_character.SetNativeSize();
        RectTransform rect = m_character.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector3(rect.sizeDelta.x / 4.5f, rect.sizeDelta.y / 4.5f);

        GetHeroCheck();
    }

    public void TextCheck()
    {
        int levelCount = BaseManager.Hero.LevelMaxCount(BaseManager.Data.HeroInfos[m_hero.m_Character_Name]);
        m_slider.fillAmount = (float)BaseManager.Data.HeroInfos[m_hero.m_Character_Name].Count / (float)levelCount;
        m_Count.text = BaseManager.Data.HeroInfos[m_hero.m_Character_Name].Count.ToString() + " / " + levelCount.ToString();
        m_Level.text = "Lv." + (BaseManager.Data.HeroInfos[m_hero.m_Character_Name].Level + 1).ToString();
    }
    public void EffectStart()
    {
        Effect.SetTrigger("isUpgrade");
    }
    public void GetHeroCheck()
    {
        bool isGet = false;
        for (int i = 0; i < BaseManager.Data.SetHeroData.Length; i++)
        {
            if (BaseManager.Data.SetHeroData[i] != null)
            {
                if (BaseManager.Data.SetHeroData[i] == m_hero)
                {
                    isGet = true;
                }
            }
        }
        GetOBJ.SetActive(isGet);
        plus.SetActive(!isGet);
        minus.SetActive(isGet);
    }

    public void ClickButton()
    {
        RenderManager.instance.Hero.GetParticle(true);
        m_parent.SetClick(this);        
    }

    public void OnClickHeroCard()
    {
        m_parent.GetHeroInfo(m_hero);
    }
}
