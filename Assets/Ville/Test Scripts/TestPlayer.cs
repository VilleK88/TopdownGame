using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayer : MonoBehaviour
{
    float moveSpeed = 5; // original speed 5
    Vector2 movement;
    Vector2 mousePos;
    Vector2 lookDir;
    float angle;
    //float horizontalInput;
    //float verticalInput;
    Rigidbody2D rb2d;

    public Camera cam;

    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Move();
        MouseAimin();
    }

    private void FixedUpdate()
    {
        rb2d.MovePosition(rb2d.position + movement * moveSpeed * Time.fixedDeltaTime);

        lookDir = mousePos - rb2d.position;
        angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg -90;
        rb2d.rotation = angle;
    }

    private void Move()
    {
        //horizontalInput = Input.GetAxis("Horizontal") * moveSpeed;
        //verticalInput = Input.GetAxis("Vertical") * moveSpeed;
        //rb2d.velocity = new Vector2(horizontalInput, verticalInput);

        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
    }

    private void MouseAimin()
    {
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
    }
}
