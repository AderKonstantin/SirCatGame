using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private CharacterController _characterController;
    private PlayerInputActions _playerInputActions;
    
    // == Ground Check ==================================
    [SerializeField] private Transform _groundCheckPos;
    [SerializeField] private LayerMask _groundLayerMask;
    [SerializeField] private float _groundCheckRadius;
    
    private Vector3 _playerVelocity;
    [SerializeField] private bool _groundedPlayer;
    
    [SerializeField] private float _moveSpeed = 3.0f;
    [SerializeField] private float _runSpeed = 5.0f;
    [SerializeField] private float _gravityValue = -9.81f;
    [SerializeField] private float _jumpHeight = 3.0f;
    //
    // private int _healthPoints = 3;
    
    
    
    private void Awake()
    {
        _playerInputActions = new PlayerInputActions();
        _playerInputActions.PlayerActionMap.Enable();
    }

    // Start is called before the first frame update
    void Start()
    {
        _characterController = GetComponent<CharacterController>();
        // _rigidBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
        _groundedPlayer = GroundCheck();
        
        // Attack();
        // Crouch();
        
        Move();
        Gravity();
        Jump();
    }

    private void FixedUpdate()
    {
        //
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(_groundCheckPos.position, _groundCheckRadius);
    }

    private void Movement(float speedValue)
    {
        Vector2 inputVector = _playerInputActions.PlayerActionMap.Move.ReadValue<Vector2>();
        Vector3 moveDirection = new Vector3(inputVector.x, 0, 0);
        if (inputVector != Vector2.zero)
        {
            // Debug.Log(_playerInputActions.PlayerActionMap.Move.ReadValue<Vector2>());
            // _rigidBody.velocity = new Vector3( inputVector.x * (speedValue * Time.deltaTime), 0, 0);
            // _rigidBody.MovePosition(transform.position + new Vector3( inputVector.x * Time.deltaTime * speedValue, 0, 0) );
            _characterController.Move(moveDirection * (speedValue * Time.deltaTime) );
        }
        else
        {
            // _rigidBody.velocity = Vector3.zero;
        }
    }
    private void Move()
    {
        if (_playerInputActions.PlayerActionMap.Run.IsInProgress())
        {
            if (_playerInputActions.PlayerActionMap.Run.WasPressedThisFrame())
                Debug.Log("Running");
            Movement(_runSpeed);
        }
        else
        {
            Movement(_moveSpeed);
        }
    }
    private void Attack()
    {
        if (_playerInputActions.PlayerActionMap.Attack.WasPerformedThisFrame())
        {
            Debug.Log("attack");
        }
    }
    private void Jump()
    {
        if (_playerInputActions.PlayerActionMap.Jump.WasPerformedThisFrame() && _groundedPlayer)
        {
            Debug.Log("Jump");
            _playerVelocity.y += Mathf.Sqrt(_jumpHeight * -3.0f * _gravityValue);
            // _rigidBody.velocity = new Vector3(0, _jumpHeight, 0);
        }
        // Changes the height position of the player..
        // if (Input.GetButtonDown("Jump") && groundedPlayer)
        // {
        //     playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        // }
    }
    private void Gravity()
    {
        if (_groundedPlayer && _playerVelocity.y < 0)
        {
            _playerVelocity.y = 0f;
        }
        _playerVelocity.y += _gravityValue * Time.deltaTime;
        _characterController.Move(_playerVelocity * Time.deltaTime);
    }

    private bool GroundCheck()
    {
        return Physics.CheckSphere(_groundCheckPos.position, _groundCheckRadius, _groundLayerMask);
    }
    
    private void Crouch()
    {
        if (_playerInputActions.PlayerActionMap.Crouch.IsInProgress() && _groundedPlayer)
        {
            Debug.Log("Crouch");
        }
    }
}
