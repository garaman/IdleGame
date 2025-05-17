using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Scriptable", menuName ="Object/Character", order = int.MinValue)]
public class Hero_Scriptable : ScriptableObject
{
    [Header("Info")]
    public string m_Character_Name;
    public float m_Attack_Range;
    public float m_Attack_Speed; 
    public Rarity m_Rarity;
    public int Max_Mp;

}
