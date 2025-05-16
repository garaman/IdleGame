using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_Barbarian : BaseSkill
{
    public GameObject skillEffect;
    public override void Set_Skill()
    {
        m_Character.AnimatorChange("isSKILL");
        skillEffect.SetActive(true);
        StartCoroutine(SetSkill_Coroutine());
        base.Set_Skill();        
    }

    IEnumerator SetSkill_Coroutine()
    {
        for(int i = 0; i < 5; i++)
        {
            for(int j = 0; j < monsters.Length; j++)
            {
                if(Distance(transform.position, monsters[j].transform.position, 1.5f))
                {
                    monsters[j].GetDamage(SkillDamage(130), true);
                }
            }
            yield return new WaitForSeconds(0.5f);
        }
        skillEffect.SetActive(false);
        ReturnSkill();
    }
}
