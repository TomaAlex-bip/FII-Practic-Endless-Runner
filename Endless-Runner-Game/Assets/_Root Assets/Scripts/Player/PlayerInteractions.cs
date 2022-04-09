using UnityEngine;

public class PlayerInteractions : MonoBehaviour
{
    [SerializeField] private Transform faceHitCheck;

    [SerializeField] private LayerMask faceHitLayerMask;

    private bool isGameOver;
    
    private void Update()
    {
        CheckFaceHit();
    }


    private void CheckFaceHit()
    {
        var hit = Physics.CheckSphere(faceHitCheck.position, 0.1f, faceHitLayerMask);
        if (hit && !isGameOver)
        {
            // TODO: GAME OVER!
            isGameOver = true;
            Debug.LogWarning("Game Over!");
            GameManager.Instance.GameOver();
        }
    }
}
