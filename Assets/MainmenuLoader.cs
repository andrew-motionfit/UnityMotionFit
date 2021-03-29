using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class MainmenuLoader : MonoBehaviour
{
    public GameObject FadeImage;
    public TextMeshProUGUI profileUsertxt;
    public List<GameObject> ListofPlayerModel = new List<GameObject>();
    // Start is called before the first frame update
    private int Levelint;

    private void Awake()
    {
        if (profileUsertxt == null)
            return;
        profileUsertxt.text = "Hello "+PlayerPrefs.GetString("username","");
        
    }
    private void Start()
    {
       StartCoroutine(LocalDatabase.instance.getCharacter(ListofPlayerModel,FadeImage));
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
