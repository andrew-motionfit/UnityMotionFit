
using UnityEngine;
[RequireComponent(typeof(Camera))]
public class CameraRotationHandler : MonoBehaviour
{
    public Transform cameraJig;
    public float rotateSpeed;
    private bool _isFingerDown = false;
    private int fingerDirection = 0;
    private Touch t = default(Touch);
    private Vector3 startPosition = Vector3.zero;

    private void Update()
    {
        if (!_isFingerDown)
            return;


        if (fingerDirection == 1)
        {
            print("Applying = 1");
            transform.RotateAround(cameraJig.position, Vector3.up, rotateSpeed * Time.deltaTime);
        }
        else if (fingerDirection == 2)
        {
            print("Applying = 2");
            transform.RotateAround(cameraJig.position, -Vector3.up, rotateSpeed * Time.deltaTime);
        }

        if (Input.touches.Length > 0)
        {

            t = Input.touches[0];

            switch (t.phase)
            {
                case TouchPhase.Began:
                    startPosition = t.position;
                    return;
                case TouchPhase.Moved:
                    Vector3 positionDelta = (Vector3)t.position - startPosition;
                    
                                                           
                        if (positionDelta.x > 0)
                        {
                      //  transform.RotateAround(cameraJig.position, Vector3.up, rotateSpeed * Time.deltaTime);
                        fingerDirection = 1;
                       // print("Swiping = 1");
                        }
                        else
                        {
                      //  transform.RotateAround(cameraJig.position, -Vector3.up, rotateSpeed * Time.deltaTime);
                       // print("Swiping = 2");
                        fingerDirection = 2;
                        }
                    
                    return;
               
                default:
                    return;
            }
        }

       
    }
   public void fingerDown()
    {
        _isFingerDown = true;
    }

    public void fingerUp()
    {
        _isFingerDown = false;
        fingerDirection = 0;
    }
}
