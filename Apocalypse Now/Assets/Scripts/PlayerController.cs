using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //Movement Checks
    [Header("Physics")]
    public float accelerationCoefficient;   //how quickly it speeds up
    public float maxVelocity;               //how fast it can go horizontally
    public float friction;                  //how quickly it slows down
    public float speedModifier;             //modifiers applied to the player (affects maxVelocity)

    //Input options
    [Header("Movement Controls")]
    public KeyCode up = KeyCode.W;
    public KeyCode down = KeyCode.S;
    public KeyCode left = KeyCode.A;
    public KeyCode right = KeyCode.D;

    //Physics info
    private Vector2 velocity, acceleration;


    Rigidbody2D rbody;

    // Pause all input besides escape
    public bool pauseInput = false;

    // Start is called before the first frame update
    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!pauseInput)
        {
            acceleration.x = ((Input.GetKey(left) ? -1 : 0) + (Input.GetKey(right) ? 1 : 0)) * accelerationCoefficient;
            acceleration.y = ((Input.GetKey(down) ? -1 : 0) + (Input.GetKey(up) ? 1 : 0)) * accelerationCoefficient;

            //Calculate velocity
            velocity.x = VelocityCalc(acceleration.x, velocity.x, speedModifier);
            velocity.y = VelocityCalc(acceleration.y, velocity.y, speedModifier);

            //Predict new position
            Vector2 currentPos = rbody.position;
            Vector2 newPos = currentPos + velocity * Time.fixedDeltaTime;

            //Move to new position
            rbody.MovePosition(newPos);
        }
    }

    private float VelocityCalc(float a, float v, float modifier = 1f)
    {
        //  a = Acceleration
        //  v = Velocity

        //Accelerate
        if (Mathf.Abs(a) > 0f && Mathf.Abs(v) <= maxVelocity * modifier)
        {
            v += a * modifier * Time.deltaTime;
            v = Mathf.Clamp(v, -maxVelocity * modifier, maxVelocity * modifier);
        }
        //Account for friction
        else if (Mathf.Abs(v) > 0f)
        {
            //Reduce our absolute velocity
            v = Mathf.Sign(v) * Mathf.Max(Mathf.Abs(v) - friction * modifier * Time.deltaTime, 0f);
        }

        //Return velocity bound by maxVelocity
        return v;
    }

    public void StopMovement()
    {
        velocity = Vector2.zero;
        acceleration = Vector2.zero;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        StopMovement();
    }

    public Vector2 GetVelocity()
    {
        return velocity;
    }

}