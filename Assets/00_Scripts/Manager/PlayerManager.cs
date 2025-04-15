using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager
{
    public int Level;
    public double EXP;
    public double ATK = 10;
    public double HP = 50;

    public float Critical_Percentage = 20.0f;
    public double Critical_rate = 140.0d;
    
    public void EXP_UP()
    {
        EXP += float.Parse(CSV_Importer.Exp[Level]["Get_EXP"].ToString());
        if(EXP >= float.Parse(CSV_Importer.Exp[Level]["EXP"].ToString()))
        {
            Level++;
            ATK += Next_ATK();
            HP += Next_HP();
            MainUI.instance.TextCheck();

            for (int i = 0; i < Spawner.m_Players.Count; i++)
            {
                Spawner.m_Players[i].Set_Status();
            }
        }
    }

    public float EXP_percentage()
    {
        float exp = float.Parse(CSV_Importer.Exp[Level]["EXP"].ToString());
        double currentExp = EXP;
        if (Level >= 1)
        {
            float preEXP = float.Parse(CSV_Importer.Exp[Level - 1]["EXP"].ToString());
            exp -= preEXP;
            currentExp -= preEXP;
        }
        return (float)currentExp/exp;
    }

    public float Next_EXP()
    {
        float exp = float.Parse(CSV_Importer.Exp[Level]["EXP"].ToString());
        float getExp = float.Parse(CSV_Importer.Exp[Level]["Get_EXP"].ToString());
        if (Level >= 1)
        {            
            exp -= float.Parse(CSV_Importer.Exp[Level - 1]["EXP"].ToString());         
        }
        return getExp / exp * 100.0f;
    }

    public double Next_ATK()
    {
        return float.Parse(CSV_Importer.Exp[Level]["Get_EXP"].ToString()) * (Level + 1) / 5;
    }

    public double Next_HP()
    {
        return float.Parse(CSV_Importer.Exp[Level]["Get_EXP"].ToString()) * (Level + 1) / 3;
    }

    public double Get_ATK(Rarity rarity)
    {
        return ATK * ((int)rarity + 1);
    }

    public double Get_HP(Rarity rarity)
    {
        return HP * ((int)rarity + 1);
    }

    public double Get_FightScore()
    {
        return ATK + HP;
    }
}
