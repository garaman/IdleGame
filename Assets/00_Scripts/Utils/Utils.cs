using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class Utils
{
    public static SpriteAtlas m_Atlas = Resources.Load<SpriteAtlas>("Atlas");    

    public static Stack<BaseUI> UI_Holder = new Stack<BaseUI>();
    public static LevelDesign DesignData = Resources.Load<LevelDesign>("Scriptable/LevelDesignData");
    public static void CloseAllPopupUI()
    {
        while(UI_Holder.Count > 0)
        {
            ClosePopupUI();
        }
    }
    public static void ClosePopupUI()
    {
        if (UI_Holder.Count == 0) { return; }

        BaseUI popup = UI_Holder.Peek();
        popup.DisableOBJ();
    }
    public static Sprite Get_Atlas(string temp)
    {
        return m_Atlas.GetSprite(temp);
    }

    public static string String_Color_Rarity(Rarity rare)
    {
        switch (rare)
        {
            case Rarity.Common: return "<color=#FFFFFF>"; 
            case Rarity.UnCommon: return "<color=#00FF00>";
            case Rarity.Rare: return "<color=#00D8FF>";
            case Rarity.Hero: return "<color=#B750C3>";
            case Rarity.Legendary: return "<color=#FF9000>";
        }

        return "<color=#FFFFFF>";
    }

    public static double CalculatedValue(float baseValue, int level, float value)
    {
        return baseValue * Mathf.Pow((level+1), value);
    }

    public static bool Coin_Check(double value)
    {
        if (DataManager.gameData.Money >= value) { return true; }
        else { return false; }
    }

    public static string GetTimer(float timer)
    {
        TimeSpan timeSpan = TimeSpan.FromSeconds(timer);
        string timeString = string.Format("{0:00}:{1:00}", timeSpan.Minutes, timeSpan.Seconds);

        return timeString;
    }

    public static int SummonLevelCheck(int count)
    {
        int[] summonLevel = CSV_Importer.summonLevel;
        for (int i = 0; i < summonLevel.Length; i++)
        {
            if (count < summonLevel[i])
            {
                return i;
            }
        }
        return summonLevel.Length; // 모든 레벨이 달성된 경우 -1 반환
    }

    public static int SummonCurrentCountCheck(int count)
    {
        int[] summonLevel = CSV_Importer.summonLevel;
        for (int i = 0; i < summonLevel.Length; i++)
        {
            if (count < summonLevel[i])
            {
                if (i == 0) return count; // 첫 번째 레벨의 경우 현재 카운트 반환
                else return count - summonLevel[i - 1]; // 이전 레벨과의 차이 반환                
            }
        }        
        return summonLevel[summonLevel.Length - 1]; // 마지막 레벨의 경우 마지막 레벨을 값을 반환 MAX설정.
    }

    public static float[] GachaPercentage()
    {
        int value = SummonLevelCheck(DataManager.gameData.HeroSummon_Count);
        float[] valueCount = new float[5];
        for(int i = 0; i < valueCount.Length; i++)
        {
            string rarity = ((Rarity)i).ToString();
            valueCount[i] = float.Parse(CSV_Importer.SummonDesign[value][rarity].ToString());
        }
        return valueCount;
    }
}
