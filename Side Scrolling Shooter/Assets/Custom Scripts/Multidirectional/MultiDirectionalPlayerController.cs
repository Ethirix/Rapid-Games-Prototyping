using UnityEngine;
using System.Collections;

public class MultiDirectionalPlayerController : MonoBehaviour 
{
	public GameObject projectile;
	public Transform firePoint;
	public float maxSpeedReverse;
	public float maxSpeed;

    public float rotateSpeed;
	public float maxForce;
	public float fireRate;

	protected float horiz;
	protected float rotate;
	protected float thrust;
	protected Rigidbody2D myRigidbody;
	
	protected bool firing;
	protected float fireTimer;	
	
	void Awake()
	{
		myRigidbody = GetComponent<Rigidbody2D>();
	}
	
	void Update () 
	{
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

        horiz = Input.GetAxis("Horizontal");
        thrust = Input.GetAxis("Vertical");

        transform.Rotate(-horiz * Vector3.forward * rotateSpeed * Time.deltaTime);
 	}
	
	void FixedUpdate()
	{
		//transform.up uses local up, rotates with the player
        myRigidbody.AddForce(thrust * transform.up * maxForce);

		//MovingForward
		if(thrust > 0 && Mathf.Abs(myRigidbody.velocity.sqrMagnitude) > maxSpeed * maxSpeed)
		{
			myRigidbody.velocity = Vector2.ClampMagnitude(myRigidbody.velocity, maxSpeed);
		}
		//MovingBackward
		else if(thrust < 0 && Mathf.Abs(myRigidbody.velocity.sqrMagnitude) > maxSpeedReverse * maxSpeedReverse)
		{
			myRigidbody.velocity = Vector2.ClampMagnitude(myRigidbody.velocity, maxSpeedReverse);
		}
	}	
}
