using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseSkill : MonoBehaviour
{
    public GameObject SkillParticle;
    protected Monster[] monsters { get { return Spawner.m_Monsters.ToArray(); } }
    protected Player[] players { get { return Spawner.m_Players.ToArray(); } }
    protected Character m_Character { get { return GetComponent<Character>(); } }

    protected Coroutine skill_Coroutine;
    protected double ATK;

    private void Start()
    {
        StageManager.m_DeadEvent += onDead;
    }

    public virtual void Set_Skill()
    {

    }


    protected double SkillDamage(double value)
    {
        return m_Character.ATK * (value / 100.0f);
    }
    protected bool Distance(Vector3 startPos, Vector3 endPos, float distance)
    {
        float targetDistance = Vector3.Distance(startPos, endPos);
        if (targetDistance <= distance)
        {
            return true;
        }
        return false;
    }

    private void onDead()
    {
        if (skill_Coroutine != null)
        {
            StopCoroutine(skill_Coroutine);
            skill_Coroutine = null;
        }
    }

    public virtual void ReturnSkill()
    {        
        m_Character.NonAttackSkill = false;
        m_Character.isGetSkill = false;
        m_Character.AnimatorChange("isIDLE");
        skill_Coroutine = null;
    }
}
