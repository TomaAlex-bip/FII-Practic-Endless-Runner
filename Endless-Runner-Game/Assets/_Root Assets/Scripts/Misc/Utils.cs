using UnityEngine;

public class Utils
{
    public static int CalculateScore(int points, int pointsMultiplier, float distance, float distanceMultiplier)
    {
        return points * pointsMultiplier + Mathf.RoundToInt(distance * distanceMultiplier);
    }
}
