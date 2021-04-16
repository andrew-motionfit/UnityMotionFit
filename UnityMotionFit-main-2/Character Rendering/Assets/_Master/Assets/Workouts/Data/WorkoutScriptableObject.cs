using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Workout",menuName ="Workout")]
public class WorkoutScriptableObject : ScriptableObject
{    
    public AnimatorOverrideController workoutController = null;
    public GameObject rightHandAsset = null;
    public GameObject leftHandAsset = null;
    public GameObject twoHandedAsset = null;
}
