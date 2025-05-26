using Firebase.Database;
using Firebase.Extensions;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class User
{
    public string userName;
    public int Stage;
}

public partial class FirebaseManager
{
    
    public void WriteData()
    {
        GameData data = new GameData(); // GameData ��ü ����
        if(DataManager.gameData != null) // DataManager�� GameData�� ������
        { 
            data = DataManager.gameData; 
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
        string HeroDataJson = JsonConvert.SerializeObject(BaseManager.Data.Infos); // Infos�� JSON ���ڿ��� ��ȯ
        reference.Child("USER").Child(currentUser.UserId).Child("HERO").SetRawJsonValueAsync(HeroDataJson).ContinueWithOnMainThread(task =>
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

                    BaseManager.Data.Init(); // ������ �б� ���� �� �ʱ�ȭ
                    LoadingScene.Instance.LaodingMain(); // ������ �б� ���� �� ���� �� �ε� ����
                }
                else
                {   // �ű�����            
                    GameData data = new GameData();
                    DataManager.gameData = data; // �ű� �����͸� GameData�� ����

                    BaseManager.Data.Init(); // �ʱ� ������ ���� ���� �� �ʱ�ȭ
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
                BaseManager.Data.Infos = data; // �о�� �����͸� Infos�� ����

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
