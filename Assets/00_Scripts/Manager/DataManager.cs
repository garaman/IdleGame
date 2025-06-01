using System;
using System.Collections.Generic;
using UnityEngine;

public class GameData
{
    public int Level;
    public int Stage;

    public double EXP;
    public double Money;
    public double Cristal;

    public float[] Buffer_Timer = { 0.0f, 0.0f, 0.0f }; // 0 : ATK, 1 : DROP, 2 : CRITICAL
    public float Buffe_x2 = 0.0f; // 버프 효과 배율.
    public int buffLevel = 0;
    public int buffCount = 0;

    // Shop 관련 정보.    
    public int HeroSummon_Count = 0; // 소환 횟수.

    public string startDate;
    public string endDate;
}

public class Info
{
    public int Level;
    public int Count;
}

public class DataManager
{
    public static GameData gameData = new GameData();
    public Dictionary<string, Info> HeroInfos = new Dictionary<string, Info>(); // 유저 정보.
    public Dictionary<string, Info> ItemInfos = new Dictionary<string, Info>(); // 아이템 정보.
        
    public Dictionary<string, Hero_Scriptable> HeroData = new Dictionary<string, Hero_Scriptable>(); // 전체 영웅 정보.
    public Dictionary<string, Item_Scriptable> ItemData = new Dictionary<string, Item_Scriptable>(); // 전체 아이템 정보.

    
    public Hero_Scriptable[] SetHeroData = new Hero_Scriptable[6]; // 사용중인 영웅 정보.
    public Item_Scriptable[] SetItemData = new Item_Scriptable[6]; // 사용중인 유물 정보.


    public void Init()
    {
        SetHeroInfo();
        SetItemInfo();;
    }

    public Hero_Scriptable Get_Rarity_Hero(Rarity rarity)
    {
        List<Hero_Scriptable> list= new List<Hero_Scriptable>();

        foreach (var hero in HeroData.Values)
        {
            if (hero.m_Rarity == rarity)
            {
                list.Add(hero);
            }
        }        
        return list[UnityEngine.Random.Range(0,list.Count)];
    }

    public void SetHeroInfo()
    {
        var datas = Resources.LoadAll<Hero_Scriptable>("Scriptable/Hero");

        foreach (var data in datas)
        {
            Info h_info = new Info();
            if (HeroInfos.ContainsKey(data.name))
            {
                h_info = HeroInfos[data.name]; // 기존 정보가 있다면 가져옴.
            }
            else
            {
                HeroInfos.Add(data.name, h_info); // 없다면 새로 추가.
            }

            if (!HeroData.ContainsKey(data.name))
            {
                HeroData.Add(data.name, data);
            }
        }
    }

    public void SetItemInfo()
    {
        var datas = Resources.LoadAll<Item_Scriptable>("Scriptable/Item");

        foreach (var data in datas)
        {
            Info I_info = new Info();
            if (ItemInfos.ContainsKey(data.name))
            {
                I_info = ItemInfos[data.name]; // 기존 정보가 있다면 가져옴.
            }
            else
            {
                ItemInfos.Add(data.name, I_info); // 없다면 새로 추가.
            }

            if (!ItemData.ContainsKey(data.name))
            {
                ItemData.Add(data.name, data);
            }
        }
    }

}

