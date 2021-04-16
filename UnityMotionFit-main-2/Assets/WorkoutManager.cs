using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Michsky.UI.ModernUIPack;
using UnityEngine.Events;
using TMPro;
public class WorkoutManager : MonoBehaviour
{
    [SerializeField] private GameObject player = null;

    public Renderer startBt;


    [Header("workoutScene1")]
    public List<GameObject> workout1 = new List<GameObject>();

    [Header("workoutScene2")]
    public List<GameObject> workout2 = new List<GameObject>();
    private int sceneSwitcher = 0;

    public List<string> nameofAnimations = new List<string>();
    public List<string> selectedAnimations = new List<string>();
    public WorkoutHandler WH;
    public HorizontalSelector HS;
    public WorkoutScriptableObject currentWorkoutSO;
    private bool _isIKon;
    private bool _startcounting;
    private int anicounter;

    [Header("POPUP")]
    public GameObject popupPanel;
    public TextMeshProUGUI PopupText;
    public ModalWindowManager myModalWindow;
    private void Awake()
    {
        startBt.material.SetColor("_Outline_Color", Color.black);
        startBt.material.SetFloat("TileX", 0.05f);
        startBt.material.SetFloat("TileY", 0.05f);

    }

    private void Start()
    {
        loadworkoutData();
     
    }
    private void LateUpdate()
    {
        OnCompleteAttackAnimation();   
    }

    void OnCompleteAttackAnimation()
    {
        if (!_startcounting)
            return;

        if (WH.animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            LocalDatabase.instance.repData(HS.label.text,anicounter.ToString());
            anicounter += 1;
        }
        // TODO: Do something when animation did complete
    }
    public void centerworkoutBt()
    {
        if (!_isIKon)
        {
            NewWorkout(currentWorkoutSO);
            StartCoroutine(startCounter());
            _isIKon = true;
        }
        else
        {
            stopWorkout(currentWorkoutSO);
            Doneanimation();
            _isIKon = false;
        }
    }

    IEnumerator startCounter()
    {
        Doneanimation();
        int temp = 5;
        popupPanel.SetActive(true);
        PopupText.fontSize = 400;
        PopupText.text = temp.ToString();
        while (temp > 0)
        {
            yield return new WaitForSeconds(1);
            temp -= 1;
            PopupText.text = temp.ToString();
           
        }
        readworkoutData();
        StartCoroutine(startingPosCounter());
    }

    IEnumerator startingPosCounter()
    {
        int temp = 10;
        popupPanel.SetActive(true);
        PopupText.fontSize = 150;
        PopupText.text = "Exercise will start in "+ temp.ToString()+" sec";
        while (temp > 0)
        {
            yield return new WaitForSeconds(1);
            temp -= 1;
            PopupText.text = "Exercise will start in " + temp.ToString() + " sec";
            if(temp > 1)
            {
                WH.animator.speed = 0;
            }
        }

        popupPanel.SetActive(false);
        WH.animator.speed = 1;
    }
    #region Circle Work button
    public void NewWorkout(WorkoutScriptableObject currentWorkout)
    {
      
        startBt.material.SetColor("_Outline_Color", Color.blue);
        startBt.material.SetFloat("_TileX",10.0f);
        startBt.material.SetFloat("_TileY", 10.0f);

        //  this.GetComponent<IHandleWorkouts>().NewWorkout(currentWorkout);
    }

    public void stopWorkout(WorkoutScriptableObject currentWorkout)
    {
        startBt.material.SetColor("_Outline_Color", Color.blue);
        startBt.material.SetFloat("_TileX", 0.05f);
        startBt.material.SetFloat("_TileY", 0.05f);
        //if (player)
        //    player.GetComponent<IHandleWorkouts>().StopWorkout(currentWorkout);
    }
    public void ReadUserData()
    {
        player.GetComponent<IHandleWorkouts>().ReadUserData();
    }

    #endregion
    public void switchBt(int counter)
    {
        print("HERE");
        //  myModalWindow.icon = "spriteVariable; // Change icon
        if (counter == 2)
        {
            myModalWindow.titleText = "SEE DATA"; // Change title
            myModalWindow.descriptionText = "Viewing Data will stop Exercise, Unsave Reps will be lost"; // Change desc
        }
        else if (counter == 1)
        {
            myModalWindow.titleText = "BACK TO EXERCISE"; // Change title
            myModalWindow.descriptionText = "You can go back to Exercise and start Exercise"; // Change desc
        }
        else if (counter == 0)
        {
            myModalWindow.titleText = "PREVIEW EXERCISE"; // Change title
            myModalWindow.descriptionText = "Preview will stop exercise, Unsave Reps will be lost"; // Change desc
        }
        else if (counter == 3)
        {
            myModalWindow.titleText = "Main Menu"; // Change title
            myModalWindow.descriptionText = "Do you really want to go to Main Menu, Unsave Reps will be lost"; // Change desc
        }
        myModalWindow.onConfirm.RemoveAllListeners();
        myModalWindow.onConfirm.AddListener(delegate { answer(counter); });
        myModalWindow.UpdateUI(); // Update UI
       // myModalWindow.OpenWindow(); // Open window
        myModalWindow.AnimateWindow(); // Close/Open window automatically
       
    }

    public void answer(int counter)
    {
        Doneanimation();
        switch (counter)
        {
            case 0:
               
                readworkoutData();
                break;
            case 1:
                innerList(workout1, true);
                innerList(workout2, false);
                break;
            case 2:
                innerList(workout1, false);
                innerList(workout2, true);
                break;
            case 3:
                Application.LoadLevel(1);
                break;
            default:
                print("Incorrect");
                break;
        }
    }

  
    void innerList(List<GameObject> temp , bool condition)
    {
        for(int i = 0; i < temp.Count; i++)
        {
            temp[i].SetActive(condition);
        }
    }


    public void loadworkoutData()
    {
      LocalDatabase.instance.Loadworkout();
        StartCoroutine(loadingWorkoutData());
      
    }

    IEnumerator loadingWorkoutData()
    {

        while (LocalDatabase.instance.workoutData.Length <= 0)
        {
           
            yield return null;
        }
       
        string listAnimation = LocalDatabase.instance.workoutData;
        string[] tempData = listAnimation.Split(","[0]);
        for (int i = 0; i < tempData.Length; i++)
        {
            int temp = int.Parse(tempData[i]);
            selectedAnimations.Add(nameofAnimations[temp]);
            print(nameofAnimations[temp]);
            HS.CreateNewItem(nameofAnimations[temp]);

        }
        HS.label.text = selectedAnimations[0];
        
    }
    public void readworkoutData()
    {
        for (int y = 0; y < nameofAnimations.Count; y++)
        {
            WH.animator.SetBool(nameofAnimations[y], false);
        }
        WH.animator.SetBool(HS.label.text, true);
        _startcounting = true;
    }

    public void Doneanimation()
    {
        for (int y = 0; y < nameofAnimations.Count; y++)
        {
            WH.animator.SetBool(nameofAnimations[y], false);
        }

        _startcounting = false;
    }

}
