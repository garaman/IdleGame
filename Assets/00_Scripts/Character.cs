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
    protected CH_Mode ch_Mode;

    [SerializeField] protected Transform m_BulletTransform;
    protected virtual void Start()
    {
        animator = GetComponent<Animator>();
    }
    protected void InitAttack() => isATTACk = false;

    protected void AnimatorChange(string temp)
    {
        animator.SetBool("isIDLE", false);
        animator.SetBool("isMOVE", false);

        if (temp == "isATTACK" || temp == "isCLEAR" || temp == "isDEAD")
        {
            animator.SetTrigger(temp);
            return;
        }       

        animator.SetBool(temp, true);
    }

    protected void FindClosetTarget<T>(T[] targetArray) where T : Component
    {
        var targets = targetArray;
        Transform closetTarget = null;
        float maxDistance = target_Range;

        foreach (var target in targets) 
        {
            float targetDistance = Vector3.Distance(transform.position, target.transform.position);

            if(targetDistance < maxDistance)
            {
                closetTarget = target.transform;
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

        BaseManager.Pool.Pooling_OBJ("Attack_Helper").Get((value) => 
        {
            value.transform.position = m_BulletTransform.position;
            value.GetComponent<Bullet>().Init(m_Target, ATK, "CH_01");
        });
    }

    protected virtual void Attack()
    {
        if (m_Target == null) { return; }

        BaseManager.Pool.Pooling_OBJ("Attack_Helper").Get((value) =>
        {
            value.transform.position = m_Target.position;
            value.GetComponent<Bullet>().Attack_Init(m_Target, ATK);
        });
    }

    public virtual void GetDamage(double damage, bool isCritical = false)
    {
        if (isDead == true) { return; }

        BaseManager.Pool.Pooling_OBJ("HIT_TEXT").Get((value) =>
        {
            value.GetComponent<HIT_TEXT>().Init(transform.position, damage, isCritical, (ch_Mode == CH_Mode.Player)?true:false);
        });

        HP -= damage;
    }
}
