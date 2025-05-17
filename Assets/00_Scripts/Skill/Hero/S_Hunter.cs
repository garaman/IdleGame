using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_Hunter : BaseSkill
{
    public override void Set_Skill()
    {
        base.Set_Skill();
        skill_Coroutine = StartCoroutine(SetSkill_Coroutine());
    }

    IEnumerator SetSkill_Coroutine()
    {
        m_Character.ATK_Speed = 2.0f;
        SkillParticle.SetActive(true);
        yield return new WaitForSeconds(5.0f);
        m_Character.ATK_Speed = 1.0f;
        SkillParticle.SetActive(false);
        ReturnSkill();
    }
}
