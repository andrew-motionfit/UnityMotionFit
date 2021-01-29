using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class FirebaseDatabase : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        FirebaseApp app = FirebaseApp.DefaultInstance;
     
    }


    
    private void writeNewUser()
    {
        User user = new User("email@gmail.com", "password");
        string json = JsonUtility.ToJson(user);
        Firebase.Database.DatabaseReference dbRef = Firebase.Database.FirebaseDatabase.DefaultInstance.RootReference;
          dbRef.Child("users").SetRawJsonValueAsync(json);
    }

    public void UpdateData(string valueinString,string username)
    {
        Firebase.Database.DatabaseReference dbRef = Firebase.Database.FirebaseDatabase.DefaultInstance.RootReference;

        dbRef.Child("users").Child("MSA-Y5s_AL3U47yKVFA").Child(username).SetValueAsync(valueinString);
    }

    // Update is called once per frame
    void Update()
    {
      
    }
}
public class User
{
    public string username;
    public string email;

    public User()
    {
    }

    public User(string username, string email)
    {
        this.username = username;
        this.email = email;
    }
}
