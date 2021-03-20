using UnityEngine;
using Vatio.Filters;


public class AccelerometerObjectControl : MonoBehaviour
{

   
    public float a = 0.05f;

    AccelerometerControl accelerometerControl;
    LowPassFilter<Vector3> lowPassFilter;

    void Start()
    {
       
      lowPassFilter = new LowPassFilter<Vector3>(a, Vector3.zero);
        
    }

  
    public Vector3 filterPos(Vector3 dataV) 
   {
        lowPassFilter.Append(dataV);
        return lowPassFilter.Get();              
    }
}
