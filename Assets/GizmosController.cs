using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshCollider))]

public class GizmosController : MonoBehaviour
{

	private Vector3 screenPoint;
	private Vector3 offset;
	public bool _isRight, _isLeft;
	public GameObject Avater;

	void OnMouseDown()
	{
		if (transform.parent != null)
			return;


		screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
		offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
	}

	void OnMouseDrag()
	{
		if (transform.parent != null)
			return;


		Vector3 cursorPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
		Vector3 cursorPosition = Camera.main.ScreenToWorldPoint(cursorPoint) + offset;
		transform.position = cursorPosition;
		sendData();
	}


	void sendData()
    {
        if (_isRight)
        {
			_isLeft = false;

			Avater.GetComponent<FirebaseDatabase>().UpdateData(transform.position.ToString(),"Right");
        }
		else if (_isLeft)
        {
			_isRight = false;
			Avater.GetComponent<FirebaseDatabase>().UpdateData(transform.position.ToString(), "Left");
		}
    }

}