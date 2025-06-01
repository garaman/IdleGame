using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderHero : MonoBehaviour
{
    public Transform[] circles;
    public Transform pivot;
    public GameObject[] particles;    
    private List<GameObject> HeroOBJ = new List<GameObject>();

    public void GetParticle(bool m_B)
    {
        for(int i = 0; i < particles.Length; i++)
        {
            particles[i].SetActive(m_B);
        }
    }

    public void InitHero()
    {
        for (int i = 0; HeroOBJ.Count > i; i++)
        {            
            Destroy(HeroOBJ[i]);
        }
        HeroOBJ.Clear();

        for (int i = 0; i < BaseManager.Data.SetHeroData.Length; i++)
        {
            if (BaseManager.Data.SetHeroData[i] != null)
            {                
                string temp = BaseManager.Data.SetHeroData[i].m_Character_Name;
                var go = Instantiate(Resources.Load<GameObject>("Hero/" + temp));
                HeroOBJ.Add(go);

                ChageLayer(go, "RenderLayer");

                go.transform.SetParent(transform);
                go.GetComponent<Player>().enabled = false;
                go.transform.position = circles[i].position;
                go.transform.LookAt(pivot.position);

            }
        }
    }

    void ChageLayer(GameObject go, string layerName)
    {
        if(go.transform.childCount == 0) { return; }
        for (int i = 0; i < go.transform.childCount; i++)
        {
            go.transform.GetChild(i).gameObject.layer = LayerMask.NameToLayer(layerName);
            ChageLayer(go.transform.GetChild(i).gameObject, layerName);
        }
    }
}
