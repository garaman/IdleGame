using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Shop : BaseUI
{
    [SerializeField] private TextMeshProUGUI HeroSummon_Level;
    [SerializeField] private TextMeshProUGUI HeroSummon_Count;
    [SerializeField] private Slider HeroSummon_Fill;

    [SerializeField] private GameObject _summonInfoPanel;
    [SerializeField] private TextMeshProUGUI[] Percentage_Text;
    [SerializeField] private TextMeshProUGUI Percentage_Level;    
    int infoLevel = 0;

    public override bool Init()
    {    
        _summonInfoPanel.SetActive(false);
        TextCheck();
        return base.Init();
    }
    
    public void TextCheck()
    {
        // 영웅 소환 관련 정보 표시
        int level = Utils.SummonLevelCheck(DataManager.gameData.HeroSummon_Count);        
        int currentCount = Utils.SummonCurrentCountCheck(DataManager.gameData.HeroSummon_Count);

        HeroSummon_Level.text = "Lv." + (level + 1).ToString();
        HeroSummon_Count.text ="("+ currentCount + "/"+CSV_Importer.summonLevel[level]+")";             
        HeroSummon_Fill.value = (float)currentCount / CSV_Importer.summonLevel[level];

        //Debug.Log("Current Count: " + currentCount + ", Fill: " + (float)currentCount / CSV_Importer.summonLevel[level] + ", LevelCount: " + CSV_Importer.summonLevel[level]);
    }
    
    public void GetSummonInfo()
    {
        _summonInfoPanel.SetActive(true);
        PercentageCheck(Utils.SummonLevelCheck(DataManager.gameData.HeroSummon_Count));
    }

    public void ArrowButton(int value)
    {
        infoLevel += value;
        if(infoLevel < 0) { infoLevel = 9; }
        else if(infoLevel > 9) { infoLevel = 0; }

        PercentageCheck(infoLevel);
    }


    private void PercentageCheck(int value)
    {
        infoLevel = value;
        for (int i = 0; i < Percentage_Text.Length; i++)
        {
            float percentage = float.Parse(CSV_Importer.SummonDesign[value][((Rarity)i).ToString()].ToString());
            Percentage_Text[i].text = Utils.String_Color_Rarity((Rarity)i) + percentage.ToString("F4") + "%</color>";
        }

        Percentage_Level.text = "LEVEL." + (value+1).ToString();
    }

    public void GachaButton(int value)
    {
        BaseCanvas.instance.Get_UI("@Gacha");
        var ui = Utils.UI_Holder.Peek().GetComponent<Ui_Gacha>();
        ui.GetGachaHero(value);
    }

    public void GachaButton_ADS()
    {
       BaseManager.ADS.ShowRewardedAd(() => 
       {
           GachaButton(1);
       });
    }

    public override void DisableOBJ()
    {
        MainUI.instance.LayerCheck(-1);
        base.DisableOBJ();
    }
}
