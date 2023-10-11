using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayer : MonoBehaviour
{
    Rigidbody2D rb2d;
    public Camera cam;

    [Header("Player Movement and Mouse Aiming")]
    float moveSpeed = 5;
    Vector2 movement;
    Vector2 mousePos;
    Vector2 lookDir;
    float angle;

    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Move();
        MouseAiming();
    }

    private void FixedUpdate()
    {
        MoveFixed();
        MouseAimingFixed();
    }

    private void Move()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
    }
    void MoveFixed()
    {
        rb2d.MovePosition(rb2d.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    private void MouseAiming()
    {
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
    }

    void MouseAimingFixed()
    {
        lookDir = mousePos - rb2d.position;
        angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90;
        rb2d.rotation = angle;
    }
}
