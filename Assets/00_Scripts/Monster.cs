using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Monster : Character
{
    public float m_Speed;
    
    bool isSpawn = false;

    public double R_ATK, R_HP;
    public float R_Attack_Range;
    public bool isBoss = false;


    protected override void Start()
    {
        base.Start();
        ch_Mode = CH_Mode.Monster;        
    }

    public void Init()
    {
        isDead = false;
        HP = R_HP;
        ATK = R_ATK;
        Attack_Range = R_Attack_Range;
        target_Range = Mathf.Infinity;

        if(isBoss)
        {
            StartCoroutine(SetSkillCoroutine());
        }

        StartCoroutine(Spawn_Start());        
    }

    IEnumerator SetSkillCoroutine()
    {
        yield return new WaitForSeconds(3.0f);
        GetComponent<BaseSkill>().Set_Skill();
        StartCoroutine(SetSkillCoroutine());
    }

    IEnumerator Spawn_Start() // ������ ���� ����ȿ�� ũ�� 0 -> 1 �� ���� Ŀ���鼭 ����.
    {
        float current = 0.0f;
        float percent = 0.0f;
        float start = 0.0f;
        float end = transform.localScale.x;
        while(percent < 1)
        {
            current += Time.deltaTime;
            percent = current / 0.5f;
            float LerpPos = Mathf.Lerp(start, end, percent);
            transform.localScale = new Vector3(LerpPos, LerpPos, LerpPos);
            yield return null;
        }
        yield return new WaitForSeconds(0.3f);        
        isSpawn = true;
    }

    protected override void Bullet()
    {
        if (m_Target == null) { return; }

        BaseManager.Pool.Pooling_OBJ("Attack_Helper").Get((value) =>
        {
            value.transform.position = m_BulletTransform.position;
            value.GetComponent<Bullet>().Init(m_Target, ATK, "MO_BOSS");
        });
    }

    public override void GetDamage(double damage, bool isCritical)
    {
        bool critical = Critical(ref damage);
        base.GetDamage(damage, critical);

        if(isBoss)
        {
          MainUI.instance.BossSlider(HP, R_HP);
        }

        if (HP <= 0)
        {
            isDead = true;
            Dead_Event();
        }
    }
    private void Dead_Event()
    {
        if(!isBoss)
        { 
            StageManager.count++;
            MainUI.instance.MonsterSlider();
        }
        else
        {
            StageManager.State_Change(Stage_State.Clear);
        }
        
        Spawner.m_Monsters.Remove(this);

        var smokeObj = BaseManager.Pool.Pooling_OBJ("Smoke").Get((value) =>
        {
            value.transform.position = new Vector3(transform.position.x, 0.5f, transform.position.z);
            BaseManager.instance.Return_Pool(value.GetComponent<ParticleSystem>().main.duration, value, "Smoke");
        });

        BaseManager.Pool.Pooling_OBJ("COIN_PARENT").Get((value) =>
        {
            value.GetComponent<COIN_PARENT>().Init(transform.position);
        });

        BaseManager.Pool.Pooling_OBJ("Item_OBJ").Get((value) =>
        {
            value.GetComponent<Item_OBJ>().Init(transform.position);
        });

        if(!isBoss)
        {
            BaseManager.Pool.m_pool_Dictionary["Monster"].Return(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    
    private void Update()
    {
        if (isSpawn == false) { return; }

        if(StageManager.m_State == Stage_State.Play || StageManager.m_State == Stage_State.BossPlay) 
        { 

            if(m_Target == null)
            {
                FindClosetTarget(Spawner.m_Players.ToArray());
            }        

            if (m_Target == null)
            {
                m_Target = Spawner.m_Players[0].transform; // �ʹ� �ָ� ������ ��� ������ �÷��̾ ã�ư����� ����.
            }

            if (m_Target.GetComponent<Character>().isDead)
            {
                FindClosetTarget(Spawner.m_Players.ToArray());
            }

            float targetDistance = Vector3.Distance(transform.position, m_Target.position);
            if (targetDistance > Attack_Range && isATTACk == false)
            {
                AnimatorChange("isMOVE");
                transform.position = Vector3.MoveTowards(transform.position, m_Target.position, Time.deltaTime);
                transform.LookAt(m_Target.position);
            }
            else if (targetDistance <= Attack_Range && isATTACk == false)
            {
                isATTACk = true;
                AnimatorChange("isATTACK");
                Invoke("InitAttack", 1.0f);
            }
        }
    }

    private bool Critical(ref double damage)
    {
        float randValue = Random.Range(0.0f, 100.0f);
        if (randValue <= BaseManager.Player.Critical_Percentage)
        {
            damage *= (BaseManager.Player.Critical_rate/100);
            return true;
        }
        return false;
    }
}
