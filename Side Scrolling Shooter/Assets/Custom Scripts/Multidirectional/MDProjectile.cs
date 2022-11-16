using UnityEngine;
using System.Collections;

public class MDProjectile : MonoBehaviour 
{
	public float speed;
	
	void Update() 
	{
		Vector2 move = transform.up * speed;
		transform.position = new Vector2(transform.position.x + move.x * Time.deltaTime, transform.position.y + move.y * Time.deltaTime);
	}
	
	void OnBecameInvisible()
	{
		Destroy(gameObject);
	}
	
	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "Enemy")
		{
			Destroy(other.gameObject);
			Destroy(gameObject);
		}
	}
}
