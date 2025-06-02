using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


#region Stage
public delegate void OnReadyEvent();
public delegate void OnPlayEvent();
public delegate void OnBossEvent();
public delegate void OnBossPlayEvent();
public delegate void OnClearEvent();
public delegate void OnDeadEvent();
#endregion

#region Relic
public delegate void MonsterDead(Monster monster);
public delegate void PlayerAttack(Player player, Monster monster);
public delegate void PlayerDameged(Player player);
#endregion
public class Delegate_Holder 
{
    public static event MonsterDead OnMonsterDead;
    public static event PlayerAttack OnPlayerAttack;
    public static event PlayerDameged OnPlayerDameged;

    public static void ClearEvents()
    {
        OnMonsterDead = null;
        OnPlayerAttack = null;
        OnPlayerDameged = null;
    }

    public static void MonsterDead(Monster monster)
    {
        OnMonsterDead?.Invoke(monster);
    }

    public static void PlayerAttack(Player player, Monster monster)
    {
        OnPlayerAttack?.Invoke(player, monster);
    }

    public static void PlayerDameged(Player player)
    {
        OnPlayerDameged?.Invoke(player);
    }
}
