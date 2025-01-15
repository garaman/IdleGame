using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Spawner : MonoBehaviour
{    
    public int m_Count; // 한번에 스폰되는 몬스터 갯수
    public float m_SpawnTime; // 스폰 시간

    public static List<Monster> m_Monsters = new List<Monster>();
    public static List<Player> m_Players = new List<Player>();

    private void Start()
    {
        StartCoroutine(SpawnCoroutine());
    }

    IEnumerator SpawnCoroutine()
    {
        Vector3 pos;

        for (int i = 0; i < m_Count; i++)
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

        StartCoroutine(SpawnCoroutine());
    }

}
