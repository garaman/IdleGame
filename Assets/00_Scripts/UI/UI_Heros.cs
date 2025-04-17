using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UI_Heros : UI_Base
{
    public Transform Content;
    public GameObject Card;

    Dictionary<string, Character_Scriptable> m_CharacterDic = new Dictionary<string, Character_Scriptable>();

    public override bool Init()
    {
        var Data = Resources.LoadAll<Character_Scriptable>("Scriptable");
        for (int i = 0; i < Data.Length; i++)
        {
            m_CharacterDic.Add(Data[i].m_Character_Name, Data[i]);
        }

        var sort_Dictionary = m_CharacterDic.OrderBy(x => x.Value.m_Rarity);

        foreach (var data in sort_Dictionary)
        {
            var go = Instantiate(Card, Content).GetComponent<UI_Heros_Card>();
            go.Initalize(data.Value);
        }

        return base.Init();
    }

}
