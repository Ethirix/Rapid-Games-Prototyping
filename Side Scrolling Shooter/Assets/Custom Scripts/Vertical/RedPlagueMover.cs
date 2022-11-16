using UnityEngine;
using System.Collections;

public class RedPlagueMover : MonoBehaviour 
{
	public float vSpeed;
	public float maxXOffset;
	protected float origXPos;
	
	void Start()
	{
		origXPos = transform.position.x;
	}
	
	// Update is called once per frame
	void Update () 
	{
		transform.position = new Vector2(origXPos + maxXOffset * Mathf.Sin(Time.time), transform.position.y  - (vSpeed * Time.deltaTime));
	}
	
	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "Player")
		{
			Destroy (other.gameObject);
		}
	}
}
