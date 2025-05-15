using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderManager : MonoBehaviour
{
    public static RenderManager instance;

    private void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public Camera cam;
    public RenderHero Hero;

    public Vector2 ReturnScreenPoint(Transform pos)
    {
        return cam.WorldToScreenPoint(pos.position);
    }
}
