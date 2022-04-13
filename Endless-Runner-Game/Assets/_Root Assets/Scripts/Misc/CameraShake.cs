using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class CameraShake: MonoBehaviour
{
    public static CameraShake Instance { get; private set; }

    [SerializeField] private CameraShakeData settings;
    
    private void Awake()
    {
        Instance = this;
    }

    public void Shake(CameraShakeType type)
    {
        var shakeTime = 0f;
        var shakeMagnitude = 0f;
        switch (type)
        {
            case CameraShakeType.Jump:
                shakeTime = settings.jumpShakeTime;
                shakeMagnitude = settings.jumpShakeMagnitude;
                break;
            
            case CameraShakeType.Obstacles:
                shakeTime = settings.obstaclesShakeTime;
                shakeMagnitude = settings.obstaclesShakeMagnitude;
                break;
            
            case CameraShakeType.Points:
                shakeTime = settings.pointsShakeTime;
                shakeMagnitude = settings.pointsShakeMagnitude;
                break;
            
            case CameraShakeType.Powerups:
                shakeTime = settings.powerupsShakeTime;
                shakeMagnitude = settings.powerupsShakeMagnitude;
                break;
        }
        StartCoroutine(ShakeCoroutine(shakeTime, shakeMagnitude));
    }

    private IEnumerator ShakeCoroutine(float shakeTime, float shakeMagnitude)
    {
        var originalPosition = transform.localPosition;
        
        var elapsed = 0.0f;
        while(elapsed < shakeTime)
        {
            var x = Random.Range(-1f, 1f) * shakeMagnitude;
            var y = Random.Range(-1f, 1f) * shakeMagnitude;
            var z = Random.Range(-1f, 1f) * shakeMagnitude;

            transform.localPosition = new Vector3(x, y, z);

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = originalPosition;
    }
    
}

public enum CameraShakeType
{
    Jump, Points, Powerups, Obstacles
}
