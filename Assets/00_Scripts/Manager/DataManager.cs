using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData
{
    public int Level;
    public int Stage;

    public double EXP;
    public double Money;

    public float[] Buffer_Timer = { 0.0f, 0.0f, 0.0f }; // 0 : ATK, 1 : DROP, 2 : CRITICAL
    public float Buffe_x2 = 0.0f; // 버프 효과 배율.
    public int buffLevel = 0;
    public int buffCount = 0;
}

public class HeroInfo
{
    public Hero_Scriptable Data;
    public Info info;
}

public class Info
{
    public int Level;
    public int Count;
}

public class DataManager
{
    public static GameData gameData = new GameData();
    public Dictionary<string, Info> Infos = new Dictionary<string, Info>(); // 유저 정보.


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
            var hero = new HeroInfo();
            hero.Data = data;

            Info h_info = new Info();
            if(Infos.ContainsKey(data.name))
            {
                h_info = Infos[data.name]; // 기존 정보가 있다면 가져옴.
            }
            else
            {
                Infos.Add(data.name, h_info); // 없다면 새로 추가.
            }

            hero.info = h_info;

            HeroInfos.Add(data.name, hero);
        }
    }
}

