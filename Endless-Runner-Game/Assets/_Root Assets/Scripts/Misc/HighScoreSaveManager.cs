using UnityEngine;

public static class HighScoreSaveManager
{
    public static void SaveHighScore(int score) => PlayerPrefs.SetInt("score", score);
    
    public static int GetHighScore() => PlayerPrefs.HasKey("score") ? PlayerPrefs.GetInt("score") : 0;
}
