using Firebase.Auth;
using UnityEngine;


public partial class FirebaseManager
{
    public void GuestLogin()
    {
        if(auth.CurrentUser != null)
        {
            Debug.Log("�̹� �α��� �Ǿ��ֽ��ϴ�." + auth.CurrentUser.UserId);
            return;
        }
        auth.SignInAnonymouslyAsync().ContinueWith(task =>
        {
            if (task.IsCanceled || task.IsFaulted)
            {
                Debug.LogError("GusetLogin ����.");
                return;
            }
                        
            FirebaseUser user = task.Result.User;
            // Unique ID �� ������.
            Debug.Log($"GusetLogin ����. ����� ID: {user.UserId}");
        });
    }
}
