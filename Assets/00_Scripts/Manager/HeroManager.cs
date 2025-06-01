using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroManager 
{
    // 사용중인 영웅 정보.
    //public Hero_Scriptable[] SetHeroData = new Hero_Scriptable[6];    

    public void GetHero(int value, string heroName)
    {
       BaseManager.Data.SetHeroData[value] = BaseManager.Data.HeroData[heroName];
    }

    public void DisableHero(int value)
    {
        BaseManager.Data.SetHeroData[value] = null; 
    }
    
    public int LevelMaxCount(Info info)
    {
        return (info.Level + 1) * 5;
    }
}
