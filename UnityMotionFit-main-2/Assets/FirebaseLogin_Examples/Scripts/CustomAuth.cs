using Firebase.Auth;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Firebase.Extensions;

public class CustomAuth : MonoBehaviour
{
    public TMP_InputField UserNameInput, PasswordInput;
    public TMP_InputField SignUpusername, SignUppassword, SignUpgmail;

    FirebaseAuth auth;
    private FirebaseUser user;
    public FirebaseSetUp FS;
    // Start is called before the first frame update
    void Start()
    {
        UserNameInput.text = "demofirebase@gmail.com";
        PasswordInput.text = "abcdefgh";

        //SignupButton.onClick.AddListener(() => Signup(SignUpgmail.text, PasswordInput.text, SignUpusername.text));
        //LoginButton.onClick.AddListener(() => Login(UserNameInput.text, PasswordInput.text));
         auth = FirebaseAuth.DefaultInstance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void UpdateErrorMessage(string message)
    {
       
        Invoke("ClearErrorMessage", 3);
    }

    void ClearErrorMessage()
    {
       
    }

    public void LogincallButton()
    {
      
        Login(UserNameInput.text, PasswordInput.text);
    }
    public void SignupcallButton()
    {
        print(SignUpgmail.text +" "+ SignUppassword.text);
        Signup(SignUpgmail.text, SignUppassword.text, SignUpusername.text);
    }

    public void Login(string email, string password)
    {
        print(password);
        auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("SignInWithEmailAndPasswordAsync canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("SignInWithEmailAndPasswordAsync error: " + task.Exception);
                if (task.Exception.InnerExceptions.Count > 0)
                    UpdateErrorMessage(task.Exception.InnerExceptions[0].Message);


                FS.CSB.Fade.enabled = false;
                return;
            }

             user = task.Result;
            FS.CSB.LoadScene();
            LocalDatabase.instance.saveData(user.DisplayName, email, user.UserId);
             PlayerPrefs.SetString("password", password);
            PlayerPrefs.SetString("loginMethod", "C");

        });
    }

    public void Signup(string email, string password, string username)
    {
        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(username))
        {
            //Error handling
            return;
        }

        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("CreateUserWithEmailAndPasswordAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("CreateUserWithEmailAndPasswordAsync error: " + task.Exception);
                string temp = task.Exception.ToString();
                if(temp.Contains("The email address is already in use by another account"))
                {
                  
                    FS.CSB.infotxt.text = "User with "+ email +" already have a account please try sign in";
                    FS.CSB.WarningPanel.SetActive(true);
                }
                if (task.Exception.InnerExceptions.Count > 0)
                    UpdateErrorMessage(task.Exception.InnerExceptions[0].Message);
                return;
            }

            user = task.Result; // Firebase user has been created.
            FS.registerUser(username,email,user.UserId);
            Debug.LogFormat("Firebase user created successfully: {0} ({1})",
                user.DisplayName, user.UserId);
            UpdateErrorMessage("Signup Success");

        });
    }
}
