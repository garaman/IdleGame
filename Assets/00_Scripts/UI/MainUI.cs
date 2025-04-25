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

            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    [SerializeField] private TextMeshProUGUI m_Level_Text;
    [SerializeField] private TextMeshProUGUI m_FightScore_Text;
    [SerializeField] private Image m_Fade;
    [SerializeField] private float m_FadeDuration = 0.5f;

    private void Start()
    {
        TextCheck();
    }

    public void TextCheck()
    {
        m_Level_Text.text = "Lv."+(BaseManager.Player.Level +1).ToString();
        m_FightScore_Text.text = StringMethod.ToCurrencyString(BaseManager.Player.Get_FightScore());
    }

    public void FadeInOut(bool FadeInOut,bool Sibling, Action action = null)
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
}
