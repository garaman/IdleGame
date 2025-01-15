using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseManager : MonoBehaviour
{
    public static BaseManager instance = null;

    private static PoolManager pool = new PoolManager();
    public static PoolManager Pool {  get { return pool; } } 

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

    IEnumerator Return_PoolCoroutine(float timer, GameObject obj, string path)
    {
        yield return new WaitForSeconds(timer);
        Pool.m_pool_Dictionary[path].Return(obj);
    }
}
