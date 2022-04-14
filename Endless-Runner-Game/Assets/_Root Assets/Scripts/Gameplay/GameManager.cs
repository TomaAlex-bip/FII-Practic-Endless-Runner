using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public int Difficulty  => difficulty;
    public int MaxDifficulty => settings.maxDifficulty;

    [SerializeField] private GameManagerSettings settings;

    private bool isGameRunning = true;
    
    private int score = 0;
    private int points = 0;
    private float distance = 0f;
    private int difficulty = 1;

    private GameObject player;
    
    private void Awake()
    {
        InstantiateSingleton();

        InputManager.Instance.OnPauseInput += PauseGameSwitch;

        difficulty = 1;
    }

    private void Start()
    {
        player = PlayerMovement.Instance.gameObject;
    }

    private void LateUpdate()
    {
        GetPlayerDistance();
        score = Utils.CalculateScore(points, settings.pointsMultiplier, distance, settings.distanceMultiplier);
        UIManager.Instance.SetCurrentStats(score, distance, points);
        UpdateDifficulty();
    }

    public void GameOver()
    {
        InputManager.Instance.DisablePlayerControls();
        InputManager.Instance.DisableUIControls();
        UIManager.Instance.GameOver();
        UIManager.Instance.SetFinalStats(score, distance, points);
        // TODO: add game over sound
        AudioManager.Instance.PlaySound(AudioManager.FALL_SOUND);
    }

    public void PauseGameSwitch()
    {
        if (isGameRunning)
        {
            isGameRunning = false;
            InputManager.Instance.DisablePlayerControls();
            UIManager.Instance.PauseGame();
            Time.timeScale = 0;
        }
        else
        {
            isGameRunning = true;
            InputManager.Instance.EnablePlayerControls();
            UIManager.Instance.UnPauseGame();
            Time.timeScale = 1;
        }
    }

    public void LoadMainMenuScene()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

    public void RestartGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(1);
    }

    public void AddPoint()
    {
        points++;
    }

    private void GetPlayerDistance()
    {
        var rawDistance = player.transform.position.z;
        if (rawDistance > 0)
        {
            distance = rawDistance;
        }
        else
        {
            distance = 0;
        }
    }

    private void UpdateDifficulty()
    {
        if (Difficulty > settings.maxDifficulty)
        {
            difficulty = settings.maxDifficulty;
            return;
        }

        difficulty = Mathf.FloorToInt(
            (settings.distanceToIncreaseDifficulty + distance) / settings.distanceToIncreaseDifficulty);
    }

    private void InstantiateSingleton()
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
