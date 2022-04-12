using System;
using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement Instance { get; private set; }

    public bool IsGrounded => isGrounded;
    public bool IsCrouched => isCrouched;
    
    private const float GRAVITY = -9.81f;
    
    [SerializeField] private float horizontalMovementSpeed = 20f;
    [SerializeField] private float forwardMovementSpeed = 10f;
    [SerializeField] private float forwardMovementSpeedCap = 50f;
    [SerializeField] private float movementSpeedIncrease = 0.05f;
    [SerializeField] private float jumpPower = 3f;
    [SerializeField] private float jumpPowerMultiplier = 2f;
    [SerializeField] private float crouchTime = 3f;
    [SerializeField] private float crouchHeight = 0.5f;
    [SerializeField] private float rotationAngle = 10f;
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private Transform mesh;
    
    [SerializeField] private float gravityMultiplier = 2f;
    
    [SerializeField] private Transform groundCheck;

    [SerializeField] private LayerMask terrainLayerMask;
    
    private float gravity;
    private float originalStepOffset;
    private float originalHeight;
    private float jumpHeight;
    private Vector3 velocity;
    private float horizontalMovement;
    private bool isGrounded;
    private bool isCrouched;

    private CharacterController characterController;

    private InputManager inputManager;
    
    private void Awake()
    {
        InitializeSingleton();
        
        inputManager = InputManager.Instance;

        inputManager.OnJumpInput += Jump;
        inputManager.OnCrouchInput += Crouch;
        
        characterController = GetComponent<CharacterController>();
        originalStepOffset = characterController.stepOffset;
        originalHeight = characterController.height;

    }

    private void Start()
    {
        jumpHeight = jumpPower;
        UpdateGravity();
    }

    private void Update()
    {
        UpdateMovementSpeed();
        GetHorizontalInput();
        UpdateGravity();
        CheckGrounding();
        UpdateJump();
        UpdateCrouch();
        MovePlayer();
        RotatePlayer();
    }

    public void StopMovement()
    {
        forwardMovementSpeed = 0f;
    }

    private void GetHorizontalInput()
    {
        horizontalMovement = inputManager.HorizontalMovement;
    }

    // move the player forward and sideways
    private void MovePlayer()
    {
        var playerTransform = transform;
        var move = playerTransform.forward * forwardMovementSpeed + 
                   playerTransform.right * horizontalMovement * horizontalMovementSpeed;
        
        characterController.Move(move * Time.deltaTime);
    }

    private void RotatePlayer()
    {
        var desireRotation = Quaternion.Euler(Vector3.up * horizontalMovement * rotationAngle);
        mesh.transform.rotation = Quaternion.Lerp(mesh.transform.rotation, desireRotation, rotationSpeed * Time.deltaTime);
    }

    // apply a jump impulse to the player velocity
    private void Jump()
    {
        if (!isGrounded)
            return;
        
        velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        characterController.stepOffset = 0f;
    }

    // update the velocity on the Y axis after the initial jump impulse
    private void UpdateJump()
    {
        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
    }
    
    private void Crouch()
    {
        if (isCrouched)
            return;

        isCrouched = true;
        StartCoroutine(StayInCrouchCoroutine());
    }

    // change the character controller height if it is crouching
    private void UpdateCrouch()
    {
        if (isCrouched && Math.Abs(characterController.height - crouchHeight) > 0.01f)
        {
            characterController.center = Vector3.down * crouchHeight;
            characterController.height = crouchHeight;
        }
        else if (!isCrouched && Math.Abs(characterController.height - originalHeight) > 0.01f)
        {
            characterController.center = Vector3.zero;
            characterController.height = originalHeight;
        }
    }
    
    // check if the character is on ground, and if it is, set it's y velocity to
    // a small negative number, so it doesn't levitate
    private void CheckGrounding()
    {
        // TODO: change to a cube ???
        isGrounded = Physics.CheckSphere(groundCheck.position, 0.3f, terrainLayerMask);
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -1f;
            characterController.stepOffset = originalStepOffset;
        }
    }

    private void UpdateMovementSpeed()
    {
        if (forwardMovementSpeed > forwardMovementSpeedCap)
            return;

        forwardMovementSpeed += movementSpeedIncrease * Time.deltaTime;
    }

    private void UpdateGravity() => gravity = GRAVITY * gravityMultiplier;

    private IEnumerator IncreaseMovementSpeedCoroutine()
    {
        while (forwardMovementSpeed < forwardMovementSpeedCap)
        {
            forwardMovementSpeed += movementSpeedIncrease * Time.deltaTime;
            yield return null;
        }
    }
    
    [ContextMenu("Activate Jump Boost")]
    public IEnumerator JumpBoostCoroutine(float timer)
    {
        jumpHeight = jumpPower * jumpPowerMultiplier;
        
        var time = 0f;
        while (time <= timer)
        {
            time += Time.deltaTime;
            yield return null;
        }

        jumpHeight = jumpPower;
    }

    private IEnumerator StayInCrouchCoroutine()
    {
        yield return new WaitForSeconds(crouchTime);
        isCrouched = false;
    }

    private void InitializeSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }
    }

}
