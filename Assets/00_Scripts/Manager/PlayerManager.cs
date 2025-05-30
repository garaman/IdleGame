using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerManager
{
    public double ATK = 10;
    public double HP = 50;

    public float Critical_Percentage = 20.0f;
    public double Critical_rate = 140.0d;
    
    public void SetLevelState()
    {
        for (int i = 0; i < DataManager.gameData.Level; i++)
        {
            ATK += Utils.DesignData.levelData.SetATK(i);
            HP += Utils.DesignData.levelData.SetHP(i);
        }
    }

    public void EXP_UP()
    {
        if(DataManager.gameData.Money < Get_MONEY()) { return; }

       DataManager.gameData.Money -= Get_MONEY();
       DataManager.gameData.EXP += Get_EXP();

        if(DataManager.gameData.EXP >= Get_MAXEXP())
        {
           DataManager.gameData.Level++;
            ATK += Get_ATK();
            HP += Get_HP();
           DataManager.gameData.EXP = 0;
            //MainUI.instance.TextCheck();

            for (int i = 0; i < Spawner.m_Players.Count; i++)
            {
                Spawner.m_Players[i].Set_Status();
            }
        }
        MainUI.instance.TextCheck();
    }

    public float EXP_percentage()
    {
        float exp = (float)Get_MAXEXP();
        double currentExp =DataManager.gameData.EXP;
        
        return (float)currentExp/exp;
    }

    public float Next_EXP()
    {
        float exp = (float)Get_MAXEXP();
        float getExp = (float)Get_EXP();
        
        return getExp / exp * 100.0f;
    }

    public double Get_EXP()
    {
        return Utils.DesignData.levelData.EXP();
    }
    public double Get_MAXEXP()
    {
        return Utils.DesignData.levelData.MAXEXP();
    }
    public double Get_MONEY()
    {
        return Utils.DesignData.levelData.MONEY();
    }
     

    public double Get_ATK()
    {
        return Utils.DesignData.levelData.ATK();
    }

    public double Get_HP()
    {
        return Utils.CalculatedValue(Utils.DesignData.levelData.B_HP,DataManager.gameData.Level, Utils.DesignData.levelData.C_HP);
    }

    public double Get_ATK(Rarity rarity, HeroInfo hero)
    {
        double Base = ATK * ((int)rarity + 1);
        int baseLevel = (hero.info.Level + 1);

        float Level = (baseLevel * 10) / 100;
        double Damege = Base + (Base * Level);

        return Damege;
    }

    public double Get_HP(Rarity rarity, HeroInfo hero)
    {
        double Base = HP * ((int)rarity + 1);
        int baseLevel = (hero.info.Level + 1);

        float Level = (baseLevel * 10) / 100;
        double hp = Base + (Base * Level);

        return hp;
    }

    public double Get_FightScore()
    {
        return ATK + HP;
    }
}
