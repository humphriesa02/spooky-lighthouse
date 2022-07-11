using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Movement : MonoBehaviour
{
    public float playerSpeed = 8;
    public float maxSpeed = 10;
    public float inAirSpeed = 500;
    public float jumpForce = 1000;
    public float runTransition = 20;
    private int colliderAmount;
    private Vector3 forward;
    private Vector3 right;
    private Rigidbody rb;
    private State state;
    private Animator ani;

    enum State
    {
        Default,
        InAir
    }

    // Start is called before the first frame update
    void Start()
    {
        state = State.Default;
        forward = new Vector3();
        right = new Vector3();
        rb = GetComponent<Rigidbody>();
        ani = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < -22)
        {
            transform.position = new Vector3(0, 2.69f, 12.43f);
        }

        //Movement Helper
        Vector3 cameraVector = Camera.main.transform.rotation.eulerAngles;
        cameraVector.x = cameraVector.z = 0;

        forward = Quaternion.Euler(cameraVector) * Vector3.forward;
        right = Quaternion.Euler(cameraVector) * Vector3.right;
        forward.y = right.y = 0;


        // State Machine
        if (colliderAmount > 0)
        {
            state = State.Default;
        }
        else
        {
            state = State.InAir;
        }

        //Raycast; mean't for future use, nothing implemented

       

        //Movement

        switch (state)
        {
            case State.Default:
                {
                    GroundMovement();
                    break;
                }
            case State.InAir:
                {
                    InAirMovement();
                    break;
                }
            default: break;
        }
    }

    private void InAirMovement()
    {
        if (XVelocityGood())
        {
            rb.AddForce(Input.GetAxis("Vertical") * forward * inAirSpeed * rb.mass * Time.deltaTime);
        }
        if (ZVelocityGood())
        {
            rb.AddForce(Input.GetAxis("Horizontal") * right * inAirSpeed * rb.mass * Time.deltaTime);
        }
    }

    private void GroundMovement()
    {
        Vector3 movementVec = Vector3.Lerp(rb.velocity, (Input.GetAxis("Vertical") * forward + Input.GetAxis("Horizontal") * right) * playerSpeed, runTransition * Time.deltaTime);
        movementVec.y = rb.velocity.y;
        rb.velocity = movementVec;
        if (Input.GetKey(KeyCode.W))
        {
            ani.SetBool("IsRunning", true);
        }
        else
        {
            ani.SetBool("IsRunning", false);
        }
        if (Input.GetKeyDown("space"))
        {
            rb.AddForce(jumpForce * Vector3.up, ForceMode.Impulse);
        }
    }


    private bool XVelocityGood()
    {
        return -maxSpeed < rb.velocity.x && rb.velocity.x < maxSpeed;
    }

    private bool ZVelocityGood()
    {
        return -maxSpeed < rb.velocity.z && rb.velocity.z < maxSpeed;
    }

    private void OnTriggerEnter(Collider other)
    {
        colliderAmount++;
    }

    private void OnTriggerExit(Collider other)
    {
        colliderAmount--;
    }
}
