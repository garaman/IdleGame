using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_ToastPopUp : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI ToastPopUpText;

    public void Initalize(string temp)
    {
        ToastPopUpText.text = temp;

        Destroy(this.gameObject,1.5f);
    }
    
}
