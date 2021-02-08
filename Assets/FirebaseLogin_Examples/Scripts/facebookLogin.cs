using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Facebook.Unity;
using Firebase.Auth;
using Firebase.Extensions;

public class facebookLogin : MonoBehaviour
{
    public FirebaseSetUp FS;
  
    // Use this for initialization
    void Awake()
        {
        if (!FB.IsInitialized)
            {
                // Initialize the Facebook SDK
                FB.Init(InitCallback, OnHideUnity);
            }
            else
            {
                // Already initialized, signal an app activation App Event
                FB.ActivateApp();
            }
        }
        private void InitCallback()
        {
            if (FB.IsInitialized)
            {
                // Signal an app activation App Event
                FB.ActivateApp();
                // Continue with Facebook SDK
                // ...
            }
            else
            {
                Debug.Log("Failed to Initialize the Facebook SDK");
            }
        }

        private void OnHideUnity(bool isGameShown)
        {
            if (!isGameShown)
            {
                // Pause the game - we will need to hide
                Time.timeScale = 0;
            }
            else
            {
                // Resume the game - we're getting focus again
                Time.timeScale = 1;
            }
        }
        public void FBlogin()
        {
            List<string> permissions = new List<string>();
            permissions.Add("public_profile");
            permissions.Add("email");
            permissions.Add("user_friends");
            FB.LogInWithReadPermissions(permissions, AuthCallback);
        }
        private void AuthCallback(ILoginResult result)
        {
            if (FB.IsLoggedIn)
            {
                Firebase.Auth.FirebaseAuth auth = 
          Firebase.Auth.FirebaseAuth.DefaultInstance;
                List<string> permissions = new List<string>();

                // AccessToken class will have session details
                var aToken = Facebook.Unity.AccessToken.CurrentAccessToken;
                // Print current access token's User ID
                Debug.Log(aToken.TokenString);
                Debug.Log(aToken.UserId);


                Firebase.Auth.Credential credential =

            Firebase.Auth.FacebookAuthProvider.GetCredential(aToken.TokenString);
                auth.SignInWithCredentialAsync(credential).ContinueWithOnMainThread(task => {
                    if (task.IsCanceled)
                    {
                        Debug.LogError("SignInWithCredentialAsync was canceled.");
                        return;
                    }
                    if (task.IsFaulted)
                    {
                        Debug.LogError("SignInWithCredentialAsync encountered an error: " + task.Exception);
                        return;
                    }

                    Firebase.Auth.FirebaseUser newUser = task.Result;
                    Debug.LogFormat("User signed in successfully: {0} ({1})",newUser.DisplayName, newUser.UserId);
                   
                    FS.registerUser(newUser.DisplayName,newUser.Email,newUser.UserId);
                          
                });
                // Print current access token's granted permissions
                //foreach(string perms in aToken.Permissions)
                //{
                // Debug.Log(perms);
                //}
            }
            else
            {
                Debug.Log("User cancelled login");
            }
        }
    }
