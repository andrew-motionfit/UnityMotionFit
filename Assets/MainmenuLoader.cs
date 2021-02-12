using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class MainmenuLoader : MonoBehaviour
{
    public GameObject FadeImage;
    public TextMeshProUGUI profileUsertxt;
    // Start is called before the first frame update
    private int Levelint;

    private void Awake()
    {
        if (profileUsertxt == null)
            return;
        profileUsertxt.text = "Hello "+PlayerPrefs.GetString("username","");
        
    }
    public void mainbt(int level)
    {
        FadeImage.SetActive(true);
        Invoke("changeLevel", 1);
        Levelint = level;
    }

    public void changeLevel()
    {
        Application.LoadLevel(Levelint);
    }
}
