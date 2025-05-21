using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public delegate void OnSavingMode();

public class UI_SavingMode : BaseUI
{
    [SerializeField] Image LandImage,BatteryFill;
    [SerializeField] TextMeshProUGUI BatteryText, TimerText, StageText, StateText;

    public static OnSavingMode m_OnSving;

    Vector2 startPos, endPos;
    Camera mainCam;
    bool isEnd = false;

    public override bool Init()
    {
        mainCam = Camera.main;
        mainCam.enabled = false;

        m_OnSving?.Invoke();

        return base.Init();
    }

    private void Update()
    {
        BatteryText.text = (SystemInfo.batteryLevel*100.0f).ToString() + "%";
        BatteryFill.fillAmount = SystemInfo.batteryLevel;

        TimerText.text = System.DateTime.Now.ToString("tt hh:mm");

        int stageValue = BaseManager.Data.Stage + 1;
        int stageForward = (stageValue / 10) + 1;
        int stageBack = stageValue % 10;

        StageText.text = "Stage " + stageForward + "-" + stageBack;

        StateText.text = StageManager.isDead ? "반복중.." : "진행중..";
        StateText.color = StageManager.isDead ? Color.yellow : Color.white;

        if(Input.GetMouseButtonDown(0))
        {
            startPos = Input.mousePosition;
        }
        if(Input.GetMouseButton(0))
        {
            endPos = Input.mousePosition;
            float distance = Vector2.Distance(startPos, endPos);
            LandImage.color = new Color(1.0f, 1.0f, 1.0f, Mathf.Clamp(distance/(Screen.width/2), 0.3f, 1.0f));

            if (distance >= Screen.width / 2)
            {
                if(!isEnd)
                {
                    isEnd = true;
                    mainCam.enabled = true;
                    BaseCanvas.isSave = false;
                    DisableOBJ();
                }
                
            }
        }
        if(Input.GetMouseButtonUp(0))
        {
            startPos = Vector2.zero;
            endPos = Vector2.zero;
            LandImage.color = new Color(1.0f, 1.0f, 1.0f, 0.3f);
        }
    }
}
