using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor : MonoBehaviour
{
    [Range(0.0f, 100.0f)]
    public float speed;

    public ParticleSystem Explosion_Particle;
    public Transform MeteorOBJ;
    public Transform Circle;

    Transform parentTransfrom;
    public void Init(double damage)
    {
        if(parentTransfrom == null)
        {
            parentTransfrom = transform.parent;
        }
        transform.SetParent(null);
        StartCoroutine(Meteor_Coroutine(damage));
    }

    IEnumerator Meteor_Coroutine(double damege)
    {
        MeteorOBJ.localPosition = new Vector3(Random.Range(-10.0f, 10.0f), Random.Range(10.0f, 15.0f), Random.Range(5.0f, 10.0f));
        MeteorOBJ.gameObject.SetActive(true);
        MeteorOBJ.LookAt(transform.parent);

        //Circle.localPosition = new Vector3(0.5f,0.5f,0.5f);
        SpriteRenderer renderer = Circle.GetComponent<SpriteRenderer>();

        while(true)
        {
            float distance = Vector3.Distance(MeteorOBJ.localPosition, Vector3.zero);

            if (distance >= 0.1f)
            {
                MeteorOBJ.localPosition = Vector3.MoveTowards(MeteorOBJ.localPosition, Vector3.zero, speed * Time.deltaTime);
                float ScaleValue = distance/speed * 0.2f;
                renderer.color = new Color(0, 0, 0, Mathf.Min((distance/speed), 0.5f));
                Circle.localScale = new Vector3(ScaleValue, ScaleValue, ScaleValue);
                yield return null;                
            }
            else
            {
                Explosion_Particle.Play();
                CameraManager.instance.CameraShake();
                for(int i = 0; i < Spawner.m_Monsters.Count; i++)
                {
                    if(Vector3.Distance(transform.position, Spawner.m_Monsters[i].transform.position) <= 1.5f)
                    {
                        Spawner.m_Monsters[i].GetDamage(damege, true);
                    }
                }
                break;
            }
        }
        yield return new WaitForSeconds(0.5f);        
        transform.SetParent(parentTransfrom);
        MeteorOBJ.gameObject.SetActive(false);
        this.gameObject.SetActive(false);        
    }
}
