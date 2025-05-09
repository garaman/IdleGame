using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEditor.Tilemaps;
using UnityEngine;

public class Player : Character
{
    private Character_Scriptable CH_Data;
    public ParticleSystem Provocation_Effect;
    public GameObject TrailObj;
    public string CH_Name;
    Vector3 startPos;
    Quaternion rot;

    protected override void Start()
    {
        base.Start();
        ch_Mode = CH_Mode.Player;
        Data_Set(Resources.Load<Character_Scriptable>("Scriptable/"+CH_Name));
        Spawner.m_Players.Add(this);

        StageManager.m_ReadyEvent += OnReady;
        StageManager.m_BossEvent += OnBoss;
        StageManager.m_ClearEvent += OnClear;

        startPos = transform.position;
        rot = transform.rotation;
    }

    private void Data_Set(Character_Scriptable data)
    {
        CH_Data = data;
        Attack_Range = data.m_Attack_Range;

        Set_Status();
    }

    public void Set_Status()
    {
        ATK = BaseManager.Player.Get_ATK(CH_Data.m_Rarity);
        HP = BaseManager.Player.Get_HP(CH_Data.m_Rarity);
    }

    private void OnReady()
    {
        AnimatorChange("isIDLE");
        transform.position = startPos;
    }
    private void OnBoss()
    {
        AnimatorChange("isIDLE");
        Provocation_Effect.Play();
    }
    private void OnClear()
    {
        AnimatorChange("isCLEAR"); 
        StartCoroutine(Clear_Time(2.0f));
    }

    IEnumerator Clear_Time(float time)
    {
        yield return new WaitForSeconds(time);
        AnimatorChange("isIDLE");
    }

    private void Update()
    {
        if(StageManager.m_State == Stage_State.Play || StageManager.m_State == Stage_State.BossPlay)
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
            else 
            {
                if (Spawner.m_Monsters.Count > 0)
                {
                    if (m_Target.GetComponent<Character>().isDead)
                    {
                        FindClosetTarget(Spawner.m_Monsters.ToArray());
                    }
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
    }    

    public void KnockBack()
    {        
        StartCoroutine(KnockBack_Coroutine(4.0f, 0.5f));
    }

    IEnumerator KnockBack_Coroutine(float power, float duration)
    {
       float time = duration;
        Vector3 force = this.transform.forward * -power;
        force.y = 0.0f;

        while (time > 0f)
        {
            time -= Time.deltaTime;
            transform.position += force * Time.deltaTime;
            yield return null;
        }
    }
    public override void GetDamage(double damage, bool isCritical = false)
    {
        base.GetDamage(damage);
    }

    protected override void Attack()
    {
        base.Attack();
        TrailObj.SetActive(true);

        Invoke("TrailDisable",1.0f);
    }

    private void TrailDisable()
    {
        TrailObj.SetActive(false);
    }
}
