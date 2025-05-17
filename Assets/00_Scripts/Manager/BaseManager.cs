using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class BaseManager : MonoBehaviour
{
    public static BaseManager instance = null;

    #region Managers
    private static PoolManager s_pool = new PoolManager();
    private static PlayerManager s_player = new PlayerManager();    
    private static GameDataManager s_data = new GameDataManager();
    private static ItemManager s_item = new ItemManager();
    private static HeroManager s_hero = new HeroManager();
    public static PoolManager Pool {  get { return s_pool; } } 
    public static PlayerManager Player {  get { return s_player; } }    
    public static GameDataManager Data { get { return s_data; } }
    public static ItemManager Item { get { return s_item; } }
    public static HeroManager Hero { get { return s_hero; } }
    #endregion

    private void Awake()
    {
        Initailize();
    }

    private void Initailize()
    {
        if(instance == null)
        {
            instance = this;
            Pool.Init(this.transform);
            Data.Init();
            Item.Init();

            Hero.GetHero(1, "Hunter");

            ActionCoroutine_Start(() => StageManager.State_Change(Stage_State.Ready), 0.5f);
            
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

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
}
