using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class COIN_PARENT : MonoBehaviour
{
    Vector3 target;
    Camera cam;
    RectTransform[] childs = new RectTransform[5];

    [Range(0.0f, 500.0f)]
    [SerializeField] private float m_DistanceRange, speed;

    private void Awake()
    {
        cam = Camera.main;
        for (int i = 0; i < childs.Length; i++)
        {
            childs[i] = transform.GetChild(i).GetComponent<RectTransform>();
        }
       
    }
    public void OnSave()
    {
       DataManager.gameData.Money += Utils.DesignData.levelData.MONEY();
        if (Distanc_Boolean_World(0.5f))
        {
            BaseManager.Pool.m_pool_Dictionary["COIN_PARENT"].Return(this.gameObject);
        }
    }

    private void OnDisable()
    {
        UI_SavingMode.m_OnSving -= OnSave;        
    }

    public void Init(Vector3 pos)
    {
        UI_SavingMode.m_OnSving += OnSave;

        if (BaseCanvas.isSave) { return; }

        target = pos;        
        transform.position = cam.WorldToScreenPoint(target);

        for (int i = 0; i < childs.Length; i++)
        {
            childs[i].anchoredPosition = Vector2.zero;
        }
        transform.SetParent(BaseCanvas.instance.HOLDER_LAYER(0));

       DataManager.gameData.Money += Utils.DesignData.stageData.MONEY();

        StartCoroutine(Coin_Effect());
    }

    IEnumerator Coin_Effect()
    {
        Vector2[] RandomPos = new Vector2[childs.Length];
        for(int i = 0;i < RandomPos.Length;i++)
        {
            RandomPos[i] = new Vector2(target.x, target.y) + Random.insideUnitCircle * Random.Range(-m_DistanceRange, m_DistanceRange);
        }

        while(true)
        {
            for(int i = 0;i < childs.Length;i++)
            {
                RectTransform rect = childs[i];

                rect.anchoredPosition = Vector2.MoveTowards(rect.anchoredPosition, RandomPos[i], Time.deltaTime * speed);
            }

            if (Distanc_Boolean(RandomPos, 0.5f))
            {
                break;
            }

            yield return null;            
        }


        yield return new WaitForSeconds(0.3f);

        while (true)
        {
            for (int i = 0; i < childs.Length; i++)
            {
                RectTransform rect = childs[i];
                rect.position = Vector2.MoveTowards(rect.position, BaseCanvas.instance.COIN.position, Time.deltaTime * (speed * 5.0f));
            }
            
            if(Distanc_Boolean_World(0.5f))
            {
                BaseManager.Pool.m_pool_Dictionary["COIN_PARENT"].Return(this.gameObject);
                break;
            }

            yield return null;
        }

        MainUI.instance.TextCheck();
    }

    private bool Distanc_Boolean(Vector2[] end, float range)
    {
        for (int i = 0; i < childs.Length; i++)
        {
            float distance = Vector2.Distance(childs[i].anchoredPosition, end[i]);
            if(distance > range)
            {
                return false;
            }
        }
        return true;
    }

    private bool Distanc_Boolean_World(float range)
    {
        for (int i = 0; i < childs.Length; i++)
        {
            float distance = Vector2.Distance(childs[i].position, BaseCanvas.instance.COIN.position);
            if (distance > range)
            {
                return false;
            }
        }
        return true;
    }
}
