using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    Animator animator;
    
    public double HP;
    public double ATK;
    public float ATK_Speed;
    public bool isDead = false;
    
    protected float Attack_Range = 3.0f;// 공격 범위
    protected float target_Range = 5.0f;// 추격 범위
    protected bool isATTACk = false;

    protected Transform m_Target;

    [SerializeField] private Transform m_BulletTransform;
    protected virtual void Start()
    {
        animator = GetComponent<Animator>();
    }
    protected void InitAttack() => isATTACk = false;

    protected void AnimatorChange(string temp)
    {
        if(temp == "isATTACK")
        {
            animator.SetTrigger("isATTACK");
            return;
        }
        animator.SetBool("isIDLE", false);
        animator.SetBool("isMOVE", false);

        animator.SetBool(temp, true);
    }

    protected void FindClosetTarget<T>(T[] targets) where T : Component
    {
        var monsters = targets;
        Transform closetTarget = null;
        float maxDistance = target_Range;

        foreach (var monster in monsters) 
        {
            float targetDistance = Vector3.Distance(transform.position, monster.transform.position);

            if(targetDistance < maxDistance)
            {
                closetTarget = monster.transform;
                maxDistance = targetDistance;
            }
        }

        m_Target = closetTarget;
        if (m_Target != null)
        {
            transform.LookAt(m_Target.position);
        }
    }

    protected virtual void Bullet()
    {
        if(m_Target == null) { return; }

        BaseManager.Pool.Pooling_OBJ("Bullet").Get((value) => 
        {
            value.transform.position = m_BulletTransform.position;
            value.GetComponent<Bullet>().Init(m_Target, 10, "CH_01");
        });
    }
}
