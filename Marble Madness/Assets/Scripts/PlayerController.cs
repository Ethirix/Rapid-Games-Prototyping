using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // player id (set to 0 for single player, or increments for local multiplayer)
    public int id;

    public bool isInitialised;

    // movement
    public float maxSpeed, acceleration;
    private float maxSpeedSqr;
    private bool canMove;

    // scoring
    private int score;
    public int Score { get { return score; } set { score = value; } }

    Vector3 posSinceLastScore;

    public bool CanMove
    {
        get { return canMove; }
        set
        {
            canMove = value;

            if (canMove == true) RB.constraints = RigidbodyConstraints.None;
            else RB.constraints = RigidbodyConstraints.FreezeAll;
        }
    }

    float horizInput, vertInput;    

    Vector3 restartPoint;
    public Vector3 RestartPoint { get { return restartPoint; } }

    protected Rigidbody rb;
    public Rigidbody RB { get { return rb; } }

    void Awake()
    {
        rb = GetComponent<Rigidbody>();

        CanMove = false;
    }

    public void init()
    {
        // do not allow duplicate players
        if (GameManager.Instance.registerNewPlayer(id, this) == false)
        {
            Destroy(gameObject);
        }
        else
        {
            restartPoint = transform.position;
            maxSpeedSqr = maxSpeed * maxSpeed;

            isInitialised = true;

            DontDestroyOnLoad(gameObject);
        }
    }

    protected float getPlanarVelocitySqr()
    {
        return (rb.velocity.x * rb.velocity.x) + (rb.velocity.z * rb.velocity.z);
    }

    protected Vector3 getPlanarVelocityDirection()
    {
        return new Vector3(rb.velocity.x, 0, rb.velocity.z);
    }

    public void addScore(int scoreToAdd)
    {
        Score += scoreToAdd;

        UIManager.Instance.updateScore(id, Score);
    }

    // Update is called once per frame
    void Update()
    {
        if (CanMove == true)
        {
            horizInput = Input.GetAxis("Horizontal");
            vertInput = Input.GetAxis("Vertical");
        }
    }

    void FixedUpdate()
    {
        if (CanMove == true && (horizInput != 0 || vertInput != 0))
        {
            Vector3 direction = new Vector3(horizInput, 0, vertInput);

            direction = Quaternion.Euler(0, -135, 0) * direction;

            if (getPlanarVelocitySqr() < maxSpeedSqr)
            {
                rb.AddForce(direction * acceleration, ForceMode.Force);
            }

            if (getPlanarVelocitySqr() > maxSpeedSqr)
            {
                Vector3 planarDir = getPlanarVelocityDirection().normalized;
                rb.velocity = new Vector3(planarDir.x * maxSpeed, rb.velocity.y, planarDir.z * maxSpeed);
            }
        }
    }

    void LateUpdate()
    {
        if (CanMove == true && Vector3.Distance(transform.position, posSinceLastScore) > 3)
        {
            posSinceLastScore = transform.position;

            score += GameManager.Instance.pointsPerUnitTravelled;

            UIManager.Instance.updateScore(id, score);
        }
    }
}
