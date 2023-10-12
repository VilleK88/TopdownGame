using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCameraController : MonoBehaviour
{
    Camera cam;
    [SerializeField] Transform player;
    float cameraHeight = 10;
    float rotationSpeed = 5;
    Vector3 playerPosition;
    Vector3 cameraPosition;
    float horizontalInput;
    float verticalInput;
    Vector3 inputDirection;
    float targetAngle;
    float currentAngle;

    private void Update()
    {
        //transform.position = new Vector3(player.position.x, transform.position.y, player.position.z);

        if(player != null)
        {
            playerPosition = player.position;
            cameraPosition = new Vector3(player.position.x, player.position.y + cameraHeight,
                player.position.z);
            transform.position = cameraPosition;

            horizontalInput = Input.GetAxis("Horizontal");
            verticalInput = Input.GetAxis("Vertical");
            if(horizontalInput != 0 || verticalInput != 0)
            {
                inputDirection = new Vector3(horizontalInput, 0, verticalInput).normalized;
                targetAngle = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg +
                    player.eulerAngles.y;
                currentAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle,
                    ref rotationSpeed, 0.1f);
                transform.rotation = Quaternion.Euler(90, currentAngle, 0);
            }
        }
    }
}
