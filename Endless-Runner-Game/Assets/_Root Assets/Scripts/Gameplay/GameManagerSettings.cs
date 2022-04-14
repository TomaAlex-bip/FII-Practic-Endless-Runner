using UnityEngine;

[CreateAssetMenu(fileName = "GameManagerSettings", menuName = "ScriptableObjects/GameManager", order = 1)]
public class GameManagerSettings : ScriptableObject
{
    public float distanceToIncreaseDifficulty = 500f;
    public int maxDifficulty = 6;
    public int pointsMultiplier = 10;
    public int distanceMultiplier = 1;
}
