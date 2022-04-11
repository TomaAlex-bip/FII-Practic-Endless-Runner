using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public int Difficulty  => difficulty;
    public int MaxDifficulty => maxDifficulty;

    // TODO: make player a singleton
    [SerializeField] private GameObject player;

    [SerializeField] private float distanceToIncreaseDifficulty = 500f;
    [SerializeField] private int maxDifficulty = 6;
    [SerializeField] private int pointsMultiplier = 10;
    [SerializeField] private int distanceMultiplier = 1;

    private bool isGameRunning = true;
    
    private int score = 0;
    private int points = 0;
    private float distance = 0f;
    private int difficulty = 1;

    private void Awake()
    {
        InstantiateSingleton();

        InputManager.Instance.OnPauseInput += PauseGameSwitch;

        difficulty = 1;
    }

    private void LateUpdate()
    {
        GetPlayerDistance();
        CalculateScore();
        UIManager.Instance.SetCurrentStats(score, distance, points);
        
        UpdateDifficulty();
    }

    public void GameOver()
    {
        InputManager.Instance.DisablePlayerControls();
        InputManager.Instance.DisableUIControls();
        UIManager.Instance.GameOver();
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
        if (Difficulty > maxDifficulty)
        {
            difficulty = maxDifficulty;
            return;
        }

        difficulty = Mathf.FloorToInt((distanceToIncreaseDifficulty + distance) / distanceToIncreaseDifficulty);
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

    // TODO: move to a static class
    private void CalculateScore()
    {
        score = points * pointsMultiplier + Mathf.RoundToInt(distance * distanceMultiplier);
    }
    
}
