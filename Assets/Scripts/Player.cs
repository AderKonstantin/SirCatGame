using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private CharacterController _characterController;
    private PlayerInputActions _playerInputActions;
    
    // == Ground Check ==================================
    [SerializeField] private Transform _groundCheckPos;
    [SerializeField] private LayerMask _groundLayerMask;
    [SerializeField] private float _groundCheckRadius;
    private bool _groundedPlayer;

    private bool _facingRight = true;
    
    private Vector3 _playerVelocity;
    
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
        TurnBack();
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
            _playerVelocity.y += Mathf.Sqrt(_jumpHeight * -3.0f * _gravityValue);
        }
    }
    private void Gravity()
    {
        if (_playerVelocity.y < 0 && _groundedPlayer)
        {
            // _playerVelocity.y -= _playerVelocity.y;
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

    private void TurnBack()
    {
        Vector2 inputDirection = _playerInputActions.PlayerActionMap.Move.ReadValue<Vector2>();
        
        if (inputDirection.x < 0 && _facingRight ) // Left is minus
        {
            Flip();
        }
        else if (inputDirection.x > 0 && !_facingRight) // Right is plus
        {
            Flip();
        }
    }

    private void Flip()
    {
        if (_facingRight)
        {
            transform.localRotation = Quaternion.Euler(0,0,0);
        }
        else if (!_facingRight)
        {
            transform.localRotation = Quaternion.Euler(0,180.0f,0);
        }
        _facingRight = !_facingRight;
        Debug.Log("Flipped");
    }
}
