using UnityEngine;
using UnityEngine.UI;
public class FirebaseSetUp : MonoBehaviour
{
    public ChangeSceneWithButton CSB;
    public void registerUser(string username,string email, string UID)
    {
       
        Firebase.Database.DatabaseReference dbRef = Firebase.Database.FirebaseDatabase.DefaultInstance.RootReference;
        dbRef.Child("users").Child(UID).Child("username").SetValueAsync(username);
        dbRef.Child("users").Child(UID).Child("email").SetValueAsync(email);
        dbRef.Child("users").Child(UID).Child("Input").Child("x").SetValueAsync("0,0,0");
        dbRef.Child("users").Child(UID).Child("Input").Child("y").SetValueAsync("0,0,0");

        CSB.LoadScene();
    }

}


