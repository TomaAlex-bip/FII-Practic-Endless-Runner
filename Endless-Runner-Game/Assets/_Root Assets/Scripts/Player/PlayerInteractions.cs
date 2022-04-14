using System.Collections;
using UnityEngine;

public class PlayerInteractions : MonoBehaviour
{
    [SerializeField] private Transform faceHitCheck;
    [SerializeField] private Transform faceHitPivotNormal;
    [SerializeField] private Transform faceHitPivotCrouch;

    [SerializeField] private LayerMask faceHitGroundLayerMask;
    [SerializeField] private LayerMask faceHitObstaclesLayerMask;

    [SerializeField] private float jumpBoostTimer = 5f;
    [SerializeField] private float invulnerabilityTimer = 10f;
    [SerializeField] private GameObject shield;
    
    [SerializeField] private bool invincible;
    private bool isGameOver;

    private float invulnerabilityTime;

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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Point"))
        {
            AudioManager.Instance.PlaySound(AudioManager.POINT_COLLECT);
            GameManager.Instance.AddPoint();
            other.gameObject.SetActive(false);
            ParticlesManager.Instance.InstantiatePointsParticles(other.transform.position);
            CameraShake.Instance.Shake(CameraShakeType.Points);
        }

        if (other.CompareTag("JumpBoost"))
        {
            // print("jump boost");
            AudioManager.Instance.PlaySound(AudioManager.POWERUP_COLLECT);
            if (playerMovement.GetJumpBoostTime() > 0f)
            {
                playerMovement.ResetJumpBoostCoroutine();
            }
            else
            {
                StartCoroutine(playerMovement.JumpBoostCoroutine(jumpBoostTimer));
            }
            other.gameObject.SetActive(false);
            ParticlesManager.Instance.InstantiateJumpBoostParticles(other.transform.position);
            CameraShake.Instance.Shake(CameraShakeType.Powerups);
        }

        if (other.CompareTag("Invulnerability"))
        {
            // print("de aia buna");
            AudioManager.Instance.PlaySound(AudioManager.POWERUP_COLLECT);
            if (invulnerabilityTime > 0f)
            {
                invulnerabilityTime = 0f;
            }
            else
            {
                StartCoroutine(InvulnerabilityCoroutine(invulnerabilityTimer));
            }
            other.gameObject.SetActive(false);
            ParticlesManager.Instance.InstantiateInvulnerabilityParticles(other.transform.position);
            CameraShake.Instance.Shake(CameraShakeType.Powerups);
        }
    }


    private void CheckFaceHit()
    {
        var hitGround = Physics.CheckSphere(faceHitCheck.position, 0.45f, faceHitGroundLayerMask);
        var hitObstacles = Physics.CheckSphere(faceHitCheck.position, 0.45f, faceHitObstaclesLayerMask);
        
        if ((hitGround || (hitObstacles && !invincible)) && !isGameOver)
        {
            isGameOver = true;
            Debug.LogWarning("Game Over!");
            GameManager.Instance.GameOver();
            playerMovement.StopMovement();
            
            PlayerAnimations.Instance.FallAnimation();
        }

        if (hitObstacles && invincible)
        {
            CameraShake.Instance.Shake(CameraShakeType.Obstacles);
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
    
    private IEnumerator InvulnerabilityCoroutine(float timer)
    {
        invincible = true;
        shield.SetActive(true);
        UIManager.Instance.SetInvulnerabilitySliderValue(1f);
        
        invulnerabilityTime = 0f;
        while (invulnerabilityTime <= timer)
        {
            invulnerabilityTime += Time.deltaTime;

            var sliderValue = 1f - invulnerabilityTime / invulnerabilityTimer;
            UIManager.Instance.SetInvulnerabilitySliderValue(sliderValue);
            
            yield return null;
        }

        invincible = false;
        shield.SetActive(false);
        UIManager.Instance.SetInvulnerabilitySliderValue(0f);
        invulnerabilityTime = 0;
    }
    
}
