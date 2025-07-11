using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelDesignData", menuName = "LevelDesign/Level Design Data")]
public class LevelDesign : ScriptableObject
{
    public int currentLevel;
    public int currentStage;

    [Space(10f)]
    public LevelData levelData;
    public StageData stageData;
}

[System.Serializable]
public class LevelData
{    
    public float C_ATK, C_HP, C_EXP, C_MAXEXP, C_MONEY;

    [Space(10f)]
    public int B_ATK;
    public int B_HP;
    public int B_EXP;
    public int B_MAXEXP;
    public int B_MONEY;

    public double ATK() => Utils.CalculatedValue(B_ATK,DataManager.gameData.Level, C_ATK);
    public double HP() => Utils.CalculatedValue(B_HP,DataManager.gameData.Level, C_HP);
    public double EXP() => Utils.CalculatedValue(B_EXP,DataManager.gameData.Level, C_EXP);
    public double MAXEXP() => Utils.CalculatedValue(B_MAXEXP,DataManager.gameData.Level, C_MAXEXP);
    public double MONEY() => Utils.CalculatedValue(B_MONEY,DataManager.gameData.Level, C_MONEY);

    public double SetATK(int level) => Utils.CalculatedValue(B_ATK,level, C_ATK);
    public double SetHP(int level) => Utils.CalculatedValue(B_HP, level, C_HP);
}

[System.Serializable]
public class StageData
{    
    public float M_ATK, M_HP, M_MONEY;

    [Space(10f)]
    public int B_ATK;
    public int B_HP;
    public int B_MONEY;

    public double ATK() => Utils.CalculatedValue(B_ATK,DataManager.gameData.Stage, M_ATK);
    public double HP() => Utils.CalculatedValue(B_HP,DataManager.gameData.Stage, M_HP);
    public double MONEY() => Utils.CalculatedValue(B_MONEY,DataManager.gameData.Stage, M_MONEY);
}
