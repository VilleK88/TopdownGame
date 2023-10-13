using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayer : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField] GameObject childSprite;
    public Camera cam;

    [Header("Player Movement and Mouse Parameters")]
    float moveSpeed = 5;
    Vector3 movement;
    Ray ray;
    RaycastHit hit;
    Vector3 cursorPosition;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        childSprite.GetComponent<Animator>();
    }

    private void Update()
    {
        Move();
        MouseAiming();
        Attack();
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

    void Attack()
    {
        if(Input.GetMouseButtonDown(0))
        {
            childSprite.GetComponent<Animator>().SetTrigger("AxeAttack1");
        }
    }
}
