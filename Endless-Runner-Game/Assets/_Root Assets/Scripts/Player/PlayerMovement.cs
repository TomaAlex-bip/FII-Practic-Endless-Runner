using System;
using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement Instance { get; private set; }

    public bool IsGrounded => isGrounded;
    public bool IsCrouched => isCrouched;
    
    //TODO: verify for broken forward speed
    [SerializeField] private PlayerMovementSettings settings;
    
    [SerializeField] private Transform mesh;
    [SerializeField] private Transform sparks;
    [SerializeField] private Transform groundCheck;

    [SerializeField] private LayerMask terrainLayerMask;
    
    private const float GRAVITY = -9.81f;
    
    private float gravity;
    private float originalStepOffset;
    private float originalHeight;
    private float jumpHeight;
    private Vector3 velocity;
    private float horizontalMovement;
    private float currentForwardSpeed;
    
    
    private bool isGrounded;
    private bool isCrouched;
    private bool shakedOnGround;

    private float jumpBoostTime;

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

        currentForwardSpeed = settings.forwardMovementSpeed;
    }

    private void Start()
    {
        jumpHeight = settings.jumpPower;
        UpdateGravity();
        StartCoroutine(StepSoundCoroutine());
    }

    private void Update()
    {
        UpdateMovementSpeed();
        GetHorizontalInput();
        UpdateGravity();
        CheckGrounding();
        ShakeOnGround();
        UpdateJump();
        UpdateCrouch();
        MovePlayer();
        RotatePlayer();
    }

    public void ResetJumpBoostCoroutine() => jumpBoostTime = 0f;

    public float GetJumpBoostTime() => jumpBoostTime;

    public void StopMovement()
    {
        currentForwardSpeed = 0f;
    }

    private void GetHorizontalInput()
    {
        horizontalMovement = inputManager.HorizontalMovement;
    }

    // move the player forward and sideways
    private void MovePlayer()
    {
        var playerTransform = transform;
        var move = playerTransform.forward * currentForwardSpeed + 
                   playerTransform.right * horizontalMovement * settings.horizontalMovementSpeed;
        
        characterController.Move(move * Time.deltaTime);
    }

    private void ShakeOnGround()
    {
        if (isGrounded && !shakedOnGround)
        {
            AudioManager.Instance.PlaySound(AudioManager.FALL_SOUND);
            CameraShake.Instance.Shake(CameraShakeType.Jump);
            shakedOnGround = true;
        }

        if (!isGrounded)
        {
            shakedOnGround = false;
        }
    }

    private void RotatePlayer()
    {
        var desireRotation = Quaternion.Euler(Vector3.up * horizontalMovement * settings.rotationAngle);
        
        mesh.transform.rotation = Quaternion.Lerp(
            mesh.transform.rotation, desireRotation, settings.rotationSpeed * Time.deltaTime);
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
        if (isCrouched && Math.Abs(characterController.height - settings.crouchHeight) > 0.01f)
        {
            characterController.center = Vector3.down * settings.crouchHeight;
            characterController.height = settings.crouchHeight;
            if (!sparks.gameObject.activeInHierarchy)
            {
                sparks.gameObject.SetActive(true);
            }
        }
        else if (!isCrouched && Math.Abs(characterController.height - originalHeight) > 0.01f)
        {
            characterController.center = Vector3.zero;
            characterController.height = originalHeight;
            if (sparks.gameObject.activeInHierarchy)
            {
                sparks.gameObject.SetActive(false);
            }
        }
    }
    
    // check if the character is on ground, and if it is, set it's y velocity to
    // a small negative number, so it doesn't levitate
    private void CheckGrounding()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, 0.3f, terrainLayerMask);
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -1f;
            characterController.stepOffset = originalStepOffset;
        }
    }

    private void UpdateMovementSpeed()
    {
        if (currentForwardSpeed > settings.forwardMovementSpeedCap)
            return;

        currentForwardSpeed += settings.movementSpeedIncrease * Time.deltaTime;
    }

    private void UpdateGravity() => gravity = GRAVITY * settings.gravityMultiplier;

    [ContextMenu("Activate Jump Boost")]
    public IEnumerator JumpBoostCoroutine(float timer)
    {
        jumpHeight = settings.jumpPower * settings.jumpPowerMultiplier;
        UIManager.Instance.SetJumpBoostSliderValue(1f);
        
        jumpBoostTime = 0f;
        while (jumpBoostTime <= timer)
        {
            jumpBoostTime += Time.deltaTime;
            var sliderValue = 1f - jumpBoostTime / timer;
            UIManager.Instance.SetJumpBoostSliderValue(sliderValue);
            yield return null;
        }

        jumpHeight = settings.jumpPower;
        UIManager.Instance.SetJumpBoostSliderValue(0f);
        jumpBoostTime = 0;
    }

    private IEnumerator StayInCrouchCoroutine()
    {
        yield return new WaitForSeconds(settings.crouchTime);
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

    private IEnumerator StepSoundCoroutine()
    {
        while (true)
        {
            if (isGrounded)
            {
                AudioManager.Instance.PlayRandomStepSound();
            }
            yield return new WaitForSeconds(1/(settings.stepSoundTimer * currentForwardSpeed));
        }
    }

}
