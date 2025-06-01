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
        ActiveButtonLock(false); // ��� ����.
    }

    public void GetGachaHero(int value)
    {
        gachaButton.onClick.RemoveAllListeners(); // ���� ������ ���� , �ߺ� Ŭ�� ����.
        switch (value)
        {
            case 1:
                gachaText.text = "1ȸ �̱�";
                gachaPrice.text = "300";
                gachaButton.onClick.AddListener(()=>OnClickGacha(value));
                break;
            case 11:
                gachaText.text = "11ȸ �̱�";
                gachaPrice.text = "3000";
                gachaButton.onClick.AddListener(() => OnClickGacha(value));
                break;
        }
        ActiveButtonLock(true); // ��� Ȱ��ȭ.
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

            // �̱� �Ϸ�� ���� ���� ����.
            Hero_Scriptable hero = BaseManager.Data.Get_Rarity_Hero(rarity);

            BaseManager.Data.HeroInfos[hero.name].Count++; // count ����.
            DataManager.gameData.HeroSummon_Count++; // ��ȯ Ƚ�� ����.

            go.sprite = Utils.Get_Atlas(rarity.ToString());
            go.transform.GetChild(0).GetComponent<Image>().sprite = Utils.Get_Atlas(hero.name);
                        
            yield return new WaitForSeconds(0.2f);
        }

        BaseManager.Firebase.WriteData(); // ���̾�̽��� ������ ����.
        ActiveButtonLock(false); // ��� ����.
    }

    private void ActiveButtonLock(bool isActive)
    {
        for (int i = 0; i < gachaLock.Length; i++)
        {
            gachaLock[i].SetActive(isActive);
        }
    }

}
