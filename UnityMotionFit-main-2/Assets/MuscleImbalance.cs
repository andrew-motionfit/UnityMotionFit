using UnityEngine;
 using System.Collections;
 
 public class MuscleImbalance : MonoBehaviour {

     public GameObject MainCamera;    
     public GameObject RightArm;
     public GameObject LeftArm;

        
    public void EnableCamera0(){
        RightArm.SetActive(false);
        LeftArm.SetActive(false);
        MainCamera.SetActive(true);

        }
 
     public void EnableCamera1() {
         RightArm.SetActive(true);
         LeftArm.SetActive(false);
         MainCamera.SetActive(false);

     }
     public void EnableCamera2() {
         RightArm.SetActive(false);
         LeftArm.SetActive(true);
         MainCamera.SetActive(false);
     }
 }