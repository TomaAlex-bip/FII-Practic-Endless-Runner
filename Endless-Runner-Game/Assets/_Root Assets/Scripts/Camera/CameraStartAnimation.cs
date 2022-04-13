using System.Collections;
using UnityEngine;

public class CameraStartAnimation : MonoBehaviour
{
    [SerializeField] private float speed = 1f;
    [SerializeField] private Transform cameraStartPivot;
    [SerializeField] private Transform cameraEndPivot;

    private void Awake()
    {
        InputManager.Instance.DisablePlayerControls();
        StartCoroutine(StartAnimationCoroutine());
    }

    private IEnumerator StartAnimationCoroutine()
    {
        transform.position = cameraStartPivot.position;
        while (Mathf.Abs(Vector3.Distance(transform.position, cameraEndPivot.position)) >= 0.5f )
        {
            transform.position = Vector3.Lerp(
                transform.position, 
                cameraEndPivot.position, 
                speed * Time.deltaTime);
            yield return null;
        }
        
        InputManager.Instance.EnablePlayerControls();
    }
}
