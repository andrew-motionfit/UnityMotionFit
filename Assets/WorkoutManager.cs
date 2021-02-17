using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Michsky.UI.ModernUIPack;
using UnityEngine.Events;

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
    private void Awake()
    {
        startBt.material.SetColor("_Outline_Color", Color.blue);
      
    }

    private void Start()
    {
        loadworkoutData();
     
    }

    #region Circle Work button
    public void NewWorkout(WorkoutScriptableObject currentWorkout)
    {
      
        startBt.material.SetColor("_Outline_Color", Color.green);

       
          //  this.GetComponent<IHandleWorkouts>().NewWorkout(currentWorkout);
    }

    public void stopWorkout(WorkoutScriptableObject currentWorkout)
    {
        startBt.material.SetColor("_Outline_Color", Color.blue);
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
        string listAnimation = PlayerPrefs.GetString("Exerciseindex", "");
        string[] tempData = listAnimation.Split(","[0]);
        for (int i = 0; i < tempData.Length; i++)
        {
            int temp = int.Parse(tempData[i]);
            selectedAnimations.Add(nameofAnimations[temp]);
            print(nameofAnimations[temp]);
            HS.CreateNewItem(nameofAnimations[temp]);

        }
        HS.label.text = selectedAnimations[0];
        WH.animator.SetBool(HS.label.text, true);
    }

    public void readworkoutData()
    {
        for (int y = 0; y < nameofAnimations.Count; y++)
        {
            WH.animator.SetBool(nameofAnimations[y], false);
        }
        WH.animator.SetBool(HS.label.text, true);
    }

    public void Doneanimation()
    {
        for (int y = 0; y < nameofAnimations.Count; y++)
        {
            WH.animator.SetBool(nameofAnimations[y], false);
        }
    }

}
