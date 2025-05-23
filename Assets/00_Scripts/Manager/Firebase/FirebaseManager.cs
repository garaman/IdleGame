using UnityEngine;
using Firebase;
using Firebase.Auth;

public partial class FirebaseManager
{
    private FirebaseAuth auth;  
    private FirebaseUser currentUser;

    public void Init()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            if(task.Result == DependencyStatus.Available)
            {
                auth = FirebaseAuth.DefaultInstance;
                currentUser = auth.CurrentUser;
                Debug.Log("Firebase Auth Initialized");

                GuestLogin();
            }
            else
            {
                Debug.LogError($"Could not resolve all Firebase dependencies: {task.Result}");
            }
        });
    }
}
