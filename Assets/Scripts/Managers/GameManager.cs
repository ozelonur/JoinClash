using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using Sirenix.OdinInspector;
using Ozel;

public enum GameState
{
    Start,
    Playing,
    Complete,
    InBattle,
    Dead
}
public class GameManager : Singleton<GameManager>
{
    private CanvasManager canvasManager;

    public Action gameOver;
    public Action gameComplete;

    private GameState currentGameState;

    [SerializeField, ReadOnly] private List<EnemyController> enemyList;
    [SerializeField, ReadOnly] public int DiamondCount { get => PlayerPrefs.GetInt(Constants.DIAMOND, 0); set => PlayerPrefs.SetInt(Constants.DIAMOND, value); }

    public GameState CurrentGameState { get => currentGameState; set => currentGameState = value; }
    public List<EnemyController> EnemyList { get => enemyList; set => enemyList = value; }

    private void Start()
    {
        CurrentGameState = GameState.Start;

        canvasManager = CanvasManager.Instance;

        gameComplete += GameComplete;
        gameOver += GameOver;
    }

    public void OnClick()
    {
        if (CurrentGameState == GameState.Start)
        {
            CurrentGameState = GameState.Playing;
            canvasManager.TapPanel.SetActive(false);
            
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    private void GameComplete()
    {
        CurrentGameState = GameState.Complete;
        LevelManager.Instance.levelIndex++;
    }

    private void GameOver()
    {
        CurrentGameState = GameState.Dead;
    }
}
