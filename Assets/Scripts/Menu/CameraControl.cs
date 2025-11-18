using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;

public class CameraController2D : MonoBehaviour
{
    [Header("Zoom Settings")]
    public float zoomSpeed = 5f;
    public float minZoom = 20f;
    public float maxZoom = 55f;

    [Header("Movement Settings")]
    public float moveSpeed = 10f;
    public Vector2 minPosition;
    public Vector2 maxPosition;

    [Header("Mouse Drag Settings")]
    public float dragSpeed = 2f;
    private Vector3 lastMousePosition;

    public Camera cam;

    public EnvironmentController environmentController;

    private bool inMenu = true;

    void Awake()
    {
        if(environmentController == null)
        environmentController = GameObject.Find("EnvironmentController").GetComponent<EnvironmentController>();

        if(cam == null)
        cam = GetComponent<Camera>();
    }


    public void ChangeInMenu()
    {
        inMenu = !inMenu;
    }

    void Update()
    {
        if(!inMenu){
            HandleZoom();
            HandleMovement();
            HandleMouseDrag();
        }
    }

    public void UpdateMaxZoom()
    {
        float viewDiagonalFactor = Mathf.Sqrt(cam.aspect * cam.aspect + 1f);

        float newZoom = environmentController.worldBorder / viewDiagonalFactor;

        if(newZoom >= minZoom){
            maxZoom = newZoom;
        } else {
            maxZoom = minZoom;
            
        }
        cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, minZoom, maxZoom);
    }

    private void HandleZoom()
    {
        float scrollData = Input.GetAxis("Mouse ScrollWheel");
        if (scrollData != 0.0f)
        {
            cam.orthographicSize -= scrollData * zoomSpeed;
            cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, minZoom, maxZoom);
        }
    }

    private void HandleMovement()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");

        Vector3 move = new Vector3(moveX, moveY, 0) * moveSpeed * Time.deltaTime;
        transform.position += move;

        ClampCameraPosition();
    }

    private void HandleMouseDrag()
    {
        if (Input.GetMouseButtonDown(1))
            lastMousePosition = Input.mousePosition;

        if (Input.GetMouseButton(1))
        {
            Vector3 delta = Input.mousePosition - lastMousePosition;

            Vector3 move = new Vector3(-delta.x, -delta.y, 0) * dragSpeed * cam.orthographicSize * Time.deltaTime;
            transform.position += move;

            lastMousePosition = Input.mousePosition;

            ClampCameraPosition();
        }
    }

    private void ClampCameraPosition()
    {
        float camHeight = cam.orthographicSize;
        float camWidth = camHeight * cam.aspect;
    if(maxZoom != minZoom){
        float limitX = environmentController.worldBorder - camWidth + 1.0f;
        float limitY = environmentController.worldBorder - camHeight + 1.0f;

        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, -limitX, limitX),
            Mathf.Clamp(transform.position.y, -limitY, limitY),
            transform.position.z
        );
    } else {
        transform.position = new Vector3(
            0,
            0,
            transform.position.z);
    }
    }
}
