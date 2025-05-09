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
    [SerializeField] private Button Hero_Button;

    private void Start()
    {
        Hero_Button.onClick.AddListener(() => Get_UI("@Heros",true));
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
            MainUI.instance.FadeInOut(false,true,() => GetPopupUI(tmp));
            return;
        }
        GetPopupUI(tmp);
    }

    void GetPopupUI(string tmp)
    {
        var go = Instantiate(Resources.Load<BaseUI>("UI/" + tmp), transform);
        Utils.UI_Holder.Push(go);
    }
}
