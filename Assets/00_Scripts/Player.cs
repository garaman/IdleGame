using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEditor.Tilemaps;
using UnityEngine;

public class Player : Character
{
    public Hero_Scriptable P_Data;
    public ParticleSystem Provocation_Effect;
    public GameObject TrailObj;
    public string CH_Name;    
    public float MP;
    Vector3 startPos;
    Quaternion rot;

    public bool isMainHero = false;
    

    protected override void Start()
    {
        base.Start();
        ch_Mode = CH_Mode.Player;
        Data_Set(Resources.Load<Hero_Scriptable>("Scriptable/Hero/"+CH_Name));
        Spawner.m_Players.Add(this);

        StageManager.m_ReadyEvent += OnReady;
        StageManager.m_BossEvent += OnBoss;
        StageManager.m_ClearEvent += OnClear;

        startPos = transform.position;
        rot = transform.rotation;
    }

    private void Data_Set(Hero_Scriptable data)
    {
        P_Data = data;
        Bullet_Name = data.m_Character_Name;
        Attack_Range = data.m_Attack_Range;
        ATK_Speed = data.m_Attack_Speed;

        Set_Status();
    }

    public void Set_Status()
    {
        ATK = BaseManager.Player.Get_ATK(P_Data.m_Rarity);
        HP = BaseManager.Player.Get_HP(P_Data.m_Rarity);
        MaxHP = BaseManager.Player.Get_HP(P_Data.m_Rarity);
    }

    private void OnReady()
    {
        AnimatorChange("isIDLE");
        if(isDead) 
        { 
            Spawner.m_Players.Add(this);
            Set_Status();
        }  
        isDead = false;
        transform.position = startPos;
        transform.rotation = rot;
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

    public void GetMp(float mp)
    {
        if(isGetSkill) { return; }
        if (isMainHero) { return; }   
        
        MP += mp;
        if(MP >= P_Data.Max_Mp)
        {
            if(GetComponent<BaseSkill>() != null)
            {                
                GetComponent<BaseSkill>().Set_Skill();
                isGetSkill = true;
                MP = 0;
            }
            else
            {   
                MP = P_Data.Max_Mp;
            }
        }
        MainUI.instance.HeroStateCheck(this);
    }

    private void Update()
    {
        if(isDead) { return; }        

        if (StageManager.m_State == Stage_State.Play || StageManager.m_State == Stage_State.BossPlay)
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
                    GetMp(5.0f);
                    AnimatorChange("isATTACK");                    
                    Invoke("InitAttack", 1.0f / ATK_Speed);            
                }
            }
        }
    }    

    public void KnockBack()
    {        
        StartCoroutine(KnockBack_Coroutine(3.0f, 0.3f));
    }

    IEnumerator KnockBack_Coroutine(float power, float duration)
    {
       float time = duration;
        Vector3 force = this.transform.forward * -power;
        force.y = 0.0f;

        while (time > 0f)
        {
            time -= Time.deltaTime;            
            if(Vector3.Distance(Vector3.zero, transform.position) < 3.0)
            {
                transform.position += force * Time.deltaTime;
            }
            yield return null;
        }
    }
    public override void GetDamage(double damage, bool isCritical = false)
    {
        if(StageManager.isDead == true) { return; }

        base.GetDamage(damage);
        
        GetMp(3.0f);

        if (HP <= 0)
        {
            isDead = true;
            DeadEvent();
        }
    }

    private void DeadEvent()
    {
        Spawner.m_Players.Remove(this);
        //Debug.Log(Spawner.m_Players.Count);       
        
        AnimatorChange("isDEAD");
        m_Target = null;

        if (Spawner.m_Players.Count <= 0 && StageManager.isDead == false)
        {
            StageManager.State_Change(Stage_State.Dead);
        }
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
