using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
public class ChangeSceneWithButton : MonoBehaviour
{
    public Image Fade;
    public GameObject WarningPanel;
    public TextMeshProUGUI infotxt;
    public void LoadScene()
    {
        Fade.enabled = true;

        Invoke("Restart",1);
    }
    private void Restart()
    {
        SceneManager.LoadScene("MainScreen");
    }


    //public void LoadScene(string sceneName)
    //{
    //    print("HERERR");
    //    SceneManager.LoadScene(sceneName);
    //}


}
