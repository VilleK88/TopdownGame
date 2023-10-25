using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCameraController : MonoBehaviour
{
    Camera cam;
    [SerializeField] Transform player;

    public Vector3 offset;
    float currentZoom = 10;
    public float pitch = 2;
    public float zoomSpeed = 4;
    public float minZoom = 5;
    public float maxZoom = 15;

    private void Update()
    {
        //transform.position = new Vector3(player.position.x, transform.position.y, player.position.z);
        currentZoom -= Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
    }

    private void LateUpdate()
    {
        //transform.position = player.position - offset * currentZoom;
        //transform.LookAt(player.position + Vector3.up * pitch);
        transform.position = new Vector3(player.position.x, transform.position.y * currentZoom, player.position.z);
    }
}
