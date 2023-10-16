using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayer1 : MonoBehaviour
{
    Rigidbody rb;
    float horizontalInput;
    float forwardInput;
    float speed = 5;
    float jumpForce = 5;
    float gravityModifier = 1;
    bool running = false;
    int jumps = 0;

    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask ground;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        Physics.gravity *= gravityModifier;
    }

    private void Update()
    {
        Move();
        Jump();
    }

    private void Move()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            running = true;
        }
        else
        {
            running = false;
        }

        if (running)
        {
            speed = 10;
        }
        else
        {
            speed = 5;
        }

        horizontalInput = Input.GetAxis("Horizontal") * speed;
        forwardInput = Input.GetAxis("Vertical") * speed;
        rb.velocity = new Vector3(horizontalInput, rb.velocity.y, forwardInput);
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            jumps = 0;
            rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
        }
        else if (Input.GetKeyDown(KeyCode.Space) && jumps < 1)
        {
            rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
            jumps++;
        }
    }

    bool IsGrounded()
    {
        return Physics.CheckSphere(groundCheck.position, 1f, ground);
    }
}
