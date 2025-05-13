using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "Object/Item")]
public class ItemScriptable : ScriptableObject
{
    public string ItemName;
    public string ItemDescription;
    public Rarity ItemRarity;
    public float ItemChance;
}
