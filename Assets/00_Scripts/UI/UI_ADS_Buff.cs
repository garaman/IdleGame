using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_ADS_Buff : BaseUI
{
    public enum ADSBuff_State
    {
        ATK,
        DROP,
        CRITICAL
    }

    [SerializeField] private TextMeshProUGUI m_LevelText, m_LevelCount;
    [SerializeField] private Image m_LevelFill;

    [SerializeField] private Button[] m_Buttons;
    [SerializeField] private GameObject[] m_Buttons_Lock, m_Lock;
    [SerializeField] private TextMeshProUGUI[] m_Timers_Text;
    [SerializeField] private Image[] m_Timers_Fill;


    public override bool Init()
    {
        for (int i = 0; i < m_Buttons.Length; i++)
        {
            int index = i;
            m_Buttons[i].onClick.AddListener(() => { GetBuff((ADSBuff_State)index); });
        }

        for (int i = 0; i <DataManager.gameData.Buffer_Timer.Length; i++)
        {
            if (DataManager.gameData.Buffer_Timer[i] > 0.0f)
            {
                SetBuff(i, true);
            }
        }
        return base.Init();
    }

    private void Update()
    {
        for (int i = 0; i <DataManager.gameData.Buffer_Timer.Length; i++)
        {
            if (DataManager.gameData.Buffer_Timer[i] > 0.0f)
            {                
                m_Timers_Fill[i].fillAmount = 1.0f - (DataManager.gameData.Buffer_Timer[i] / 1800.0f);                
                m_Timers_Text[i].text = Utils.GetTimer(DataManager.gameData.Buffer_Timer[i]);
            }
            else
            {
                m_Timers_Text[i].text = "00:00";
                m_Timers_Fill[i].fillAmount = 1;
            }
        }
    }

    public void GetBuff(ADSBuff_State state)
    {
        BaseManager.ADS.ShowRewardedAd(() =>
        {
            int stateValue = (int)state;

           DataManager.gameData.buffCount++;

           DataManager.gameData.Buffer_Timer[stateValue] = 1800.0f;

            SetBuff(stateValue, true);

            MainUI.instance.BuffCheck();
        });
    }

    void SetBuff(int value, bool GetBool)
    {
        m_Buttons_Lock[value].SetActive(GetBool);
        m_Lock[value].SetActive(!GetBool);
        m_Timers_Fill[value].gameObject.SetActive(GetBool);
    }
}
