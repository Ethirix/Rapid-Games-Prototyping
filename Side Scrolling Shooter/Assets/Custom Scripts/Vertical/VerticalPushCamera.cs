using UnityEngine;
using System.Collections;

public class VerticalPushCamera : MonoBehaviour
{
	public float speed;
	
	// Update is called once per frame
	void Update () 
	{
		transform.position = new Vector3(transform.position.x, transform.position.y + (speed * Time.deltaTime), transform.position.z);
	}
}
