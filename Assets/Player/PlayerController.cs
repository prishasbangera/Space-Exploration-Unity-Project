using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Source: https://www.youtube.com/watch?v=v_ncMFEoHTg
/// https://www.youtube.com/watch?v=dlN_ZOVZs9M
/// </summary>
[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public float walkSpeed = 5.0f;
    public float gravity = 20f;
    public float jumpForce = 8.0f;

    public float lookSensitivity = 0.4f;
    public float maxLookAngle = 85f;
    public bool lookY = true;

    // private

    
    private CharacterController controller;
    private Rigidbody rb;
    private Vector2 moveInput;
    //private InputAction jumpInput;
    private Vector3 moveDirection = Vector3.zero;
    private bool jumpPressed = false;


    private Camera mainCamera;
    private float lookAngle = 0.0f;



    //private Vector2 moveInput;
    //private float verticalVelocity;
   

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        mainCamera = GetComponentInChildren<Camera>();
        rb = GetComponentInChildren<Rigidbody>();

        //moveInput = InputSystem.actions.FindAction("Move");
        //jumpInput = InputSystem.actions.FindAction("Jump");
        //jumpInput.started += OnJump;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

    }

    private void Update()
    {
        HandleMovement(moveInput);
        

        // Mouse
        //Vector2 mouseDelta = new Vector2(Mouse.current.delta.x.ReadValue(), Mouse.current.delta.y.ReadValue());
        Vector2 mouseDelta = Mouse.current.delta.ReadValue();
        HandleLooking(mouseDelta);
    }

    private void HandleMovement(Vector2 moveVector)
    {
        bool isGrounded = controller.isGrounded;

        if (!isGrounded)
        {
            jumpPressed = false;
        }

        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        float oldY = moveDirection.y;
        Vector2 newSpeed = new Vector2(moveVector.x * walkSpeed, moveVector.y * walkSpeed);
        moveDirection = (forward * newSpeed.y) + (right * newSpeed.x);
        moveDirection.y = (jumpPressed && isGrounded) ? jumpForce : oldY;

        if (!isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }

        controller.Move(moveDirection * Time.deltaTime);
        //rb.AddForce(moveDirection);


    }


    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    public void OnJump(InputValue value)
    {
        jumpPressed = value.isPressed;
    }

    private void HandleLooking(Vector2 mouseDelta)
    {
        if (lookY)
        {
            lookAngle += -mouseDelta.y * lookSensitivity;
            lookAngle = Mathf.Clamp(lookAngle, -maxLookAngle, maxLookAngle);
            mainCamera.transform.localRotation = Quaternion.Euler(lookAngle, 0, 0);
        }

        transform.rotation *= Quaternion.Euler(0, mouseDelta.x * lookSensitivity, 0);
    }

}
