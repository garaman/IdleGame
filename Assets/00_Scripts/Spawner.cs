using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Spawner : MonoBehaviour
{    
    private int m_Count; // 한번에 스폰되는 몬스터 갯수
    private float m_SpawnTime; // 스폰 시간

    public static List<Monster> m_Monsters = new List<Monster>();
    public static List<Player> m_Players = new List<Player>();

    Coroutine coroutine;

    private void Start()
    {
        StageManager.m_ReadyEvent += OnReady;
        StageManager.m_PlayEvent += OnPlay;
        StageManager.m_BossEvent += OnBoss;
        StageManager.m_DeadEvent += OnDead;
    }

    public void OnReady()
    {
        m_Count = int.Parse(CSV_Importer.SpawnDesign[BaseManager.Data.Stage]["Spawn_Count"].ToString());
        m_SpawnTime = int.Parse(CSV_Importer.SpawnDesign[BaseManager.Data.Stage]["Spawn_Timer"].ToString());
    }
    public void OnPlay()
    {
        coroutine = StartCoroutine(SpawnCoroutine());
    }
    public void OnBoss()
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
            coroutine = null;
        }
        for(int i = 0; i < m_Monsters.Count; i++)
        {
            BaseManager.Pool.m_pool_Dictionary["Monster"].Return(m_Monsters[i].gameObject);            
        }
        m_Monsters.Clear();

        StartCoroutine(BossSetCoroutine());
    }
    public void OnDead()
    {
        if (m_Monsters.Count > 0)
        {
            foreach(var monster in m_Monsters)
            {
                monster.OnDead();
            }
        }
    }

    IEnumerator BossSetCoroutine()
    {
        yield return new WaitForSeconds(2.0f);
        var monster_Boss = Instantiate(Resources.Load<Monster>("PoolOBJ/BOSS"), Vector3.zero, Quaternion.Euler(0, 180, 0));
        monster_Boss.Init();

        Vector3 targetPos = monster_Boss.transform.position;

        for (int i = 0; i < m_Players.Count; i++)
        {
            if(Vector3.Distance(targetPos, m_Players[i].transform.position) <= 2.5f)
            {
                m_Players[i].transform.LookAt(targetPos);
                m_Players[i].KnockBack();
            }
        }
        yield return new WaitForSeconds(1.5f);
        m_Monsters.Add(monster_Boss);
        StageManager.State_Change(Stage_State.BossPlay);
    }

    IEnumerator SpawnCoroutine()
    {
        if (StageManager.m_State != Stage_State.Play) { yield return null; }
        
        Vector3 pos;
        int value = m_Count - m_Monsters.Count;

        for (int i = 0; i < value; i++)
        {
            pos = Vector3.zero + Random.insideUnitSphere * 5.0f;
            pos.y = 0.0f;
            
            while(Vector3.Distance(pos, Vector3.zero) <= 3.0f)
            {
                pos = Vector3.zero + Random.insideUnitSphere * 5.0f;
                pos.y = 0.0f;
            }

            //Pool
            var goObj = BaseManager.Pool.Pooling_OBJ("Monster").Get((value) =>
            { 
                value.GetComponent<Monster>().Init();
                value.transform.position = pos;
                value.transform.LookAt(Vector3.zero);
                m_Monsters.Add(value.GetComponent<Monster>());
            });
                        
        }

        yield return new WaitForSeconds(m_SpawnTime);

        coroutine = StartCoroutine(SpawnCoroutine());
    }

}
