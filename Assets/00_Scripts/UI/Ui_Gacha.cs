using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Ui_Gacha : BaseUI
{
    public Transform content;
    public Image HeroFrame;

    [SerializeField] private Button gachaButton;
    [SerializeField] private TextMeshProUGUI gachaText;
    [SerializeField] private TextMeshProUGUI gachaPrice;
    [SerializeField] private GameObject[] gachaLock;
    public override bool Init()
    {
        GachaInit();        
        return base.Init();
    }

    public void OnClickGacha(int value)
    {
        GachaInit();
        GetGachaHero(value);
    }

    private void GachaInit()
    {
        for(int i = 0; i < content.childCount; i++)
        {
            Destroy(content.GetChild(i).gameObject);
        }
        ActiveButtonLock(false); // 잠금 해제.
    }

    public void GetGachaHero(int value)
    {
        gachaButton.onClick.RemoveAllListeners(); // 기존 리스너 제거 , 중복 클릭 방지.
        switch (value)
        {
            case 1:
                gachaText.text = "1회 뽑기";
                gachaPrice.text = "300";
                gachaButton.onClick.AddListener(()=>OnClickGacha(value));
                break;
            case 11:
                gachaText.text = "11회 뽑기";
                gachaPrice.text = "3000";
                gachaButton.onClick.AddListener(() => OnClickGacha(value));
                break;
        }
        ActiveButtonLock(true); // 잠금 활성화.
        StartCoroutine(Gacha_Coroutine(value));
    }


    IEnumerator Gacha_Coroutine(int value)
    {
        yield return new WaitForSeconds(0.2f);
        for (int i = 0; i < value; i++)
        {            
            Rarity rarity = Rarity.Common;
            float R_Percentage = 0.0f;
            float percentage = Random.Range(0.0f, 100.0f);

            var go = Instantiate(HeroFrame, content);
            go.gameObject.SetActive(true);
            
            for (int j = 0; j < Utils.GachaPercentage().Length; j++)
            {
                R_Percentage += Utils.GachaPercentage()[j];
                if (percentage <= R_Percentage)
                {
                    rarity = (Rarity)j;
                    break;
                }
            }

            // 뽑기 완료된 영웅 정보 결정.
            Hero_Scriptable hero = BaseManager.Data.Get_Rarity_Hero(rarity);

            BaseManager.Data.HeroInfos[hero.name].Count++; // count 증가.
            DataManager.gameData.HeroSummon_Count++; // 소환 횟수 증가.

            go.sprite = Utils.Get_Atlas(rarity.ToString());
            go.transform.GetChild(0).GetComponent<Image>().sprite = Utils.Get_Atlas(hero.name);
                        
            yield return new WaitForSeconds(0.2f);
        }

        BaseManager.Firebase.WriteData(); // 파이어베이스에 데이터 저장.
        ActiveButtonLock(false); // 잠금 해제.
    }

    private void ActiveButtonLock(bool isActive)
    {
        for (int i = 0; i < gachaLock.Length; i++)
        {
            gachaLock[i].SetActive(isActive);
        }
    }

}
