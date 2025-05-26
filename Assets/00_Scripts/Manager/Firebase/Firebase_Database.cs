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
        GameData data = new GameData(); // GameData 객체 생성
        if(DataManager.gameData != null) // DataManager의 GameData를 가져옴
        { 
            data = DataManager.gameData; 
        }

        #region GameData
        string GameDataJson = JsonUtility.ToJson(DataManager.gameData);
        reference.Child("USER").Child(currentUser.UserId).Child("DATA").SetRawJsonValueAsync(GameDataJson).ContinueWithOnMainThread(task =>
        {
            if (!task.IsCompleted)
            {
                Debug.LogError("데이터 저장 실패: " + task.Exception.ToString());
            }
        });
        #endregion

        #region HeroInfo
        string HeroDataJson = JsonConvert.SerializeObject(BaseManager.Data.Infos); // Infos를 JSON 문자열로 변환
        reference.Child("USER").Child(currentUser.UserId).Child("HERO").SetRawJsonValueAsync(HeroDataJson).ContinueWithOnMainThread(task =>
        {
            if (!task.IsCompleted)
            {
                Debug.LogError("데이터 저장 실패: " + task.Exception.ToString());
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
                    
                    DataManager.gameData = data; // 읽어온 데이터를 GameData에 저장                   

                    BaseManager.Data.Init(); // 데이터 읽기 성공 후 초기화
                    LoadingScene.Instance.LaodingMain(); // 데이터 읽기 성공 후 메인 씬 로딩 시작
                }
                else
                {   // 신규유저            
                    GameData data = new GameData();
                    DataManager.gameData = data; // 신규 데이터를 GameData에 저장

                    BaseManager.Data.Init(); // 초기 데이터 생성 성공 후 초기화
                    LoadingScene.Instance.LaodingMain(); // 데이터 읽기 성공 후 메인 씬 로딩 시작                    
                }
            }
            else
            {
                Debug.LogError("데이터 읽기 실패: " + task.Exception.ToString());
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
                BaseManager.Data.Infos = data; // 읽어온 데이터를 Infos에 저장

                BaseManager.Data.Init(); // 데이터 읽기 성공 후 초기화
            }
            else
            {
                Debug.LogError("영웅 정보 읽기 실패: " + task.Exception.ToString());
            }
        });
        #endregion
    }
}
