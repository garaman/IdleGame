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


    public hero_Scriptable m_hero;
    UI_Heros m_parent;

    public void Initalize(hero_Scriptable data, UI_Heros parentBase)
    {
        m_parent = parentBase;
        m_hero = data;
        m_character.sprite = Utils.Get_Atlas(data.m_Character_Name + " 1");
        m_rarity.sprite = Utils.Get_Atlas(data.m_Rarity.ToString());
               
        m_character.SetNativeSize();
        RectTransform rect = m_character.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector3(rect.sizeDelta.x / 4.5f, rect.sizeDelta.y / 4.5f);

        GetHeroCheck();
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
    }

    public void OnClickHeroCard()
    {
        m_parent.SetClick(this);
        RenderManager.instance.Hero.GetParticle(true);

    }
}
