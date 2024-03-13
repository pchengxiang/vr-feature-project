using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

[Serializable]
public class CameraLook
{
    public bool LockCursor
    {
        get { return Cursor.lockState == CursorLockMode.Locked; }
        set
        {
            Cursor.visible = !value;
            Cursor.lockState = value ? CursorLockMode.Locked : CursorLockMode.None;
        }
    }

    [Header("Control Settings")]
    public float mouseSensitivity = 300.0f;

    private float xRotation = 0f;

    private float yRotation = 0f;

    public float viewAngle = 90f;
    [Header("Forbid to Look Up/Down or Left/Right")]
    [SerializeField]
    private bool lockLookUpAndDown;

    public bool LockLookUpAndDown
    {
        get { return lockLookUpAndDown; }
        set { lockLookUpAndDown = value;}
    }

    [SerializeField]
    private bool lockLookLeftAndRight;

    public bool LockLookLeftAndRight
    {
        get { return lockLookUpAndDown; }
        set { lockLookUpAndDown = value; }
    }

    Transform transform;

    public Transform Transform 
    { 
        get { return transform; } 
        set { transform = value; }
    }

    Transform cameraTransform;

    public Transform CameraTransform
    {
        get { return cameraTransform; }
        set { cameraTransform = value; }
    }
    
    public void Rotate(Vector2 direction)
    {
        if(!lockLookUpAndDown)
        {
            float mouseY = direction.y * mouseSensitivity * Time.deltaTime;
            // 上下看，以X轴旋转
            yRotation -= mouseY;
            yRotation = Mathf.Clamp(yRotation, -viewAngle, viewAngle);
        }
        if (!lockLookLeftAndRight)
        {
            // 鼠标输入
            float mouseX = direction.x * mouseSensitivity * Time.deltaTime;
            xRotation -= mouseX;
        }
        cameraTransform.localRotation = Quaternion.Euler(yRotation, xRotation, 0);
    }
}
