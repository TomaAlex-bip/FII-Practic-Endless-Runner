using System;
using UnityEngine;

public class PlayerInteractions : MonoBehaviour
{
    [SerializeField] private Transform faceHitCheck;
    [SerializeField] private Transform faceHitPivotNormal;
    [SerializeField] private Transform faceHitPivotCrouch;

    [SerializeField] private LayerMask faceHitLayerMask;

    private bool isGameOver;

    private PlayerMovement playerMovement;

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        UpdateFaceHitPosition();
        CheckFaceHit();
        if (!isGameOver)
        {
            UpdateAnimations();
        }
    }


    private void CheckFaceHit()
    {
        var hit = Physics.CheckSphere(faceHitCheck.position, 0.45f, faceHitLayerMask);
        if (hit && !isGameOver)
        {
            // TODO: GAME OVER!
            isGameOver = true;
            Debug.LogWarning("Game Over!");
            GameManager.Instance.GameOver();
            playerMovement.StopMovement();
            
            PlayerAnimations.Instance.FallAnimation();
        }
    }
    
    private void UpdateFaceHitPosition()
    {
        if (playerMovement.IsCrouched)
        {
            faceHitCheck.position = faceHitPivotCrouch.position;
        }
        else
        {
            faceHitCheck.position = faceHitPivotNormal.position;
        }
    }

    private void UpdateAnimations()
    {
        if (playerMovement.IsGrounded)
        {
            if (playerMovement.IsCrouched)
            {
                PlayerAnimations.Instance.SlideAnimation();
            }
            else
            {
                PlayerAnimations.Instance.RunAnimation();
            }
        }
        else
        {
            PlayerAnimations.Instance.JumpAnimation();
        }
    }
}
