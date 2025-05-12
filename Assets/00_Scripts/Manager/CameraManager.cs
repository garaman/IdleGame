using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance = null;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;    
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    [Header("Default")]    
    [Range(0f, 10.0f)]
    [SerializeField] private float Distance_Value;
    
    [Space(20f)]

    [Header("Camera Shake")]
    [Range(0f, 10.0f)]
    [SerializeField] private float duration;
    [Range(0f, 10.0f)]
    [SerializeField] private float power;

    float m_Distance = 4.0f;
    Vector3 OriginPos;
    bool isCameraShake = false;
    Camera cam;

    private void Start()
    {
        cam = GetComponent<Camera>();
        OriginPos = cam.transform.localPosition;
    }

    private void Update()
    {

        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, Distance(), Time.deltaTime * 2.0f);
    }


    float Distance()
    {
        var players = Spawner.m_Players.ToArray();
        float maxDistance = m_Distance;

        foreach (var player in players)
        {
            float targetDistance = Vector3.Distance(Vector3.zero, player.transform.position) + Distance_Value;
            //Debug.Log(targetDistance);

            if(targetDistance > maxDistance)
            {
                maxDistance = targetDistance;
            }
        }
        return maxDistance;
    }

    public void CameraShake()
    {
        if(isCameraShake) { return; }
        isCameraShake = true;
        StartCoroutine(CameraShake_Coroutine());
    }

    IEnumerator CameraShake_Coroutine()
    {
        float timer = 0.0f;

        while(timer <= duration)
        {
            transform.localPosition = OriginPos + Random.insideUnitSphere * power;

            timer += Time.deltaTime;
            yield return null;
        }
        isCameraShake = false;
        transform.localPosition = OriginPos;
    }
}
