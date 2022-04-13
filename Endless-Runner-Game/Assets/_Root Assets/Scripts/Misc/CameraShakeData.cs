using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CameraShakeSettings", menuName = "ScriptableObjects/CameraShake", order = 1)]
public class CameraShakeData : ScriptableObject
{
    public float jumpShakeMagnitude;
    public float jumpShakeTime;
    public float pointsShakeMagnitude;
    public float pointsShakeTime;
    public float powerupsShakeMagnitude;
    public float powerupsShakeTime;
    public float obstaclesShakeMagnitude;
    public float obstaclesShakeTime;
}
