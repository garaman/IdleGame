using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "Object/Item")]
public class Item_Scriptable : ScriptableObject
{
    public string ItemName;
    public string ItemDescription;
    public Rarity ItemRarity;
    public ItemType ItemType;
    public float ItemChance;
}
