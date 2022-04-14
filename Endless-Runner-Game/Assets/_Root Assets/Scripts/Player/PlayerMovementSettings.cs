using UnityEngine;

[CreateAssetMenu(fileName = "PlayerMovementSettings", menuName = "ScriptableObjects/PlayerMovement", order = 0)]
public class PlayerMovementSettings : ScriptableObject
{
    public float horizontalMovementSpeed = 7.5f;
    public float forwardMovementSpeed = 10f;
    public float forwardMovementSpeedCap = 20f;
    public float movementSpeedIncrease = 0.03f;
    public float jumpPower = 3f;
    public float jumpPowerMultiplier = 3f;
    public float crouchTime = 1f;
    public float crouchHeight = 0.5f;
    public float rotationAngle = 30f;
    public float rotationSpeed = 30f;
    public float gravityMultiplier = 2f;
    public float stepSoundTimer = 0.32f;
}
