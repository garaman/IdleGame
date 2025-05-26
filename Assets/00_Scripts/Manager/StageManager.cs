using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public delegate void OnReadyEvent();
public delegate void OnPlayEvent();
public delegate void OnBossEvent();
public delegate void OnBossPlayEvent();
public delegate void OnClearEvent();
public delegate void OnDeadEvent();

public class StageManager 
{
    // ���� ���� { State Pattern }
    public static Stage_State m_State;

    public static int maxCount;
    public static int count = 0;    

    public static bool isDead = false;

    public static OnReadyEvent m_ReadyEvent;
    public static OnPlayEvent m_PlayEvent;
    public static OnBossEvent m_BossEvent;
    public static OnBossPlayEvent m_BossPlayEvent;
    public static OnClearEvent m_ClearEvent;
    public static OnDeadEvent m_DeadEvent;
        
    public static void State_Change(Stage_State state)
    {
        m_State = state;
        switch (state)
        {
            case Stage_State.Ready:
                maxCount = int.Parse(CSV_Importer.SpawnDesign[DataManager.gameData.Stage]["MaxCount"].ToString());
                count = 0;
                m_ReadyEvent?.Invoke();
                BaseManager.instance.ActionCoroutine_Start(() => State_Change(Stage_State.Play), 2.0f);
                break;
            case Stage_State.Play: 
                m_PlayEvent?.Invoke();
                break;
            case Stage_State.Boss:
                count = 0;
                m_BossEvent?.Invoke();
                break;
            case Stage_State.BossPlay:
                m_BossPlayEvent?.Invoke();
                break;
            case Stage_State.Clear:
                count = 0;
               DataManager.gameData.Stage++;
                m_ClearEvent?.Invoke();
                break;
            case Stage_State.Dead:
                isDead = true;
                m_DeadEvent?.Invoke();                
                break;
        }
    }
}
