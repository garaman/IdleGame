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
        
        int levelCount = (BaseManager.Data.Infos[data.m_Character_Name].Level+1) * 5;
        m_slider.fillAmount = (float)BaseManager.Data.Infos[data.m_Character_Name].Count / (float)levelCount;
        m_Count.text = BaseManager.Data.Infos[data.m_Character_Name].Count.ToString() + " / " + levelCount.ToString();
        m_Level.text = "Lv." + (BaseManager.Data.Infos[data.m_Character_Name].Level+1).ToString();

        m_character.sprite = Utils.Get_Atlas(data.m_Character_Name);
        m_rarity.sprite = Utils.Get_Atlas(data.m_Rarity.ToString());
               
        m_character.SetNativeSize();
        RectTransform rect = m_character.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector3(rect.sizeDelta.x / 4.5f, rect.sizeDelta.y / 4.5f);

        GetHeroCheck();
    }

    public void TextCheck()
    {
        int levelCount = (BaseManager.Data.Infos[m_hero.m_Character_Name].Level + 1) * 5;
        m_slider.fillAmount = (float)BaseManager.Data.Infos[m_hero.m_Character_Name].Count / (float)levelCount;
        m_Count.text = BaseManager.Data.Infos[m_hero.m_Character_Name].Count.ToString() + " / " + levelCount.ToString();
        m_Level.text = "Lv." + (BaseManager.Data.Infos[m_hero.m_Character_Name].Level + 1).ToString();
    }
    public void EffectStart()
    {
        Effect.SetTrigger("isUpgrade");
    }
    public void GetHeroCheck()
    {
        bool isGet = false;
        for (int i = 0; i < BaseManager.Hero.SetHeroInfos.Length; i++)
        {
            if (BaseManager.Hero.SetHeroInfos[i] != null)
            {
                if (BaseManager.Hero.SetHeroInfos[i].Data == m_hero)
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
