using UnityEngine;
using System.Collections;

public class HorizontalPushCamera : MonoBehaviour 
{
	public float speed;
		
	// Update is called once per frame
	void Update () 
	{
		transform.position = new Vector3(transform.position.x + (speed * Time.deltaTime), transform.position.y, transform.position.z);
	}
}
