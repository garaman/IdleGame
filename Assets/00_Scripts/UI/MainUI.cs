using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MainUI : MonoBehaviour
{
    public static MainUI instance = null;

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

    [SerializeField] private TextMeshProUGUI m_Level_Text;
    [SerializeField] private TextMeshProUGUI m_FightScore_Text;

    private void Start()
    {
        TextCheck();
    }

    public void TextCheck()
    {
        m_Level_Text.text = "Lv."+(BaseManager.Player.Level +1).ToString();
        m_FightScore_Text.text = StringMethod.ToCurrencyString(BaseManager.Player.Get_FightScore());
    }
}
