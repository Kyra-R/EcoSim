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

    private Transform target;

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
            HandleSelection();

            if(target == null)
            {
                HandleZoom();
                HandleMovement();
                HandleMouseDrag();
            } else {
                FollowTarget();
                HandleDeselection();
            }
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


    private void HandleSelection()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

            if (hit.collider != null)
            {
                Animal animal = hit.collider.GetComponent<Animal>();

                if (animal != null)
                {
                    target = animal.transform;

                    cam.orthographicSize = minZoom;

                    return;
                }
            }

        }
    }


    private void HandleDeselection()
    {
        if (Input.GetMouseButtonDown(1))
        {
            target = null;
        }
    }

    private void FollowTarget()
    {
        if (target == null) return;

        Vector3 targetPos = new Vector3(target.position.x, target.position.y, transform.position.z);

        //less "jumps" during following
        transform.position = Vector3.Lerp(
            transform.position,
            targetPos,
            Time.deltaTime * 5f
        );

        //limit via wordborders
        ClampCameraPosition();
    }



}
