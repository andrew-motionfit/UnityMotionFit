using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkoutManager : MonoBehaviour
{
    [SerializeField] private GameObject player = null;  
    public void NewWorkout(WorkoutScriptableObject currentWorkout)
    {
        if (player)
            player.GetComponent<IHandleWorkouts>().NewWorkout(currentWorkout);
    }

    public void ReadUserData()
    {
        player.GetComponent<IHandleWorkouts>().ReadUserData();
    }
}
