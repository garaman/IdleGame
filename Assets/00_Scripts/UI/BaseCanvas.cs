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
    [SerializeField] private Transform BACKLAYER;
    
    [Space(10.0f)]
    [Header("Bottom")]
    [SerializeField] private Button Hero_Button;
    [SerializeField] private Button Shop_Button;

    [Space(10.0f)]
    [Header("Right")]
    [SerializeField] private Button Inventory_Button;
    [SerializeField] private Button SavingMode_Button;
    [SerializeField] private Button ADS_Buff_Button;

    [HideInInspector]public PopUp_Item popUp = null;
    public static bool isSave = false;

    private void Start()
    {
        // Bottom UI
        Hero_Button.onClick.AddListener(() => Get_UI("@Heros", true));
        Shop_Button.onClick.AddListener(() => Get_UI("@Shop", false, true));
        Shop_Button.onClick.AddListener(() => MainUI.instance.LayerCheck(5));

        // Right UI
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
                Get_UI("@BackButton");                
            }
        }
    }

    public Transform HOLDER_LAYER(int value)
    {
        return LAYER.GetChild(value);
    }

    public void Get_UI(string tmp, bool Fade = false, bool Back = false)
    {
        Utils.CloseAllPopupUI();

        if (Fade)
        {
            MainUI.instance.FadeInOut(false, true, () => GetPopupUI(tmp));
            return;
        }
        GetPopupUI(tmp, Back);
    }

    void GetPopupUI(string tmp, bool Back = false)
    {
        var go = Instantiate(Resources.Load<BaseUI>("UI/" + tmp), Back ? BACKLAYER : transform);
        Utils.UI_Holder.Push(go);
    }

    public PopUp_Item PopUpItem()
    {        
        if(popUp != null) Destroy(popUp.gameObject);
        popUp = Instantiate(Resources.Load<PopUp_Item>("UI/PopUp_Item"), transform);
        return popUp;
    }

    public UI_ToastPopUp Get_Toast()
    {
        return Instantiate(Resources.Load<UI_ToastPopUp>("UI/@ToastPopup"),transform);
    }
}