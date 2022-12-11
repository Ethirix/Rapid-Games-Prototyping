using UnityEngine;
using System.Collections;

public class HorizontalPlayerController : MonoBehaviour
{
	public GameObject projectile;
	public Transform firePoint;
	public float maxSpeed;
	public float moveForce;
	public float fireRate;
	
	protected float horizVelocity;
	protected float vertVelocity;
	protected Rigidbody2D myRigidbody;
	
	protected bool firing;
	protected float fireTimer;	
	
	protected HorizontalPushCamera cam;
		
	void Start()
	{
		cam = Camera.main.GetComponent<HorizontalPushCamera>();
		myRigidbody = GetComponent<Rigidbody2D>();
	}
		
	// Update is called once per frame
	void Update () 
	{
		transform.position = new Vector3(transform.position.x + (cam.speed * Time.deltaTime), transform.position.y, transform.position.z);
	
		if (firing == true)
		{
			fireTimer += Time.deltaTime;
			
			if (fireTimer >= fireRate)
			{
				firing = false;
				fireTimer = 0;	
			}	
		}
	
		if (Input.GetButton("Fire1") && firing == false)
		{
			Instantiate (projectile, firePoint.position, transform.rotation);
			firing = true;
		}
		
		horizVelocity = Input.GetAxis("Horizontal");
		vertVelocity = Input.GetAxis("Vertical");
	}
	
	void FixedUpdate()
	{
		myRigidbody.AddForce (horizVelocity * Vector2.right * moveForce);
		
		myRigidbody.AddForce (vertVelocity * Vector2.up * moveForce);
		
		if (Mathf.Abs (myRigidbody.velocity.sqrMagnitude) > maxSpeed*maxSpeed)
		{
			myRigidbody.velocity = Vector2.ClampMagnitude(myRigidbody.velocity, maxSpeed);
		}			
	}
}
