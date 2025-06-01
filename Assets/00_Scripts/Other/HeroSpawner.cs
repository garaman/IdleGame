using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroSpawner : MonoBehaviour
{
    public Transform[] SpawnTransform = new Transform[6];
    public static Player[] players = new Player[6];
    

    private void Awake()
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            SpawnTransform[i] = transform.GetChild(i);    
        }

        StageManager.m_ReadyEvent += SetHero;
    }

    private void SetHero()
    {
        for(int i = 0; i < BaseManager.Data.SetHeroData.Length; i++)
        {
            var data = BaseManager.Data.SetHeroData[i];
            if (data != null)
            {
                if (players[i] != null)
                {
                    if (players[i].P_Data != data)
                    {
                        Destroy(players[i].gameObject);
                        players[i] = null;
                        MakeHero(data, i);
                    }
                }
                else
                {                   
                    MakeHero(data, i);                   
                }

            }
        }
    }

    private void MakeHero(Hero_Scriptable data, int value)
    {
        string temp = data.m_Character_Name;
        GameObject go = Instantiate(Resources.Load<GameObject>("Hero/" + temp));
        players[value] = go.GetComponent<Player>();

        go.transform.SetParent(SpawnTransform[value]);
        go.transform.localPosition = SpawnTransform[value].position;
        go.transform.LookAt(Vector3.zero);        
    }
}
