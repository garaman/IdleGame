using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using static UnityEditor.Progress;

public class Item_OBJ : MonoBehaviour
{
    [SerializeField] private Transform ItemTextRect;
    [SerializeField] private TextMeshProUGUI m_Text;
    [SerializeField] private GameObject[] Raritys;
    [SerializeField] private ParticleSystem m_Loot;
    [SerializeField] private float firingAngle = 45.0f;
    [SerializeField] private float gravity = 9.8f;

    Rarity rarity;
    bool isCheck = false;

    ItemScriptable m_item;
    void RarityCheck()
    {
        isCheck = true;
        transform.position = new Vector3(transform.position.x, 0f, transform.position.z);
        transform.rotation = Quaternion.identity;       

        Raritys[(int)rarity].SetActive(true);
        ItemTextRect.gameObject.SetActive(true);
        ItemTextRect.SetParent(BaseCanvas.instance.HOLDER_LAYER(2));
        
        m_Text.text = Utils.String_Color_Rarity(rarity) + m_item.ItemName + "</color>";

        StartCoroutine(LootItem());
    }

    IEnumerator LootItem()
    {
        yield return new WaitForSeconds(Random.Range(1.0f, 1.5f));

        for(int i = 0;  i < Raritys.Length; i++)
        {
            Raritys[i].SetActive(false);
        }

        ItemTextRect.transform.SetParent(this.transform);
        ItemTextRect.gameObject.SetActive(false);

        m_Loot.Play();

        MainUI.instance.GetItem(m_item);        

        yield return new WaitForSeconds(0.5f);

        BaseManager.Pool.m_pool_Dictionary["Item_OBJ"].Return(this.gameObject);
    }

    private void Update()
    {
        if(isCheck == false) { return; }
        
        ItemTextRect.position = Camera.main.WorldToScreenPoint(transform.position);

    }

    public void Init(Vector3 pos, ItemScriptable data)    
    {
        m_item = data;
        rarity = m_item.ItemRarity;
        
        isCheck = false;
        ItemTextRect.gameObject.SetActive(false);

        transform.position = pos;
        Vector3 targetPos = new Vector3(pos.x +(Random.insideUnitSphere.x*2.0f), 0.5f, pos.z + (Random.insideUnitSphere.z * 2.0f));
        StartCoroutine(SimulaterProjectile(targetPos));
    }

    IEnumerator SimulaterProjectile(Vector3 pos)
    {
        float target_Distance = Vector3.Distance(transform.position, pos);

        float projectile_Velocity = target_Distance / (Mathf.Sin(2*firingAngle*Mathf.Deg2Rad / gravity));

        float Vx = Mathf.Sqrt(projectile_Velocity) * Mathf.Cos(firingAngle * Mathf.Deg2Rad);
        float Vy = Mathf.Sqrt(projectile_Velocity) * Mathf.Sin(firingAngle * Mathf.Deg2Rad);
        float flightDuration = target_Distance / Vx;

        transform.rotation = Quaternion.LookRotation(pos - transform.position);

        float time = 0.0f;
        while(time < flightDuration)
        {
            transform.Translate(0, (Vy - (gravity * time)) * Time.deltaTime, Vx * Time.deltaTime);
            time += Time.deltaTime;            
            yield return null;
        }

        RarityCheck();        
    }
}
