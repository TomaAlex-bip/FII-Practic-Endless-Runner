using System;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    public static PlayerAnimations Instance { get; private set; }

    [SerializeField] private float initialRunningAnimationSpeed = 1.75f;
    
    // private const string PLAYER_RUN = "Running";
    // private const string PLAYER_JUMP = "Jumping";
    // private const string PLAYER_SLIDE = "Sliding";
    // private const string PLAYER_FALL = "Fall";

    private Animator animator;

    private void Awake()
    {
        InitializeSingleton();
        animator = GetComponent<Animator>();
    }

    private void LateUpdate()
    {
        UpdateRunningAnimationSpeed();
    }

    // public void RunAnimation() => animator.Play(PLAYER_RUN);
    // public void JumpAnimation() => animator.Play(PLAYER_JUMP);
    // public void SlideAnimation() => animator.Play(PLAYER_SLIDE);
    // public void FallAnimation() => animator.Play(PLAYER_FALL);

    public void FallAnimation()
    {
        SetSlideBool(false);
        SetJumpBool(false);
        FallTrigger();
    }

    public void RunAnimation()
    {
        SetSlideBool(false);
        SetJumpBool(false);
    }

    public void JumpAnimation()
    {
        SetSlideBool(false);
        SetJumpBool(true);
    }

    public void SlideAnimation()
    {
        SetSlideBool(true);
        SetJumpBool(false);
    }

    private void SetJumpBool(bool value) => animator.SetBool("jump", value);
    private void SetSlideBool(bool value) => animator.SetBool("slide", value);
    private void FallTrigger() => animator.SetTrigger("fall");

    private void UpdateRunningAnimationSpeed()
    {
        var initialSpeed = PlayerMovement.Instance.InitialSpeed;
        var currentSpeed = PlayerMovement.Instance.CurrentSpeed;

        var currentRunningAnimationSpeed = (currentSpeed * initialRunningAnimationSpeed) / initialSpeed;
        animator.SetFloat("animationMultiplier", currentRunningAnimationSpeed);
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
