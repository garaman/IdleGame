using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Main_Part : MonoBehaviour
{
    [SerializeField] private GameObject Lock, Plus;
    [SerializeField] private Image Icon,Fill;
    [SerializeField] private TextMeshProUGUI Hp, Mp;    
    [SerializeField] private GameObject ReadyOBJ;    

    hero_Scriptable m_Data;

    private void Start()
    {
        Initalize();
    }

    public void Initalize()
    {
        if(m_Data == null)
        {
            Hp.gameObject.SetActive(false);            
            Fill.transform.parent.gameObject.SetActive(false);            
            Lock.SetActive(false);
            Plus.SetActive(true);
            Icon.gameObject.SetActive(false);
        }
        else
        {
            InitData(m_Data, true);
        }
    }

    public void GetHeroSetPopup()
    {
        BaseCanvas.instance.Get_UI("@Heros");
    }
    public void InitData(hero_Scriptable data, bool isReady)
    {
        m_Data = data;

        Lock.SetActive(false);
        Plus.SetActive(false);
        Icon.gameObject.SetActive(true);
        Icon.sprite = Utils.Get_Atlas(data.m_Character_Name);

        ReadyOBJ.SetActive(isReady);
        Fill.transform.parent.gameObject.SetActive(!isReady);
        Hp.gameObject.SetActive(!isReady);
    }

    public void StateCheck(Player player)
    {
        Fill.fillAmount = player.MP / m_Data.Max_Mp;
        Hp.text = StringMethod.ToCurrencyString(player.HP);
        Mp.text = player.MP.ToString() + "/" + m_Data.Max_Mp;
    }
}
