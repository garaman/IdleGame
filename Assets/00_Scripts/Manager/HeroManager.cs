using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroManager 
{
    // 사용중인 영웅 정보.
    public HeroInfo[] SetHeroInfos = new HeroInfo[6];    

    public void GetHero(int value, string heroName)
    {
       SetHeroInfos[value] = BaseManager.Data.HeroInfos[heroName];
    }
}
