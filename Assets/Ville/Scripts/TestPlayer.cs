using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayer : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField] GameObject childSprite;
    public Camera cam;

    [Header("Player Movement and Mouse Aiming Parameters")]
    float moveSpeed = 5;
    Vector3 movement;
    Ray ray;
    RaycastHit hit;
    Vector3 cursorPosition;
    bool running = false;

    [Header("Jump Parameters")]
    float jumpForce = 5;
    float gravityModifier = 1;
    int jumps = 0;
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask ground;

    public bool blocking; // if player is blocking or not


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        childSprite.GetComponent<Animator>();
        Physics.gravity *= gravityModifier;
    }

    private void Update()
    {
        Move();
        MouseAiming();
        Block();
        if(!blocking)
        {
            Jump();
            Attack();
        }
    }

    private void FixedUpdate()
    {
        FixedMove();
    }

    void Move()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.z = Input.GetAxisRaw("Vertical");
    }

    void FixedMove()
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    private void MouseAiming()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if(Physics.Raycast(ray, out hit))
        {
            cursorPosition = hit.point - transform.position;

            transform.forward = new Vector3(cursorPosition.x, 0, cursorPosition.z);
        }
    }

    public void Attack()
    {
        if(Input.GetMouseButtonDown(0))
        {
            childSprite.GetComponent<Animator>().SetTrigger("AxeAttack1");
        }
    }

    void Block()
    {
        if(Input.GetKey(KeyCode.Q))
        {
            childSprite.GetComponent<Animator>().SetBool("AxeBlock", true);
            blocking = true;
        }
        else
        {
            childSprite.GetComponent<Animator>().SetBool("AxeBlock", false);
            blocking = false;
        }
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
