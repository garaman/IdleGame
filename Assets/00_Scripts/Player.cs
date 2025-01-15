using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    Vector3 startPos;
    Quaternion rot;
    protected override void Start()
    {
        base.Start();

        startPos = transform.position;
        rot = transform.rotation;
    }

    private void Update()
    {
        FindClosetTarget(Spawner.m_Monsters.ToArray());
        if (m_Target == null)
        {
            float targetPos = Vector3.Distance(transform.position, startPos);
            if (targetPos > 0.1f)
            {
                transform.position = Vector3.MoveTowards(transform.position, startPos, Time.deltaTime);
                transform.LookAt(startPos);
                AnimatorChange("isMOVE");
            }
            else
            {
                transform.rotation = rot;
                AnimatorChange("isIDLE");
            }
            return;
        }

        if(m_Target.GetComponent<Character>().isDead)
        {
            FindClosetTarget(Spawner.m_Monsters.ToArray());
        }

        float targetDistance = Vector3.Distance(transform.position, m_Target.position);
        if (targetDistance <= target_Range && targetDistance > Attack_Range && isATTACk == false) // 추격범위 O , 공격범위 X
        {
            AnimatorChange("isMOVE");
            transform.position = Vector3.MoveTowards(transform.position, m_Target.position, Time.deltaTime);
            transform.LookAt(m_Target.position);
        }
        else if(targetDistance <= Attack_Range && isATTACk == false)
        {
            isATTACk = true;
            AnimatorChange("isATTACK");
            Invoke("InitAttack", 1.0f);            
        }
    }

    
}
