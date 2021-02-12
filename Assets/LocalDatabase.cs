using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalDatabase : MonoBehaviour
{
    public string username;
    public string gmail;
    public string UID;

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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
