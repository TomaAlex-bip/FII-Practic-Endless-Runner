using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("Main Panels")]
    [SerializeField] private Transform pausePanel;
    [SerializeField] private Transform gameOverPanel;
    [SerializeField] private Transform hudPanel;

    [Header("Current Stats")]
    [SerializeField] private Text currentScoreText;
    [SerializeField] private Text currentDistanceText;
    [SerializeField] private Text currentPointsText;
    
    [Header("Final Stats")]
    [SerializeField] private Text finalScoreText;
    [SerializeField] private Text finalDistanceText;
    [SerializeField] private Text finalPointsText;
    [SerializeField] private Text highScoreText;

    [Header("Power-up Timers")] 
    [SerializeField] private Slider jumpBoostSlider;
    [SerializeField] private Slider invulnerabilitySlider;
    
    [Header("UI Text Settings")]
    [SerializeField] private UITextSettings uiTextSettings;

    [Header("Animations")] 
    [SerializeField] private float pauseAnimationSpeed = 50f;
    [SerializeField] private float gameOverAnimationSpeed = 30f;
    [SerializeField] private float outsidePosition = 2000;

    private void Awake()
    {
        InstatiateSingleton();
    }

    private void Start()
    {
        SetCurrentStats(0,0.0f,0);
        SetFinalStats(0,0.0f,0);
    }

    public void GameOver()
    {
        StartCoroutine(
            UIAnimations.PanelAnimationCoroutine(gameOverPanel, gameOverAnimationSpeed, outsidePosition));
        hudPanel.gameObject.SetActive(false);
        pausePanel.gameObject.SetActive(false);
        gameOverPanel.gameObject.SetActive(true);
    }

    public void PauseGame()
    {
        StartCoroutine(
            UIAnimations.PanelAnimationCoroutine(pausePanel, pauseAnimationSpeed, outsidePosition));
        pausePanel.gameObject.SetActive(true);
    }

    public void UnPauseGame()
    {
        pausePanel.gameObject.SetActive(false);
        StopCoroutine(
            UIAnimations.PanelAnimationCoroutine(pausePanel, pauseAnimationSpeed, outsidePosition));
    }

    public void SetCurrentStats(int score, float distance, int points)
    {
        currentScoreText.text = uiTextSettings.currentScoreString + score.ToString();
        currentDistanceText.text = $"{uiTextSettings.currentDistanceString}{distance:N1} m";
        currentPointsText.text = uiTextSettings.currentPointsString + points.ToString();
    }

    public void SetFinalStats(int score, float distance, int points)
    {
        finalScoreText.text = uiTextSettings.finalScoreString + score.ToString();
        finalDistanceText.text = $"{uiTextSettings.finalDistanceString}{distance:N2} m";
        finalPointsText.text = uiTextSettings.finalPointsString + points.ToString();

        var highScore = HighScoreSaveManager.GetHighScore();
        if (score > highScore)
        {
            HighScoreSaveManager.SaveHighScore(score);
            highScoreText.text = uiTextSettings.highScoreString + score;
        }
        else
        {
            highScoreText.text = uiTextSettings.highScoreString + highScore;
        }
    }

    public void SetJumpBoostSliderValue(float value)
    {
        jumpBoostSlider.value = value;
    }
    
    public void SetInvulnerabilitySliderValue(float value)
    {
        invulnerabilitySlider.value = value;
    }

    private void InstatiateSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }
    }



}
