using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerControl : MonoBehaviour
{
    public CharacterController Controller;
    public CameraLook Perspective;
    
    public Camera PlayerCamera;
    [Header("UI Related")]
    public UIController UIController;
    public float ClickUIDelay = 0.1f;
    float timer = 0f;

    [Header("Player Properties")]
    public float speed = 1.0f;
    public float rushMultiply = 1.0f;
    float terminalSpeed;
    Vector3 direction = new Vector3(0,0,0);

    [Header("Virtual Game Pad Import")]
    public VirtualGamePadControl gamePadControl;

# region Read Input Values
    public Vector2 Direction
    {
        get
        {
            return Control.KeyBoard.Move.ReadValue<Vector2>();
        }
    }

    Vector2 perspectiveDirection;
    public Vector2 PerspectiveDirection
    {
        get
        {
            return Control.KeyBoard.Look.ReadValue<Vector2>();
        }
    }

    public bool TryingMove
    {
        get
        {
            return Control.KeyBoard.Move.IsPressed();
        }
    }

    public bool TryingRush
    {
        get
        {
            return Control.KeyBoard.Rush.IsPressed();
        }
    }

    public bool TryingInteraction
    {
        get
        {
            return Control.KeyBoard.Interaction.IsPressed();
        }
    }
    public bool TryingEscape
    {
        get
        {
            return Control.KeyBoard.Escape.IsPressed();
        }
    }



    public bool TryingMovePerspective
    {
        get
        {
            return Control.KeyBoard.Look.IsPressed();
        }
    }

#endregion
    private bool isMoving;
    public bool IsMoving
    {
        get { return isMoving; }
    }

    BasicControl Control;
    void Awake()
    {
        if(Controller == null)
            Controller = GetComponent<CharacterController>();
        if(PlayerCamera == null)
            PlayerCamera = GetComponentInChildren<Camera>();
    }

    public void Start()
    {
        Perspective.Transform = Controller.transform;
        Perspective.CameraTransform = PlayerCamera.transform;
        Control = new BasicControl();
        Control.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        if (UIController != null && TryingInteraction && Time.time - timer >= ClickUIDelay)
        {
            timer = Time.time;
            if (!UIController.Use)
                OpenRoomUI();
            else if (UIController.Use)
                EndAction();
        }
        if(TryingEscape)
            EndAction();
       
        isMoving = TryMove();
        if(!isMoving && TryingMovePerspective)
            Perspective.Rotate(PerspectiveDirection);
    }

    /// <summary>
    /// Perspective.Rotate()這邊放一個是因為提前或放後面都會造成畫面旋轉不順暢
    /// </summary>
    /// <returns></returns>
    public bool TryMove()
    {
        if (TryingMove)
        {

            if (TryingRush)
                terminalSpeed = speed * rushMultiply;
            else
                terminalSpeed = speed;
            var dir = Direction;
            direction.x = dir.x;
            direction.z = dir.y;
            direction = Controller.transform.TransformDirection(direction);
            if (TryingMovePerspective)
            {
                Move(direction, terminalSpeed);
                Perspective.Rotate(PerspectiveDirection);
                return true;
            }
            Move(direction, terminalSpeed);
            return true;
        }
        return false;  
    }

    public void Move(Vector3 direction, float speed)
    {
        Controller.Move(speed * Time.deltaTime * direction);
    }

    public void OpenRoomUI()
    {
        UIController.OpenUI();
        Perspective.LockCursor = false;
    }

    public void CloseRoomUI()
    {
        UIController.CloseUI();
        Perspective.LockCursor = true;
    }

    public void EndAction()
    {
        if(UIController != null && UIController.Use)
            CloseRoomUI();
    }
}