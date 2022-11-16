using UnityEngine;
using System.Collections;

public class MDFollowCam : MonoBehaviour 
{
	public Transform followTarget;
	
	void Update()
	{
		transform.position = new Vector3(followTarget.position.x, followTarget.position.y, transform.position.z);
	}
}
