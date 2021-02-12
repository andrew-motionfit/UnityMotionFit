using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Wolf3D.ReadyPlayerMe.AvatarSDK;
using UnityEditor;

public class AvatarLoaderProfile : MonoBehaviour
{
    public GameObject warningMessage;
    public TMP_InputField LinkField;
    public GameObject FadeImage;
    public Transform AvaterParent;


    private void Start()
    {

   
        
        if (PlayerPrefs.GetString("geturl","").Length > 1)
        {
          
           // FadeImage.SetActive(true);
            AvatarLoader avatarLoader = new AvatarLoader();
            avatarLoader.LoadAvatar(PlayerPrefs.GetString("geturl", ""), AvatarLoadedCallback);
        }
    }
    public void GetAvatar()
    {
     //  FadeImage.SetActive(true);
        AvatarLoader avatarLoader = new AvatarLoader();
        PlayerPrefs.SetString("geturl", LinkField.text);
        avatarLoader.LoadAvatar(LinkField.text, AvatarLoadedCallback);
    }
    private void AvatarLoadedCallback(GameObject avatar)
    {
        avatar.transform.parent = AvaterParent;
        avatar.transform.localPosition = Vector3.zero;
        avatar.transform.localEulerAngles = Vector3.zero;
        AvaterParent.transform.GetChild(0).transform.gameObject.SetActive(false);

        if(AvaterParent.GetComponent<WorkoutHandler>() != null)
        {
            AvaterParent.GetComponent<WorkoutHandler>().animator = avatar.GetComponentInChildren<Animator>();
        }
        FadeImage.SetActive(false);
       
    }

    public void OpenURL()
    {
        
        Application.OpenURL("https://motionfit.readyplayer.me/avatar");
        warningMessage.SetActive(true);
        
    }


    public void mainbt()
    {
        FadeImage.SetActive(true);
        Invoke("changeLevel",1);
    }

    public void changeLevel()
    {
        Application.LoadLevel(1);
    }
}
