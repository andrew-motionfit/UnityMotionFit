using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TapGizmos : MonoBehaviour
{

	private bool _isStart = false;
	public GameObject Avater;
	public WorkoutScriptableObject WS;
	private void Awake()
	{
		GetComponent<Renderer>().material.SetColor("_Outline_Color", Color.blue);
	}

	void OnMouseUp()
	{
		if (!_isStart)
		{
			GetComponent<Renderer>().material.SetColor("_Outline_Color", Color.green);
			Avater.GetComponent<WorkoutManager>().NewWorkout(WS);
			_isStart = true;
		}
        else
        {

			GetComponent<Renderer>().material.SetColor("_Outline_Color", Color.blue);
			_isStart = false;
		}
	}

	
}
