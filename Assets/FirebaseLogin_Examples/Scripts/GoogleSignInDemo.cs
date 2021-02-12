using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Firebase;
using Firebase.Auth;
using Firebase.Extensions;
using Google;
using UnityEngine;
using UnityEngine.UI;

public class GoogleSignInDemo : MonoBehaviour
{
    public string webClientId = "<your client id here>";
    public FirebaseSetUp FS;
    private FirebaseAuth auth;
    private GoogleSignInConfiguration configuration;
    private string CurrentUsername, CurrentEmail, CurrentUid;
    private void Awake()
    {
        configuration = new GoogleSignInConfiguration { WebClientId = webClientId, RequestEmail = true, RequestIdToken = true };
        CheckFirebaseDependencies();
    }

    private void CheckFirebaseDependencies()
    {      
       auth = FirebaseAuth.DefaultInstance;        
    }

    public void SignInWithGoogle() { OnSignIn(); }
    public void SignOutFromGoogle() { OnSignOut(); }

    private void OnSignIn()
    {
        GoogleSignIn.Configuration = configuration;
        GoogleSignIn.Configuration.UseGameSignIn = false;
        GoogleSignIn.Configuration.RequestIdToken = true;
       

        GoogleSignIn.DefaultInstance.SignIn().ContinueWith(OnAuthenticationFinished);
    }

    private void OnSignOut()
    {
       
        GoogleSignIn.DefaultInstance.SignOut();
    }

    public void OnDisconnect()
    {
       
        GoogleSignIn.DefaultInstance.Disconnect();
    }

    internal void OnAuthenticationFinished(Task<GoogleSignInUser> task)
    {
        if (task.IsFaulted)
        {
            using (IEnumerator<Exception> enumerator = task.Exception.InnerExceptions.GetEnumerator())
            {
                if (enumerator.MoveNext())
                {
                    GoogleSignIn.SignInException error = (GoogleSignIn.SignInException)enumerator.Current;
                   
                }
                else
                {
                   
                }
            }

            FS.CSB.infotxt.text = "User with " + task.Result.Email + " already have a account please try sign in";
            FS.CSB.WarningPanel.SetActive(true);

        }
        else if (task.IsCanceled)
        {
            FS.CSB.infotxt.text = "User with " + task.Result.Email + " already have a account please try sign in";
            FS.CSB.WarningPanel.SetActive(true);
        }
        else
        {
         
            SignInWithGoogleOnFirebase(task.Result.IdToken);
            CurrentUsername = task.Result.DisplayName;
            CurrentEmail = task.Result.Email;
            CurrentUid = task.Result.UserId;
        }
    }

    private void SignInWithGoogleOnFirebase(string idToken)
    {
        Credential credential = GoogleAuthProvider.GetCredential(idToken, null);

        auth.SignInWithCredentialAsync(credential).ContinueWithOnMainThread(task =>
        {
                AddToInformation(CurrentUsername,CurrentEmail,CurrentUid);
                
            
        });
    }

    public void OnSignInSilently()
    {
        GoogleSignIn.Configuration = configuration;
        GoogleSignIn.Configuration.UseGameSignIn = false;
        GoogleSignIn.Configuration.RequestIdToken = true;
       

        GoogleSignIn.DefaultInstance.SignInSilently().ContinueWith(OnAuthenticationFinished);
    }

    public void OnGamesSignIn()
    {
        GoogleSignIn.Configuration = configuration;
        GoogleSignIn.Configuration.UseGameSignIn = true;
        GoogleSignIn.Configuration.RequestIdToken = false;

       

        GoogleSignIn.DefaultInstance.SignIn().ContinueWith(OnAuthenticationFinished);
    }

    private void AddToInformation(string us_name,string Em,string U_id) {

        FS.registerUser(us_name,Em,U_id);
    }
}