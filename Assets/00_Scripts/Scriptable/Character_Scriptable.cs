using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Scriptable", menuName ="Object/Character", order = int.MinValue)]
public class Character_Scriptable : ScriptableObject
{
    [Header("Info")]
    public string m_Character_Name;
    public float m_Attack_Range;
    public Rarity m_Rarity;

}
