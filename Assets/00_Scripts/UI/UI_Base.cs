using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Base : MonoBehaviour
{
    protected bool _Init = false;
    public virtual bool Init()
    {
        if(_Init) { return false; }
        
        _Init = true;
        return _Init;
    }

    private void Start()
    {
        Init();
    }

    public virtual void DisableOBJ()
    {
        Utils.UI_Holder.Pop();
        Destroy(this.gameObject);
    }
}
