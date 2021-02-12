using UnityEngine;
interface IHandleWorkouts
{
     void NewWorkout(WorkoutScriptableObject workout);
    void StopWorkout(WorkoutScriptableObject workout);
    void ReadUserData();
}
