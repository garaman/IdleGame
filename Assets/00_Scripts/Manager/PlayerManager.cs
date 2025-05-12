using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager
{
    public double ATK = 10;
    public double HP = 50;

    public float Critical_Percentage = 20.0f;
    public double Critical_rate = 140.0d;
    
    public void EXP_UP()
    {
        if(BaseManager.Data.Money < Get_MONEY()) { return; }

        BaseManager.Data.Money -= Get_MONEY();
        BaseManager.Data.EXP += Get_EXP();

        if(BaseManager.Data.EXP >= Get_MAXEXP())
        {
            BaseManager.Data.Level++;
            ATK += Get_ATK();
            HP += Get_HP();
            BaseManager.Data.EXP = 0;
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
        double currentExp = BaseManager.Data.EXP;
        
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
        return Utils.CalculatedValue(Utils.DesignData.levelData.B_HP, BaseManager.Data.Level, Utils.DesignData.levelData.C_HP);
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
