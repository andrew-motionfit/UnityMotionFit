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
        print(UserNameInput.text);
        Login(UserNameInput.text, PasswordInput.text);
    }
    public void SignupcallButton()
    {
        Signup(SignUpgmail.text, PasswordInput.text, SignUpusername.text);
    }

    private void Login(string email, string password)
    {
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
                return;
            }

             user = task.Result;
            FS.CSB.LoadScene();
            Debug.LogFormat("User signed in successfully: {0} ({1})",
                user.DisplayName, user.UserId);

          

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
