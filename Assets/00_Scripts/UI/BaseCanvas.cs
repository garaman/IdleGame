using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseCanvas : MonoBehaviour
{
    public static BaseCanvas instance = null;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;

            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public Transform COIN;
    [SerializeField] private Transform LAYER;
    [SerializeField] private Button Hero_Button, Inventory_Button, SavingMode_Button, ADS_Buff_Button;

    [HideInInspector]public PopUp_Item popUp = null;
    public static bool isSave = false;

    private void Start()
    {
        Hero_Button.onClick.AddListener(() => Get_UI("@Heros", true));
        Inventory_Button.onClick.AddListener(() => Get_UI("@Inventory"));
        SavingMode_Button.onClick.AddListener(() => { 
            Get_UI("@SavingMode"); 
            isSave = true;
        });
        ADS_Buff_Button.onClick.AddListener(() => Get_UI("@ADS_Buffer"));
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Utils.UI_Holder.Count > 0)
            {
                Utils.ClosePopupUI();
            }
            else
            {
                Debug.Log("게임 종료 팝업 노출"); // 추후 작업예정.
            }
        }
    }

    public Transform HOLDER_LAYER(int value)
    {
        return LAYER.GetChild(value);
    }

    public void Get_UI(string tmp, bool Fade = false)
    {
        if (Fade)
        {
            MainUI.instance.FadeInOut(false, true, () => GetPopupUI(tmp));
            return;
        }
        GetPopupUI(tmp);
    }

    void GetPopupUI(string tmp)
    {
        var go = Instantiate(Resources.Load<BaseUI>("UI/" + tmp), transform);
        Utils.UI_Holder.Push(go);
    }

    public PopUp_Item PopUpItem()
    {        
        if(popUp != null) Destroy(popUp.gameObject);
        popUp = Instantiate(Resources.Load<PopUp_Item>("UI/PopUp_Item"), transform);
        return popUp;
    }
}