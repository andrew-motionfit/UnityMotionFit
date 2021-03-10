using Firebase.Database;
using Firebase.Extensions;
using Firebase.Storage;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class LocalDatabase : MonoBehaviour
{
    public string username;
    public string gmail;
    public string UID;
    public string workoutData;
    public static LocalDatabase instance;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

       
    }
    private void Start()
    {
        FirebaseStorage storage = FirebaseStorage.DefaultInstance;

        // Create a storage reference from our storage service
        StorageReference storageRef =
            storage.GetReferenceFromUrl("gs://motionfit-878e2.appspot.com/");

    }

    public List<string> Getvalue()
    {
        username = PlayerPrefs.GetString("username", "");
        gmail = PlayerPrefs.GetString("gmail", "");
        UID = PlayerPrefs.GetString("uid", "");
        List<string> InfoData = new List<string>();
        InfoData.Add(username);
        InfoData.Add(gmail);
        InfoData.Add(UID);
        return InfoData;
    }

    public void saveData(string usernameP,string gmailP,string uidP)
    {
        username = usernameP;
        gmail = gmailP;
        UID = uidP;
        PlayerPrefs.SetString("username",username);
        PlayerPrefs.SetString("gmail", gmail);
        PlayerPrefs.SetString("uid", UID);

    }

    public void saveWorkout(string Data)
    {

        Firebase.Database.DatabaseReference dbRef = Firebase.Database.FirebaseDatabase.DefaultInstance.RootReference;
        dbRef.Child("users").Child(UID).Child("workout").SetValueAsync(Data);  
    }
    public void Loadworkout()
    {
        Firebase.Database.DatabaseReference dbRef = Firebase.Database.FirebaseDatabase.DefaultInstance.RootReference;
      dbRef.Child("users").Child(UID).Child("workout").GetValueAsync().ContinueWithOnMainThread(task => {
            if (task.IsFaulted)
            {
                // Failure
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
              workoutData = snapshot.Value.ToString();
                // Success
            }
        });
    }
    public Image Photo;
    public void repData()
    {
        GameObject.FindObjectOfType<UploadGameData>().Trigger();
    }
}
