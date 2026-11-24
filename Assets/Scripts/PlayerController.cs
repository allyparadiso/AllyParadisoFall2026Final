using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    //movement
    [SerializeField] private float acceleration = 12f;
    [SerializeField] private float maxSpeed = 16f;
    [SerializeField] private float rollingFriction = 2.5f;
    [SerializeField] private float turnSpeed = 110f;
    [SerializeField] private float deceleration = 15f;
    private float moveSpeed = 5f;
    private float gravity = -18f;

    //rotation
    public Transform cameraTarget;
    public Camera mainCamera;
    public Transform cameraPivot;
    private float lookSensitivity = 0.1f;
    private float topClamp = 85f;
    private float bottomClamp = -85f;

    //inputs
    private CharacterController _controller;
    //private Rigidbody rb;
    private Vector2 _moveInput;
    private Vector2 _lookInput;

    private float _currentSpeed;
    private float _cinemachineTargetPitch;
    private float _verticalVelocity;
    private bool isGrounded;

    private void Awake()
    {
        //rb = GetComponent<Rigidbody>();
        _controller = GetComponent<CharacterController>();
    }

    private void Start()
    {
        //isGrounded = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        Movement();
    }

    private void LateUpdate()
    {
        CameraRotation();
    }

    // void OCollisionEnter(GameObject other)
    // {
    //    if (other.CompareTag("Ground"))
    //     {
    //         isGrounded = true;
    //     } 
    // }

    // void OCollisionExit(GameObject other)
    // {
    //    if (other.CompareTag("Ground"))
    //     {
    //         isGrounded = false;
    //     } 
    // }

    public void OnMove(InputAction.CallbackContext context)
    {
        _moveInput = context.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        _lookInput = context.ReadValue<Vector2>();
    }

    private void Movement()
    {
        if (isGrounded && _verticalVelocity < 0)
        {
            _verticalVelocity = -2;
        }
        _verticalVelocity += gravity * Time.deltaTime;

        if (isGrounded) //new stuff
        {
            if (_moveInput.y > 0.05f)
            {
                _currentSpeed += _moveInput.y * acceleration * Time.deltaTime;
            }
            else if (_moveInput.y < -0.05f)
            {
                _currentSpeed = Mathf.MoveTowards(_currentSpeed, 0, deceleration * Time.deltaTime);
            }
            else
            {
                _currentSpeed = Mathf.MoveTowards(_currentSpeed, 0f, rollingFriction * Time.deltaTime);
            }
        }

        _currentSpeed = Mathf.Clamp(_currentSpeed, 0, maxSpeed);

        if (Mathf.Abs(_moveInput.x) > 0.05f)
        {
            float activeTurnSpeed = isGrounded ? turnSpeed : turnSpeed * 0.7f;

            float moveFactor = isGrounded ? Mathf.Clamp01(_currentSpeed / 2f) : 1f;

            float turnAmount = _moveInput.x * activeTurnSpeed * moveFactor * Time.deltaTime;
            transform.Rotate(0f, turnAmount, 0f);
        }

        Vector3 moveVelocity = transform.forward * _currentSpeed;
        moveVelocity.y = _verticalVelocity;

        // rb.AddForce(moveVelocity * Time.deltaTime, ForceMode.Acceleration);

        Vector3 forwardVector = mainCamera.transform.forward;
        forwardVector.y = 0;

        Vector3 rightVector = mainCamera.transform.right;
        //rightVector.y = 0;
        Vector3 moveDirection = forwardVector * _moveInput.y + rightVector * _moveInput.x;

        moveDirection.y = _verticalVelocity;

        _controller.Move(moveDirection * moveSpeed * Time.deltaTime);
    }

    public void CameraRotation()
    {
        if (_lookInput.sqrMagnitude >= .01f)
        {
            //transform.Rotate(0, _lookInput.x, 0);
            _cinemachineTargetPitch -= _lookInput.y * lookSensitivity;
            _cinemachineTargetPitch = Mathf.Clamp(_cinemachineTargetPitch, bottomClamp, topClamp);
            cameraPivot.localRotation = Quaternion.Euler(_cinemachineTargetPitch, 0f, 0f);

            mainCamera.transform.localRotation = Quaternion.Euler(_cinemachineTargetPitch, 0f, 0f);
            transform.Rotate(Vector3.up * (_lookInput.x * lookSensitivity));
        }
    }
}
