using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseManager : MonoBehaviour
{
    public static BaseManager instance = null;

    #region Managers
    private static PoolManager s_pool = new PoolManager();
    private static PlayerManager s_player = new PlayerManager();
    private static DataManager s_data = new DataManager();
    private static ItemManager s_item = new ItemManager();
    private static HeroManager s_hero = new HeroManager();
    private static InventoryManager s_inventory = new InventoryManager();
    private static ADSManager s_ADS = new ADSManager();
    private static FirebaseManager s_firebase = new FirebaseManager();
    private static RelicManager s_relic = new RelicManager();
    public static PoolManager Pool { get { return s_pool; } }
    public static PlayerManager Player { get { return s_player; } }
    public static DataManager Data { get { return s_data; } }
    public static ItemManager Item { get { return s_item; } }
    public static HeroManager Hero { get { return s_hero; } }
    public static InventoryManager Inventory { get { return s_inventory; } }
    public static ADSManager ADS { get { return s_ADS; } }
    public static FirebaseManager Firebase { get { return s_firebase; } }
    public static RelicManager Relic { get { return s_relic; } }
    #endregion

    public static bool isFast = false;
    public static bool isGameStart = false;

    float SaveTimer = 0.0f;

    private void Awake()
    {
        Initailize();
    }

    private void Update()
    {
        if(isGameStart == false)  { return; }

        for (int i = 0; i < DataManager.gameData.Buffer_Timer.Length; i++)
        {
            if (DataManager.gameData.Buffer_Timer[i] > 0.0f)
            {
                DataManager.gameData.Buffer_Timer[i] -= Time.deltaTime;
            }
        }
        if(DataManager.gameData.Buffe_x2 > 0.0f)
        {
            DataManager.gameData.Buffe_x2 -= Time.deltaTime;
        }

        SaveTimer += Time.unscaledDeltaTime;
        if (SaveTimer >= 10.0f)
        {
            SaveTimer = 0.0f;
            Firebase.WriteData();
        }
    }

    private void Initailize()
    {
        if (instance == null)
        {
            instance = this;

            Pool.Init(this.transform);
            ADS.Init();
            Firebase.Init();                        

            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }


    private void GetReward() => Debug.Log("보상형 광고를 시청하고, 보상을 획득하였습니다.");

    public GameObject Instantiate_Path(string path)
    {
        return Instantiate(Resources.Load<GameObject>(path));
    }

    public void Return_Pool(float timer, GameObject obj, string path)
    {
        StartCoroutine(Return_PoolCoroutine(timer, obj, path));
    }

    public void ActionCoroutine_Start(Action action, float timer)
    {
        StartCoroutine(Action_Coroutine(action, timer));
    }

    IEnumerator Return_PoolCoroutine(float timer, GameObject obj, string path)
    {
        yield return new WaitForSeconds(timer);
        Pool.m_pool_Dictionary[path].Return(obj);
    }

    IEnumerator Action_Coroutine(Action action, float timer)
    {
        yield return new WaitForSeconds(timer);
        action?.Invoke();
    }

    private void OnDestroy()
    {
        if(isGameStart)
        {
            Firebase.WriteData();
        }        
    }
}
