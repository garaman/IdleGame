using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;
public class LevelUpButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private Image m_Exp_Slider;
    [SerializeField] private TextMeshProUGUI EXP_Text, ATK_Text, HP_Text, GOIN_Text, Get_EXP_Text;
    bool isPush = false;
    float timer = 0.0f;
    Coroutine coroutine;

    private void Start()
    {
        InitEXP();
    }
    private void Update()
    {
        if(isPush)
        {
            timer += Time.deltaTime;
            if(timer >= 0.01f)
            {
                timer = 0.0f;
                //Debug.Log("연속 클릭");
            }
        }
    }
    public void EXP_UP()
    {
        BaseManager.Player.EXP_UP();
        InitEXP();
        transform.DORewind();
        transform.DOPunchScale(new Vector3(0.2f, 0.2f, 0.2f), 0.25f);
    }

    public void OnPointerDown(PointerEventData eventData)
    {        
        EXP_UP();
        coroutine = StartCoroutine(Push_Coroutin());
    }


    public void OnPointerUp(PointerEventData eventData)
    {
        //EXP_UP();
        isPush = false;
        if(coroutine != null)
        {
            StopCoroutine(coroutine);
        }
        timer = 0.0f;
    }

    private void InitEXP()
    {
        m_Exp_Slider.fillAmount = BaseManager.Player.EXP_percentage();
        EXP_Text.text = string.Format("{0:0.00}%", BaseManager.Player.EXP_percentage() * 100.0f);
        ATK_Text.text = $"+{StringMethod.ToCurrencyString(BaseManager.Player.Next_ATK())}";
        HP_Text.text = $"+{StringMethod.ToCurrencyString(BaseManager.Player.Next_HP())}";
        Get_EXP_Text.text = $"<color=#F66BFF>EXP</color> +{string.Format("{0:0.00}%", BaseManager.Player.Next_EXP())}";
    }
    IEnumerator Push_Coroutin()
    {
        yield return new WaitForSeconds(1.0f);
        isPush = true;
    }
}
