using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Monster : Character
{
    public float m_Speed;
    
    bool isSpawn = false;

    protected override void Start()
    {
        base.Start();
        HP = 5;
    }

    public void Init()
    {
        isDead = false;
        HP = 5;

        StartCoroutine(Spawn_Start());        
    }

    IEnumerator Spawn_Start() // 스폰시 몬스터 등장효과 크기 0 -> 1 로 점점 커지면서 등장.
    {
        float current = 0.0f;
        float percent = 0.0f;
        float start = 0.0f;
        float end = transform.localScale.x;
        while(percent < 1)
        {
            current += Time.deltaTime;
            percent = current / 0.5f;
            float LerpPos = Mathf.Lerp(start, end, percent);
            transform.localScale = new Vector3(LerpPos, LerpPos, LerpPos);
            yield return null;
        }
        yield return new WaitForSeconds(0.3f);        
        isSpawn = true;
    }

    public void GetDaamage(double damage)
    {
        if (isDead == true) { return; }
        HP -= damage;

        if (HP <= 0)
        {
            isDead = true;
            Spawner.m_Monsters.Remove(this);

            var smokeObj = BaseManager.Pool.Pooling_OBJ("Smoke").Get((value) => 
            { 
                value.transform.position = new Vector3(transform.position.x, 0.5f, transform.position.z);
                BaseManager.instance.Return_Pool(value.GetComponent<ParticleSystem>().main.duration, value, "Smoke");                
            });

            BaseManager.Pool.m_pool_Dictionary["Monster"].Return(this.gameObject);
        }
    }


    private void Update()
    {        
        transform.LookAt(Vector3.zero);

        if(isSpawn == false) { return; }

        float targetDistance = Vector3.Distance(transform.position, Vector3.zero);
        if(targetDistance <= 0.5f)
        {
            AnimatorChange("isIDLE");
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, Vector3.zero, Time.deltaTime * m_Speed);
            AnimatorChange("isMOVE");
        }        
    }

    IEnumerator ReturnCoroutine(float timer, GameObject obj, string path)
    {
        yield return new WaitForSeconds(timer);
        BaseManager.Pool.m_pool_Dictionary[path].Return(obj);
    }
}
