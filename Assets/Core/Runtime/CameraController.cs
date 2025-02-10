using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    private CameraControlActions cameraActions;
    private InputAction movement;
    private Transform cameraTransfrom;

    //水平移动
    [SerializeField]
    private float maxSpeed = 5f;
    private float speed;
    [SerializeField]
    private float acceleration = 10f;
    [SerializeField]
    private float damping = 15f;

    //垂直移动+放大缩小
    [SerializeField]
    private float stepSize = 2f;
    [SerializeField]
    private float zoomDampening = 7.5f;
    [SerializeField]
    private float minHeight = 5f;
    [SerializeField]
    private float maxHeight = 50f;
    [SerializeField]
    private float zoomSpeed = 2f;
    [SerializeField]
    private float zoomCorrection = 1f;

    //旋转
    [SerializeField]
    private float maxRotationSpeed = 1f;

    //屏幕边缘运动
    [SerializeField]
    [Range(0f, 0.1f)]
    private float edgeTolerance = 0.05f;
    [SerializeField]
    private bool useScreenEdge = true;

    // 中间使用的变量
    //相机的位置
    private Vector3 targetPositions;

    //相机的高度
    private float zoomHeight;

    //用于在没有刚体的情况下跟踪和保持速度
    private Vector3 horizontalVelocity;
    private Vector3 lastPosition;

    //初始点位
    Vector3 startDrag;


    private void Awake()
    {
        cameraActions = new CameraControlActions();
        cameraTransfrom = GetComponentInChildren<Camera>().transform;

    }
    private void OnEnable()
    {
        zoomHeight = cameraTransfrom.localPosition.y;
        cameraTransfrom.LookAt(transform);

        lastPosition = transform.position;
        movement = cameraActions.Camera.Movement;
        cameraActions.Camera.RotateCamera.performed += RotateCamera;
        cameraActions.Camera.ZoomCamera.performed += ZoomCamera;
        cameraActions.Camera.Enable();
    }



    private void OnDisable()
    {
        cameraActions.Camera.RotateCamera.performed -= RotateCamera;
        cameraActions.Camera.ZoomCamera.performed -= ZoomCamera;
        cameraActions.Disable();
    }

    private void Update()
    {
        GetKeyboardMovement();
        if (useScreenEdge)
        {
            CheckMouseAtScreenEdge();
        }
        DragCamera();

        UpdateVelocity();
        UpdateCameraPosition();
        UpdateBasePosition();
    }
    private void UpdateVelocity()
    {
        horizontalVelocity = (transform.position - lastPosition) / Time.deltaTime;
        horizontalVelocity.y = 0;
        lastPosition = transform.position;

    }
    private void GetKeyboardMovement()
    {
        Vector3 inputValue = movement.ReadValue<Vector2>().x * GetCameraRight() + movement.ReadValue<Vector2>().y * GetCameraForward();

        inputValue = inputValue.normalized;

        if (inputValue.sqrMagnitude > 0.1f)
        {
            targetPositions = inputValue;
        }
    }
    private Vector3 GetCameraRight()
    {
        Vector3 right = cameraTransfrom.right;
        right.y = 0;
        return right;
    }
    private Vector3 GetCameraForward()
    {
        Vector3 forward = cameraTransfrom.forward;
        forward.y = 0;
        return forward;
    }

    private void UpdateBasePosition()
    {
        if (targetPositions.sqrMagnitude > 0.1f)
        {
            speed = Mathf.Lerp(speed, maxSpeed, Time.deltaTime * acceleration);
            transform.position += targetPositions * speed * Time.deltaTime;
        }
        else
        {
            horizontalVelocity = Vector3.Lerp(horizontalVelocity, Vector3.zero, Time.deltaTime * damping);
            transform.position += horizontalVelocity * Time.deltaTime;
        }
        targetPositions = Vector3.zero;
    }
    private void RotateCamera(InputAction.CallbackContext inputValue)
    {
        if (!Mouse.current.middleButton.isPressed)
            return;

        float value = inputValue.ReadValue<Vector2>().x;
        transform.rotation = Quaternion.Euler(0f, value * maxRotationSpeed + transform.rotation.eulerAngles.y, 0f);
    }
    private void ZoomCamera(InputAction.CallbackContext inputValue)
    {
        float value = -inputValue.ReadValue<Vector2>().y / zoomCorrection;

        Debug.Log("zoom" + value);

        if (Mathf.Abs(value) > 0.1f)
        {
            Debug.Log("zoomValue" + value);
            zoomHeight = cameraTransfrom.localPosition.y + value * stepSize;
            if (zoomHeight < minHeight)
            {
                zoomHeight = minHeight;
            }
            else if (zoomHeight > maxHeight)
            {
                zoomHeight = maxHeight;
            }
        }
    }
    private void UpdateCameraPosition()
    {
        Vector3 zoomTarget = new Vector3(cameraTransfrom.localPosition.x, zoomHeight, cameraTransfrom.localPosition.z);
        zoomTarget -= zoomSpeed * (zoomHeight - cameraTransfrom.localPosition.y) * Vector3.forward;

        cameraTransfrom.localPosition = Vector3.Lerp(cameraTransfrom.localPosition, zoomTarget, Time.deltaTime * zoomDampening);
        cameraTransfrom.LookAt(transform);
    }

    private void CheckMouseAtScreenEdge()
    {
        Vector2 mousePosition = Mouse.current.position.ReadValue();
        Vector3 moveDirection = Vector3.zero;

        if(mousePosition.x < edgeTolerance * Screen.width)
        {
            moveDirection += -GetCameraRight();
        }
        else if(mousePosition.x > (1f - edgeTolerance) * Screen.width)
        {
            moveDirection += GetCameraRight();
        }

        if (mousePosition.y < edgeTolerance * Screen.height)
        {
            moveDirection += -GetCameraForward();
        }
        else if (mousePosition.y > (1f - edgeTolerance) * Screen.height)
        {
            moveDirection += GetCameraForward();
        }

        targetPositions += moveDirection;

    }

    private void DragCamera()
    {
        if(!Mouse.current.rightButton.isPressed)
        {
            return;
        }

        Plane plane = new Plane(Vector3.up, Vector3.zero);
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

        if(plane.Raycast(ray,out float distance))
        {
            if(Mouse.current.rightButton.wasPressedThisFrame)
            {
                startDrag = ray.GetPoint(distance);
            }
            else
            {
                targetPositions += startDrag - ray.GetPoint(distance);
            }

        }
    }

}
