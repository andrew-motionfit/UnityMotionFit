using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class WorkoutHandler : MonoBehaviour, IHandleWorkouts
{
    public Transform playerRightHand = null;
    public Transform playerLeftHand = null;


    [SerializeField] private Transform rightHandPosition;
    [SerializeField] private Transform leftHandPosition;


    public Animator animator = null;
    private bool isReadingPlayerData = false;

    private bool usingLeftHand = false;
    private bool usingRightHand = false;
    public void NewWorkout(WorkoutScriptableObject workout)
    {
        isReadingPlayerData = false;

        animator.runtimeAnimatorController = workout.workoutController;
        animator.SetBool("isWorkoutActive", true);
        if (workout.leftHandAsset)
        {
            usingLeftHand = true;
            var leftHandAsset = Instantiate(workout.leftHandAsset);
            leftHandAsset.transform.parent = leftHandPosition;
            leftHandAsset.transform.localPosition = Vector3.zero;
            leftHandAsset.transform.localRotation = Quaternion.identity;
        }

        if (workout.rightHandAsset)
        {
            usingRightHand = true;
            var rightHandAsset = Instantiate(workout.leftHandAsset);
            rightHandAsset.transform.parent = rightHandPosition;
            rightHandAsset.transform.localPosition = Vector3.zero;
            rightHandAsset.transform.localRotation = Quaternion.identity;
        }
    }

    public void StopWorkout(WorkoutScriptableObject workout)
    {
        animator.runtimeAnimatorController = workout.workoutController;
        animator.SetBool("isWorkoutActive", false);
    }

    public void ReadUserData()
    {
        isReadingPlayerData = true;
    }

    private void OnAnimatorIK(int layerIndex)
    {
        if (isReadingPlayerData)
        {
            if (usingLeftHand)
            {
                animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
                animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1);

                animator.SetIKPosition(AvatarIKGoal.LeftHand, playerLeftHand.position);
                animator.SetIKRotation(AvatarIKGoal.LeftHand, playerLeftHand.rotation);
            }


            if (usingRightHand)
            {
                animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
                animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1);

                animator.SetIKPosition(AvatarIKGoal.RightHand, playerRightHand.position);
                animator.SetIKRotation(AvatarIKGoal.RightHand, playerRightHand.rotation);
            }



        }
    }

    
}
