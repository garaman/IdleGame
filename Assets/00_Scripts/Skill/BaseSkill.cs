using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseSkill : MonoBehaviour
{
    protected Monster[] monsters { get { return Spawner.m_Monsters.ToArray(); } }
    protected Player[] players { get { return Spawner.m_Players.ToArray(); } }

    private void Start()
    {
        StageManager.m_DeadEvent += onDead;
    }

    public virtual void Set_Skill()
    {
        
    }

    private void onDead()
    {
        StopAllCoroutines();
    }

}
