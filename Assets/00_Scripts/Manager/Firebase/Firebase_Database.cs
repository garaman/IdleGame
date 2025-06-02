using Firebase.Database;
using Firebase.Extensions;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public partial class FirebaseManager
{
    
    public void WriteData()
    {
        GameData data = new GameData(); // GameData ��ü ����
        if(DataManager.gameData != null) // DataManager�� GameData�� ������
        { 
            data = DataManager.gameData;
            data.endDate = DateTime.Now.ToString();
        }

        #region GameData
        string GameDataJson = JsonUtility.ToJson(DataManager.gameData);
        reference.Child("USER").Child(currentUser.UserId).Child("DATA").SetRawJsonValueAsync(GameDataJson).ContinueWithOnMainThread(task =>
        {
            if (!task.IsCompleted)
            {
                Debug.LogError("������ ���� ����: " + task.Exception.ToString());
            }
        });
        #endregion

        #region HeroInfo
        string HeroDataJson = JsonConvert.SerializeObject(BaseManager.Data.HeroInfos); // Infos�� JSON ���ڿ��� ��ȯ
        reference.Child("USER").Child(currentUser.UserId).Child("HERO").SetRawJsonValueAsync(HeroDataJson).ContinueWithOnMainThread(task =>
        {
            if (!task.IsCompleted)
            {
                Debug.LogError("������ ���� ����: " + task.Exception.ToString());
            }
        });
        #endregion

        #region ItemInfo
        string ItemDataJson = JsonConvert.SerializeObject(BaseManager.Data.ItemInfos); // Infos�� JSON ���ڿ��� ��ȯ
        reference.Child("USER").Child(currentUser.UserId).Child("ITEM").SetRawJsonValueAsync(ItemDataJson).ContinueWithOnMainThread(task =>
        {
            if (!task.IsCompleted)
            {
                Debug.LogError("������ ���� ����: " + task.Exception.ToString());
            }
        });
        #endregion
    }

    public void ReadData()
    {
        #region GameData
        reference.Child("USER").Child(currentUser.UserId).Child("DATA").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {                
                DataSnapshot snapshot = task.Result;
                if (snapshot.Exists)
                {
                    var data = JsonUtility.FromJson<GameData>(snapshot.GetRawJsonValue());
                    
                    DataManager.gameData = data; // �о�� �����͸� GameData�� ����                                       
                    DataManager.gameData.startDate = DateTime.Now.ToString();
                    LoadingScene.Instance.LaodingMain(); // ������ �б� ���� �� ���� �� �ε� ����
                }
                else
                {   // �ű�����            
                    GameData data = new GameData();
                    DataManager.gameData = data; // �ű� �����͸� GameData�� ����
                                        
                    LoadingScene.Instance.LaodingMain(); // ������ �б� ���� �� ���� �� �ε� ����                    
                }
            }
            else
            {
                Debug.LogError("������ �б� ����: " + task.Exception.ToString());
            }
        });
        #endregion

        #region HeroInfo
        reference.Child("USER").Child(currentUser.UserId).Child("HERO").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
               
                var data = JsonConvert.DeserializeObject<Dictionary<string, Info>>(snapshot.GetRawJsonValue());
                BaseManager.Data.HeroInfos = data; // �о�� �����͸� Infos�� ����

                BaseManager.Data.Init(); // ������ �б� ���� �� �ʱ�ȭ
            }
            else
            {
                Debug.LogError("���� ���� �б� ����: " + task.Exception.ToString());
            }
        });
        #endregion

        #region ItemInfo
        reference.Child("USER").Child(currentUser.UserId).Child("ITEM").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;

                var data = JsonConvert.DeserializeObject<Dictionary<string, Info>>(snapshot.GetRawJsonValue());
                BaseManager.Data.ItemInfos = data; // �о�� �����͸� Infos�� ����

                BaseManager.Data.Init(); // ������ �б� ���� �� �ʱ�ȭ
            }
            else
            {
                Debug.LogError("���� ���� �б� ����: " + task.Exception.ToString());
            }
        });
        #endregion
    }
}
