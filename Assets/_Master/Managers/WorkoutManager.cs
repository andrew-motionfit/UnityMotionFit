using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkoutManager : MonoBehaviour
{
    [SerializeField] private GameObject player = null;

    public Renderer startBt;


    private void Awake()
    {
        startBt.material.SetColor("_Outline_Color", Color.blue);
    }
    public void NewWorkout(WorkoutScriptableObject currentWorkout)
    {
      
        startBt.material.SetColor("_Outline_Color", Color.green);

        if (player)
            player.GetComponent<IHandleWorkouts>().NewWorkout(currentWorkout);
    }

    public void stopWorkout(WorkoutScriptableObject currentWorkout)
    {
        startBt.material.SetColor("_Outline_Color", Color.blue);
        if (player)
            player.GetComponent<IHandleWorkouts>().StopWorkout(currentWorkout);
    }
    public void ReadUserData()
    {
        player.GetComponent<IHandleWorkouts>().ReadUserData();
    }

    public void stopWorkout()
    {

    }
}
