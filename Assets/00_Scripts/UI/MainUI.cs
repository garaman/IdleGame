using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class MainUI : MonoBehaviour
{
    public static MainUI instance = null;
    #region 필드
    [Header("Default")]
    [SerializeField] private TextMeshProUGUI m_Level_Text;
    [SerializeField] private TextMeshProUGUI m_FightScore_Text;
    [SerializeField] private TextMeshProUGUI m_Money_Text;
    [SerializeField] private TextMeshProUGUI m_LevelUpMoney_Text;
    [SerializeField] private TextMeshProUGUI m_StageCount_Text;
    [SerializeField] private TextMeshProUGUI m_StageType_Text;
    Color m_StageColor = new Color(0.0f, 0.7f, 1.0f, 1.0f);

    [Space(10.0f)]
    [Header("Fade")]
    [SerializeField] private Image m_Fade;
    [SerializeField] private float m_FadeDuration = 0.5f;

    [Space(10.0f)]
    [Header("MonsterSlider")]
    [SerializeField] private GameObject m_MonsterSlider_OBJ;
    [SerializeField] private Image m_MonsterSlider;
    [SerializeField] private TextMeshProUGUI m_MonsterSlider_Text;

    [Space(10.0f)]
    [Header("BossSlider")]
    [SerializeField] private GameObject m_BossSlider_OBJ;
    [SerializeField] private Image m_BossSlider_HP;
    [SerializeField] private TextMeshProUGUI m_BossSlider_Text;
    [SerializeField] private TextMeshProUGUI m_BossSlider_Name;

    [Space(10.0f)]
    [Header("BossEnter")]
    [SerializeField] private GameObject m_BossEnter_OBJ;

    [Space(10.0f)]
    [Header("LagendaryPopUp")]
    [SerializeField] private Animator m_LagendaryPopUp;
    [SerializeField] private Image m_PopUp_ItemFrame;
    [SerializeField] private Image m_PopUp_ItemImage;
    [SerializeField] private TextMeshProUGUI m_PopUp_Text;
    bool isPopup;
    Coroutine LagendaryPopup_Coroutine;

    [Space(10.0f)]
    [Header("ItemPopup")]
    [SerializeField] private Transform m_ItemContent;
    private List<TextMeshProUGUI> m_ItemTexts = new List<TextMeshProUGUI>();
    private List<Coroutine> m_ItemCoroutines = new List<Coroutine>();

    [Space(10.0f)]
    [Header("HeroFrame")]
    [SerializeField] private UI_Main_Part[] main_Parts;
    public Image main_HeroSkillFill;
    Dictionary<Player, UI_Main_Part> m_Parts = new Dictionary<Player, UI_Main_Part>();

    [Space(10.0f)]
    [Header("ADS")]
    [SerializeField] private Image FastLock;
    [SerializeField] private GameObject FastFrame;
    [SerializeField] private GameObject[] BuffLock;
    [SerializeField] private Image x2_Fill;
    [SerializeField] private TextMeshProUGUI x2_Text;

    [Space(10.0f)]
    [Header("LayerButtons")]
    [SerializeField] private Transform[] Layer_Images;
    #endregion
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;

            //DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        TextCheck();
        SliderOBJCheck(false);

        BaseManager.isFast = PlayerPrefs.GetInt("FAST") == 1 ? true : false;

        TimeCheck();
        BuffCheck();

        for (int i = 0; i < m_ItemContent.childCount; i++)
        {
            m_ItemTexts.Add(m_ItemContent.GetChild(i).GetComponent<TextMeshProUGUI>());
            m_ItemCoroutines.Add(null);
        }

        StageManager.m_ReadyEvent += () => OnReady();
        StageManager.m_BossEvent += () => OnBoss();
        StageManager.m_ClearEvent += () => OnClear();
        StageManager.m_DeadEvent += () => OnDead();

        StageManager.State_Change(Stage_State.Ready);
    }

    private void Update()
    {
        if(DataManager.gameData.Buffe_x2 > 0.0f)
        {
            x2_Fill.fillAmount =DataManager.gameData.Buffe_x2 / 1800.0f;            
            x2_Text.text = Utils.GetTimer(DataManager.gameData.Buffe_x2);
        }
        else
        {
            x2_Fill.fillAmount = 0.0f;
            x2_Text.text = "00:00";
        }        
    }

    public void LayerCheck(int value)
    {
        if(value != -1)
        {
            StartCoroutine(ImageMoveCoroutine(value));
        }        

        for (int i = 0; i < Layer_Images.Length; i++)
        {
            if(value != i && Layer_Images[i].transform.localScale.x >= 1.3f)
            {
                StartCoroutine(ImageMoveCoroutine(i, true));
            }
        }
    }

    IEnumerator ImageMoveCoroutine(int value, bool ScaleDown = false)
    {
        float current = 0.0f;
        float percent = 0.0f;
        float start = ScaleDown ? 80.0f : 45.0f;
        float end = ScaleDown ? 45.0f : 80.0f;
        float startScale = ScaleDown ? 1.5f : 1.0f;
        float endScale = ScaleDown ? 1.0f : 1.5f;

        while (percent < 1.0f)
        {
            current += Time.unscaledDeltaTime;
            percent = current / 0.2f;
            float LerpYPos = Mathf.Lerp(start, end, percent);
            float lerpScale = Mathf.Lerp(startScale, endScale, percent);
            Layer_Images[value].localPosition = new Vector2(Layer_Images[value].localPosition.x, LerpYPos);
            Layer_Images[value].localScale = new Vector3(lerpScale, lerpScale, 1.0f);
            yield return null;
        }
    }
    public void BuffCheck()
    {
        for (int i = 0; i <DataManager.gameData.Buffer_Timer.Length; i++)
        {
            BuffLock[i].SetActive(DataManager.gameData.Buffer_Timer[i] > 0.0f ? false : true);
        }
        if(DataManager.gameData.Buffe_x2 > 0.0f)
        {
            x2_Fill.transform.parent.gameObject.SetActive(true);
        }
        else
        {
            x2_Fill.transform.parent.gameObject.SetActive(false);
        }
    }

    private void TimeCheck()
    {
        Time.timeScale = BaseManager.isFast ? 1.5f : 1.0f;
        FastLock.gameObject.SetActive(!BaseManager.isFast);
        FastFrame.SetActive(BaseManager.isFast);
    }

    public void GetFast()
    {
        bool Fast = !BaseManager.isFast;
        if(Fast == true)
        {
           if(DataManager.gameData.Buffe_x2 <= 0.0f)
            {
                BaseManager.ADS.ShowRewardedAd(() => 
                {                    
                   DataManager.gameData.Buffe_x2 = 1800.0f;
                    
                    BuffCheck();
                    TimeCheck();
                });                

            }           
        }

        BaseManager.isFast = Fast;
        PlayerPrefs.SetInt("FAST", Fast == true ? 1 : 0);        
    }

    public void Set_BossState()
    {
        StageManager.isDead = false;
        StageManager.State_Change(Stage_State.Boss);
        m_BossEnter_OBJ.SetActive(false);
    }

    private void SliderOBJCheck(bool isBoss)
    {
        if (StageManager.isDead == true)
        {
            m_BossSlider_OBJ.SetActive(false);
            m_MonsterSlider_OBJ.SetActive(false);

            m_BossEnter_OBJ.SetActive(true);            
            return;
        }

        m_BossSlider_OBJ.SetActive(isBoss);
        m_MonsterSlider_OBJ.SetActive(!isBoss);

        MonsterSlider();

        float value = isBoss ? 1.0f : 0.0f;
        BossSlider(value, 1.0f);
    }


    private void OnReady()
    {
        FadeInOut(true, false);
        SliderOBJCheck(false);

        m_Parts.Clear();
        for(int i = 0; i < main_Parts.Length; i++)
        {
            main_Parts[i].Initalize();
        }

        int indexValue = 0;
        for (int i = 0; i < BaseManager.Data.SetHeroData.Length; i++)
        {
            var data = BaseManager.Data.SetHeroData[i];
            if(data != null)
            {
                indexValue++;
                main_Parts[i].InitData(data, false);
                main_Parts[i].transform.SetSiblingIndex(indexValue);
                m_Parts.Add(HeroSpawner.players[i], main_Parts[i]);
            }
        }
    }

    public void SetHeroData()
    {
        int indexValue = 0;
        for (int i = 0; i < BaseManager.Data.SetHeroData.Length; i++)
        {
            var data = BaseManager.Data.SetHeroData[i];
            if (data != null)
            {
                indexValue++;
                main_Parts[i].InitData(data, true);
                main_Parts[i].transform.SetSiblingIndex(indexValue);
            }
        }
    }

    public void HeroStateCheck(Player player)
    {
        m_Parts[player].StateCheck(player);
    }
    private void OnBoss()
    {
        SliderOBJCheck(true);        
    }

    private void OnClear()
    {
        SliderOBJCheck(false);
        StartCoroutine(Clear_Daley());
    }

    private void OnDead()
    {        
        StartCoroutine(Dead_Daley());
    }


    IEnumerator Clear_Daley()
    {
        yield return new WaitForSeconds(2.0f);
        FadeInOut(false);

        yield return new WaitForSeconds(1.0f);
        StageManager.State_Change(Stage_State.Ready);
    }

    IEnumerator Dead_Daley()
    {
        yield return StartCoroutine(Clear_Daley());
        SliderOBJCheck(false);

        for(int i = 0; i < Spawner.m_Monsters.Count; i++)
        {
            BaseManager.Pool.m_pool_Dictionary["Monster"].Return(Spawner.m_Monsters[i].gameObject);         
        }
        Spawner.m_Monsters.Clear();
        TextCheck();
    }

    public void TextCheck()
    {
        m_Level_Text.text = "Lv."+(DataManager.gameData.Level +1).ToString();
        m_FightScore_Text.text = StringMethod.ToCurrencyString(BaseManager.Player.Get_FightScore());

        double levelUpMoneyValue = Utils.DesignData.levelData.MONEY();
        m_LevelUpMoney_Text.text = StringMethod.ToCurrencyString(levelUpMoneyValue) +"G";
        m_LevelUpMoney_Text.color = Utils.Coin_Check(levelUpMoneyValue) ? Color.green : Color.red;

        m_Money_Text.text = StringMethod.ToCurrencyString(DataManager.gameData.Money) + "G";
        
        m_StageType_Text.text = StageManager.isDead ? "반복중.." : "진행중..";
        m_StageType_Text.color = StageManager.isDead ? Color.yellow : m_StageColor;

        int stageValue =DataManager.gameData.Stage + 1;
        int stageForward = (stageValue / 10)+1;
        int stageBack = stageValue % 10;

        m_StageCount_Text.text = "Stage " + stageForward + "-" +stageBack;
    }
    

    public void FadeInOut(bool FadeInOut,bool Sibling = false, Action action = null)
    {
        if(Sibling == false)
        {
            m_Fade.transform.SetParent(this.transform);
            m_Fade.transform.SetSiblingIndex(0);
        }
        else
        {
            m_Fade.transform.SetParent(BaseCanvas.instance.transform);
            m_Fade.transform.SetAsLastSibling();
        }
        
        StartCoroutine(FadeInOout_Coroutine(FadeInOut, action));
    }

    IEnumerator FadeInOout_Coroutine(bool FadeInOut, Action action = null)
    {
        if (FadeInOut == false)
        {
            m_Fade.raycastTarget = true;
        }

        float current = 0.0f;
        float percent = 0.0f;
        float start = FadeInOut ? 1.0f : 0.0f;
        float end = FadeInOut ? 0.0f : 1.0f;

        while (percent < 1.0f)
        {
            current += Time.deltaTime;
            percent = current / m_FadeDuration;
            float LerpPos = Mathf.Lerp(start, end, percent);
            m_Fade.color = new Color(0, 0, 0, LerpPos);
            yield return null;
        }

        if (action != null)
        {
            action?.Invoke();
        }

        m_Fade.raycastTarget = false;
    }

    public void MonsterSlider()
    {
        float value = (float)StageManager.count / (float)StageManager.maxCount;
        if(value >= 1.0f) 
        { 
            value = 1.0f; 
            if(StageManager.m_State != Stage_State.Boss)
            { 
                StageManager.State_Change(Stage_State.Boss); 
            }            
        }        
        m_MonsterSlider.fillAmount = value;
        m_MonsterSlider_Text.text = string.Format("{0:0.0}", value * 100.0f) + "%";
    }

    public void BossSlider(double hp, double maxHp)
    {
        float value = (float)hp / (float)maxHp;
        if (value <= 0.0f)
        {
            value = 0.0f;            
        }
        m_BossSlider_HP.fillAmount = value;
        m_BossSlider_Text.text = string.Format("{0:0.0}", value * 100.0f) + "%";
    }

    public void GetLegendaryPopUp(Item_Scriptable item)
    {
        if (isPopup)
        {
            m_LagendaryPopUp.gameObject.SetActive(false);
        }

        if (LagendaryPopup_Coroutine != null)
        {
            StopCoroutine(LagendaryPopup_Coroutine);
        }
        
        isPopup = true;
        m_LagendaryPopUp.gameObject.SetActive(true);
        m_PopUp_ItemImage.sprite = Utils.Get_Atlas(item.name);
        m_PopUp_ItemFrame.sprite = Utils.Get_Atlas(item.ItemRarity.ToString());
        m_PopUp_Text.text = Utils.String_Color_Rarity(item.ItemRarity) + item.ItemName + "</color>을(를) 획득하였습니다.";
        
        LagendaryPopup_Coroutine = StartCoroutine(LagendaryPopUp_Coroutine());
    }

    IEnumerator LagendaryPopUp_Coroutine()
    {
        yield return new WaitForSecondsRealtime(2.0f);
        isPopup = false;
        m_LagendaryPopUp.SetTrigger("isCLOSE");
    }

    public void GetItem(Item_Scriptable item)
    {
        bool AllActive = true;
        for (int i = 0; i < m_ItemTexts.Count; i++)
        {
            if (m_ItemTexts[i].gameObject.activeSelf == false)
            {
                m_ItemTexts[i].gameObject.SetActive(true);
                m_ItemTexts[i].text = "아이템을 획특하였습니다. " + Utils.String_Color_Rarity(item.ItemRarity) + "[" + item.ItemName + "]</color>";

                for (int j = 0; j < i; j++)
                {
                    RectTransform rect = m_ItemTexts[j].GetComponent<RectTransform>();
                    rect.anchoredPosition = new Vector2(rect.anchoredPosition.x, rect.anchoredPosition.y + 30.0f);
                }
                if (m_ItemCoroutines[i] != null)
                {
                    StopCoroutine(m_ItemCoroutines[i]);
                    m_ItemCoroutines[i] = null;
                }
                m_ItemCoroutines[i] = StartCoroutine(ItemText_Coroutine(m_ItemTexts[i].GetComponent<RectTransform>()));
                AllActive = false;
                break;
            }
        }

        if (AllActive == true)
        {
            GameObject baseRect = null;
            float yCount = 0.0f;
            for (int i = 0; i < m_ItemTexts.Count; i++)
            {
                RectTransform rect = m_ItemTexts[i].GetComponent<RectTransform>();
                if (rect.anchoredPosition.y > yCount)
                {
                    baseRect = rect.gameObject;
                    yCount = rect.anchoredPosition.y;
                }
            }

            for (int i = 0; i < m_ItemTexts.Count; i++)
            {
                if (baseRect == m_ItemTexts[i].gameObject)
                {
                    m_ItemTexts[i].gameObject.SetActive(false);
                    m_ItemTexts[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(0.0f, 0.0f);

                    m_ItemTexts[i].gameObject.SetActive(true);
                    m_ItemTexts[i].text = "아이템을 획특하였습니다. " + Utils.String_Color_Rarity(item.ItemRarity) + "[" + item.ItemName + "]</color>";

                    if (m_ItemCoroutines[i] != null)
                    {
                        StopCoroutine(m_ItemCoroutines[i]);
                        m_ItemCoroutines[i] = null;
                    }
                    m_ItemCoroutines[i] = StartCoroutine(ItemText_Coroutine(m_ItemTexts[i].GetComponent<RectTransform>()));
                }
                else
                {
                    RectTransform rect = m_ItemTexts[i].GetComponent<RectTransform>();
                    rect.anchoredPosition = new Vector2(rect.anchoredPosition.x, rect.anchoredPosition.y + 30.0f);
                }
            }
        }

        if ((int)item.ItemRarity >= (int)Rarity.Hero)
        {
            GetLegendaryPopUp(item);
        }
    }

    IEnumerator ItemText_Coroutine(RectTransform rect)
    {
        yield return new WaitForSeconds(2.0f);
        rect.gameObject.SetActive(false);
        rect.anchoredPosition = new Vector2(0.0f, 0.0f);
    }
}
