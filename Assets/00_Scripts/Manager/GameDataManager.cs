using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDataManager
{
    public int Level;
    public int Stage;

    public double EXP;
    public double Money;        

    // 전체 영웅 정보.
    public Dictionary<string, HeroInfo> HeroInfos = new Dictionary<string, HeroInfo>();

    public void Init()
    {
        SetHeroInfo();
    }

    public void SetHeroInfo()
    {
        var datas = Resources.LoadAll<Hero_Scriptable>("Scriptable/Hero");

        foreach (var data in datas)
        {
            var info = new HeroInfo();
            info.Data = data;
            info.Level = 0;
            info.Count = 0;
            HeroInfos.Add(data.name, info);
        }
    }
}

public class HeroInfo
{
    public Hero_Scriptable Data;
    public int Level;
    public int Count;
}
