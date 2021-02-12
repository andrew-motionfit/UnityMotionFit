using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHandler : MonoBehaviour
{
    float rotationSpeed = 0.2f;
    bool _canRotate = false;
    [SerializeField] 
    private float mouseyminiClamp = 270;
    [SerializeField]
    private float mouseymixClamp = 440;
    private void Update()
    {
       
            if (Input.GetMouseButtonDown(0))
            {

                _canRotate = true;
            }
           
        


        if (_canRotate)
        {
            float XaxisRotation = Input.GetAxis("Mouse X") * rotationSpeed;
            transform.RotateAround(Vector3.down, XaxisRotation);
        }

        if (Input.GetMouseButtonUp(0))
        {
            _canRotate = false;
        }
    }

   

}
