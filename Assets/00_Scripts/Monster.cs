using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Monster : Character
{
    public float m_Speed;
    
    bool isSpawn = false;

    protected override void Start()
    {
        base.Start();
        ch_Mode = CH_Mode.Monster;        
    }

    public void Init()
    {
        isDead = false;
        HP = 20;
        ATK = 10;
        Attack_Range = 0.5f;
        target_Range = Mathf.Infinity;
        StartCoroutine(Spawn_Start());        
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

    public override void GetDamage(double damage, bool isCritical)
    {
        bool critical = Critical(ref damage);
        base.GetDamage(damage, critical);

        if (HP <= 0)
        {
            isDead = true;
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

            BaseManager.Pool.m_pool_Dictionary["Monster"].Return(this.gameObject);
        }
    }


    private void Update()
    {
        if (isSpawn == false) { return; }

        FindClosetTarget(Spawner.m_Players.ToArray());

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

    IEnumerator ReturnCoroutine(float timer, GameObject obj, string path)
    {
        yield return new WaitForSeconds(timer);
        BaseManager.Pool.m_pool_Dictionary[path].Return(obj);
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
