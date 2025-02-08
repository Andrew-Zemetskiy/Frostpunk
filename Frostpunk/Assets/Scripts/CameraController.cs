using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    private InputSystem _inputSystem;
    
    public float moveSpeed = 10f;
    public float zoomSpeed = 2f;
    public float minHeight = 10f;
    public float maxHeight = 30f;
    public float minAngle = 30f;
    public float maxAngle = 60f;

    private float currentHeight = 20f;
    private float currentAngle = 45f;

    private Vector3 moveDirection;
    private bool isMoving = false;

    private void Awake()
    {
        _inputSystem = new InputSystem();
    }

    private void OnEnable()
    {
        _inputSystem.Player.Move.performed += OnMove;
        _inputSystem.Player.Move.canceled += (context) => moveDirection = Vector2.zero;
        _inputSystem.Player.Scroll.performed += OnScroll;
        _inputSystem.Enable();
    }

    private void OnDisable()
    {
        _inputSystem.Disable();
        _inputSystem.Player.Move.performed -= OnMove;
        _inputSystem.Player.Scroll.performed -= OnScroll;
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        Vector2 moveInput = context.ReadValue<Vector2>();
        float horizontal = moveInput.x;
        float vertical = moveInput.y;
        
        moveDirection = new Vector3(horizontal, 0, vertical).normalized;
    }
    
    private void OnScroll(InputAction.CallbackContext context)
    {
        float scroll = context.ReadValue<float>();
        currentHeight -= scroll * zoomSpeed;
        currentHeight = Mathf.Clamp(currentHeight, minHeight, maxHeight);
        
        currentAngle = Mathf.Lerp(minAngle, maxAngle, Mathf.InverseLerp(minHeight, maxHeight, currentHeight));
        
        transform.position = new Vector3(transform.position.x, currentHeight, transform.position.z);
        transform.eulerAngles = new Vector3(currentAngle, transform.eulerAngles.y, 0);
    }
        
    void Update()
    {
        if (moveDirection != Vector3.zero)
        {
            transform.position += moveDirection * (moveSpeed * Time.deltaTime);
        }
    }
}