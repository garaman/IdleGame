using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Database;

public partial class FirebaseManager
{
    private FirebaseAuth auth;  
    private FirebaseUser currentUser;
    private DatabaseReference reference;
    public void Init()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            if(task.Result == DependencyStatus.Available)
            {
                auth = FirebaseAuth.DefaultInstance;
                currentUser = auth.CurrentUser;
                
                reference = FirebaseDatabase.DefaultInstance.RootReference;
                

                GuestLogin();
            }
            else
            {
                Debug.LogError($"Could not resolve all Firebase dependencies: {task.Result}");
            }
        });

        
    }
}
