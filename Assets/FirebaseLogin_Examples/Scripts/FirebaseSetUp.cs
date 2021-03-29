using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class FirebaseSetUp : MonoBehaviour
{
    public ChangeSceneWithButton CSB;


    private void Start()
    {
        
        CSB.Fade.enabled = true;
        Invoke("checkLogin",2);
    }

    public void registerUser(string username,string email, string UID)
    {

        Firebase.Database.DatabaseReference dbRef = Firebase.Database.FirebaseDatabase.DefaultInstance.RootReference;
        dbRef.Child("users").Child(UID).Child("username").SetValueAsync(username);
        dbRef.Child("users").Child(UID).Child("email").SetValueAsync(email);
        dbRef.Child("users").Child(UID).Child("email").SetValueAsync(email);
        dbRef.Child("users").Child(UID).Child("characterselect").SetValueAsync("0");
        dbRef.Child("users").Child(UID).Child("Input").Child("x").SetValueAsync("0,0,0");
        dbRef.Child("users").Child(UID).Child("Input").Child("y").SetValueAsync("0,0,0");
        LocalDatabase.instance.saveData(username,email,UID);
        CSB.LoadScene();
    }

    public void checkLogin()
    {



        if(PlayerPrefs.GetString("uid","").Length > 0)
        {
           List<string> tempSignData = LocalDatabase.instance.Getvalue();
            if (PlayerPrefs.GetString("loginMethod", "C") == "C")
            {
                print(tempSignData[1] );
                GetComponent<CustomAuth>().Login(tempSignData[1], PlayerPrefs.GetString("password", "0").ToString());
            }
            else if (PlayerPrefs.GetString("loginMethod", "C") == "F")
            {
                GetComponent<facebookLogin>().FBlogin();
            }
            else if (PlayerPrefs.GetString("loginMethod", "C") == "G")
            {
                GetComponent<GoogleSignInDemo>().SignInWithGoogle();
            }

            //   CSB.LoadScene();
            return;
        }

        CSB.Fade.enabled = false;
    }

}


