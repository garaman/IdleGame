using System.Collections;
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
    public float Buffe_x2 = 0.0f; // ���� ȿ�� ����.
    public int buffLevel = 0;
    public int buffCount = 0;

    // Shop ���� ����.    
    public int HeroSummon_Count = 0; // ��ȯ Ƚ��.
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
    public Dictionary<string, Info> Infos = new Dictionary<string, Info>(); // ���� ����.


    // ��ü ���� ����.
    public Dictionary<string, HeroInfo> HeroInfos = new Dictionary<string, HeroInfo>();

    public void Init()
    {
        SetHeroInfo();
        CheckInfo();
    }

    public Hero_Scriptable Get_Rarity_Hero(Rarity rarity)
    {
        List<Hero_Scriptable> list= new List<Hero_Scriptable>();

        foreach (var hero in HeroInfos.Values)
        {
            if (hero.Data.m_Rarity == rarity)
            {
                list.Add(hero.Data);
            }
        }
        return list[Random.Range(0,list.Count)];
    }

    public void SetHeroInfo()
    {
        var datas = Resources.LoadAll<Hero_Scriptable>("Scriptable/Hero");

        foreach (var data in datas)
        {
            Info h_info = new Info();
            if (Infos.ContainsKey(data.name))
            {
                h_info = Infos[data.name]; // ���� ������ �ִٸ� ������.
            }
            else
            {
                Infos.Add(data.name, h_info); // ���ٸ� ���� �߰�.
            }

            var hero = new HeroInfo();
            hero.Data = data;
            hero.info = h_info;

            HeroInfos.Add(data.name, hero);
        }
    }

    public void CheckInfo()
    {
        foreach (var hero in HeroInfos)
        {
            hero.Value.info = Infos[hero.Value.Data.name];
        }
    }
}

