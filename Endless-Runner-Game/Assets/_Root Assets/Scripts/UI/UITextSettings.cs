using UnityEngine;

[CreateAssetMenu(fileName = "UITextSettings", menuName = "ScriptableObjects/UI/TextSettings", order = 1)]
public class UITextSettings : ScriptableObject
{
    public string currentScoreString = "Score: ";
    public string currentDistanceString = "Distance: ";
    public string currentPointsString = "Points: ";
    
    public string finalScoreString = "Final Score: ";
    public string finalDistanceString = "Distance covered: ";
    public string finalPointsString = "Points collected: ";

    public string highScoreString = "High Score: ";
}
