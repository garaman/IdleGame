using Firebase.Auth;
using UnityEngine;


public partial class FirebaseManager
{
    public void GuestLogin()
    {
        if(auth.CurrentUser != null)
        {
            Debug.Log("이미 로그인 되어있습니다." + auth.CurrentUser.UserId);
            return;
        }
        auth.SignInAnonymouslyAsync().ContinueWith(task =>
        {
            if (task.IsCanceled || task.IsFaulted)
            {
                Debug.LogError("GusetLogin 실패.");
                return;
            }
                        
            FirebaseUser user = task.Result.User;
            // Unique ID 로 생성됨.
            Debug.Log($"GusetLogin 성공. 사용자 ID: {user.UserId}");
        });
    }
}
