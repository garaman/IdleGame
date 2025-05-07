using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainUI : MonoBehaviour
{
    public static MainUI instance = null;
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;

            //DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    [Header("Default")]
    [SerializeField] private TextMeshProUGUI m_Level_Text;
    [SerializeField] private TextMeshProUGUI m_FightScore_Text;

    [Space(10.0f)]
    [Header("Fade")]    
    [SerializeField] private Image m_Fade;
    [SerializeField] private float m_FadeDuration = 0.5f;

    [Space(10.0f)]
    [Header("MonsterSlider")]    
    [SerializeField] private GameObject m_MonsterSlider_OBJ;
    [SerializeField] private Slider m_MonsterSlider;
    [SerializeField] private TextMeshProUGUI m_MonsterSlider_Text;

    [Space(10.0f)]
    [Header("BossSlider")]    
    [SerializeField] private GameObject m_BossSlider_OBJ;
    [SerializeField] private Image m_BossSlider_HP;
    [SerializeField] private TextMeshProUGUI m_BossSlider_Text;
    [SerializeField] private TextMeshProUGUI m_BossSlider_Name;
    [SerializeField] private TextMeshProUGUI m_Stage_Text;

    private void Start()
    {
        TextCheck();
        SlliderCheck();

        StageManager.m_ReadyEvent += () => FadeInOut(true, false);
        StageManager.m_BossEvent += () => OnBoss();        
    }

    private void OnBoss()
    {
        m_BossSlider_OBJ.SetActive(true);
        m_MonsterSlider_OBJ.SetActive(false);
    }

    public void TextCheck()
    {
        m_Level_Text.text = "Lv."+(BaseManager.Player.Level +1).ToString();
        m_FightScore_Text.text = StringMethod.ToCurrencyString(BaseManager.Player.Get_FightScore());
    }
    public void SlliderCheck()
    {
        m_BossSlider_OBJ.SetActive(false);
        m_MonsterSlider_OBJ.SetActive(true);

        m_MonsterSlider.value = 0;
        m_MonsterSlider_Text.text = "0%";
    }

    public void FadeInOut(bool FadeInOut,bool Sibling = false, Action action = null)
    {
        if(Sibling == false)
        {
            m_Fade.transform.SetParent(this.transform);
            m_Fade.transform.SetSiblingIndex(0);
        }
        else
        {
            m_Fade.transform.SetParent(BaseCanvas.instance.transform);
            m_Fade.transform.SetAsLastSibling();
        }
        
        StartCoroutine(FadeInOout_Coroutine(FadeInOut, action));
    }

    IEnumerator FadeInOout_Coroutine(bool FadeInOut, Action action = null)
    {
        if (FadeInOut == false)
        {
            m_Fade.raycastTarget = true;
        }

        float current = 0.0f;
        float percent = 0.0f;
        float start = FadeInOut ? 1.0f : 0.0f;
        float end = FadeInOut ? 0.0f : 1.0f;

        while (percent < 1.0f)
        {
            current += Time.deltaTime;
            percent = current / m_FadeDuration;
            float LerpPos = Mathf.Lerp(start, end, percent);
            m_Fade.color = new Color(0, 0, 0, LerpPos);
            yield return null;
        }

        if (action != null)
        {
            action?.Invoke();
        }

        m_Fade.raycastTarget = false;
    }

    public void MonsterSlider()
    {
        float value = (float)StageManager.count / (float)StageManager.maxCount;
        if(value >= 1.0f) 
        { 
            value = 1.0f; 
            if(StageManager.m_State != Stage_State.Boss)
            { 
                StageManager.State_Change(Stage_State.Boss); 
            }            
        }        
        m_MonsterSlider.value = value;
        m_MonsterSlider_Text.text = string.Format("{0:0.0}", value * 100.0f) + "%";
    }

    public void BossSlider(double hp, double maxHp)
    {
        float value = (float)hp / (float)maxHp;
        if (value <= 0.0f)
        {
            value = 0.0f;            
        }
        m_BossSlider_HP.fillAmount = value;
        m_BossSlider_Text.text = string.Format("{0:0.0}", value * 100.0f) + "%";
    }
}
