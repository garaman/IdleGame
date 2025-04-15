using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEditor.Tilemaps;
using UnityEngine;

public class Player : Character
{
    private Character_Scriptable CH_Data;
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
