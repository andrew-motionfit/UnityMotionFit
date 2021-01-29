using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Michsky.UI.ModernUIPack;
[RequireComponent(typeof(Animator))]
public class WorkoutHandler : MonoBehaviour, IHandleWorkouts
{
    public Transform playerRightHand = null;
    public Transform playerLeftHand = null;


    [SerializeField] private Transform rightHandPosition;
    [SerializeField] private Transform leftHandPosition;


    Animator animator = null;
    private bool isReadingPlayerData = false;

    public bool usingLeftHand = false;
    public bool usingRightHand = false;

    public WorkoutScriptableObject workout;
    public HorizontalSelector HS;
    private int tempValue = 0;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        playerRightHand.SetParent(rightHandPosition);
        playerLeftHand.SetParent(leftHandPosition);
        playerRightHand.localPosition = Vector3.zero;
        playerLeftHand.localPosition = Vector3.zero;
        if (workout.leftHandAsset)
        {

            var leftHandAsset = Instantiate(workout.leftHandAsset);
            leftHandAsset.transform.SetParent(playerLeftHand);
            leftHandAsset.transform.localPosition = Vector3.zero;
            leftHandAsset.transform.localRotation = Quaternion.identity;
        }

        if (workout.rightHandAsset)
        {

            var rightHandAsset = Instantiate(workout.leftHandAsset);
            rightHandAsset.transform.SetParent(playerRightHand);
            rightHandAsset.transform.localPosition = Vector3.zero;
            rightHandAsset.transform.localRotation = Quaternion.identity;
        }

          GetComponent<Animator>().SetBool("shoulderpress",false);
            Invoke("SetIK", 1f);
            tempValue = 0;

    }
    public void NewWorkout()
    {
        if (HS.index == tempValue)
            return;


        usingLeftHand = false;
        usingRightHand = false;
        animator = GetComponent<Animator>();
        playerRightHand.SetParent(rightHandPosition);
        playerLeftHand.SetParent(leftHandPosition);
        playerRightHand.localPosition = Vector3.zero;
        playerLeftHand.localPosition = Vector3.zero;
        isReadingPlayerData = false;
        if (HS.index == 0 && tempValue != 0)
        {
            GetComponent<Animator>().SetBool("shoulderpress",false);
            Invoke("SetIK", 2f);
            tempValue = 0;
        }
        else if(HS.index == 1 && tempValue != 1)
        {
            GetComponent<Animator>().SetBool("shoulderpress", true);
            Invoke("SetIK",2f);
            tempValue = 1;
        }
   
    }

    public void ReadUserData()
    {
        isReadingPlayerData = true;
    }

     public void SetIK()
    {
        usingLeftHand = true;
        usingRightHand = true;
        playerRightHand.SetParent(null);
        playerLeftHand.SetParent(null);
        //    animator.runtimeAnimatorController = workout.workoutController;
        //  animator.SetBool("isWorkoutActive", true);
        

    }

    private void OnAnimatorIK(int layerIndex)
    {
        //if (isReadingPlayerData)
        //{
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



     //   }
    }
}
