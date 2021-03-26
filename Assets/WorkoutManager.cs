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

    [Header("CounterPanel")]
    public GameObject CounterPanel;
    public TextMeshProUGUI Countertxt;
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
            StartCoroutine(CounterCountDown());
            _isIKon = true;
        }
        else
        {
            stopWorkout(currentWorkoutSO);
            _isIKon = false;
        }
    }

    IEnumerator CounterCountDown()
    {
        CounterPanel.SetActive(true);
        int temp = 5;
        Countertxt.fontSize = 400;
        Countertxt.text = temp.ToString();
        while(temp > 0)
        {
            yield return new WaitForSeconds(1);
            temp -= 1;
            Countertxt.text = temp.ToString();
        }

        CounterPanel.SetActive(false);
        StartCoroutine(StartPosCounter());
        readworkoutData();
    }
    IEnumerator StartPosCounter()
    {
        CounterPanel.SetActive(true);
        int temp = 10;
        Countertxt.fontSize = 150;
        Countertxt.text = "Set Youre Position Exercise will start In" + temp.ToString();
        while (temp > 0)
        {
            yield return new WaitForSeconds(1);
            temp -= 1;
            Countertxt.text = "Set Youre Position Exercise will start In" + temp.ToString();
            if(temp > 1)
            {
                WH.animator.speed = 0;
            }
        }

        CounterPanel.SetActive(false);
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
        switch (counter)
        {
            case 1:
                innerList(workout1,true);
                innerList(workout2, false);
                break;
            case 2:
                innerList(workout1, false);
                innerList(workout2, true);
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
