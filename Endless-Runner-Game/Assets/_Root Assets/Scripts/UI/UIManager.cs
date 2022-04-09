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

    [Header("UI Text Settings")]
    [SerializeField] private UITextSettings uiTextSettings;

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
        hudPanel.gameObject.SetActive(false);
        pausePanel.gameObject.SetActive(false);
        gameOverPanel.gameObject.SetActive(true);
    }

    public void PauseGame()
    {
        pausePanel.gameObject.SetActive(true);
    }

    public void UnPauseGame()
    {
        pausePanel.gameObject.SetActive(false);
    }

    public void SetCurrentStats(int score, float distance, int points)
    {
        currentScoreText.text = uiTextSettings.currentScoreString + score.ToString();
        currentDistanceText.text = uiTextSettings.currentDistanceString + distance.ToString("N2");
        currentPointsText.text = uiTextSettings.currentPointsString + points.ToString();
    }
    
    public void SetFinalStats(int score, float distance, int points)
    {
        finalScoreText.text = uiTextSettings.finalScoreString + score.ToString();
        finalDistanceText.text = uiTextSettings.finalDistanceString + distance.ToString("N2");
        finalPointsText.text = uiTextSettings.finalPointsString + points.ToString();
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
