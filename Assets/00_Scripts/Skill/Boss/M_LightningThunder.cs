using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_LightningThunder : BaseSkill
{
    private int m_SkillCount = 5;
    
    public override void Set_Skill()
    {
        base.Set_Skill();
        ATK = Utils.DesignData.stageData.ATK();
        skill_Coroutine = StartCoroutine(M_SkillCoroutine());
    }

    IEnumerator M_SkillCoroutine()
    {
        double damege = ATK / (m_SkillCount / 2);

        for (int i = 0; i < m_SkillCount; i++)
        {
            Player player = players[Random.Range(0,players.Length)];
            
            Instantiate(Resources.Load("PoolOBJ/Skill/M_LightningThunder"), player.transform.position, Quaternion.identity);

            CameraManager.instance.CameraShake();

            player.GetDamage(damege);
            yield return new WaitForSeconds(0.2f);
        }

        skill_Coroutine = null;
    }
}
