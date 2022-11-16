using UnityEngine;
using System.Collections;

public class ButterflyMover : MonoBehaviour 
{
	public float hSpeed;
	public float maxYOffset;
	protected float origYPos;
	
	void Start()
	{
		origYPos = transform.position.y;
	}
		
	// Update is called once per frame
	void Update () 
	{
		transform.position = new Vector2(transform.position.x - (hSpeed * Time.deltaTime), origYPos + maxYOffset * Mathf.Sin(Time.time));
	}
	
	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "Player")
		{
			Destroy (other.gameObject);
		}
	}
}
