using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum UIState
{
    Home,
    Game,
    GameOver,
    Null,
}
public class UIManager : MonoBehaviour
{
    private HomeUI homeUI;
    private GameUI gameUI;
    private GameOverUI gameOverUI;
    public UIState currentState;
    
    private void Awake()
    {
        homeUI = GetComponentInChildren<HomeUI>(true);
        homeUI?.Init(this);
        gameUI = GetComponentInChildren<GameUI>(true);
        gameUI?.Init(this);
        gameOverUI = GetComponentInChildren<GameOverUI>(true);
        gameOverUI?.Init(this);
        
        //ChangeState(UIState.Home);
    }
    
    public void SetPlayGame()
    {
        ChangeState(UIState.Null);
        SceneManager.LoadScene("GameScene");
    }

    public void EntranceDungeon()
    {
        ChangeState(UIState.Game);
        SceneManager.LoadScene("DungeonScene");
    }
    
    public void SetGameOver()
    {
        gameOverUI.BestScoreText.text = GameManager.instance.BestScore.ToString();
        gameOverUI.CurrentScoreText.text = GameManager.instance.CurrentScore.ToString();
        ChangeState(UIState.GameOver);
    }
    
    public void ChangeWave(int waveIndex)
    {
        gameUI?.UpdateWaveText(waveIndex);
    }

    public void ChangePlayerHP(int currentHP, int maxHP)
    {
        //gameUI?.UpdateHPSlider(currentHP/ maxHP);
    }
    
    public void ChangeState(UIState state)
    {
        currentState = state;
        homeUI?.SetActive(currentState);
        gameUI?.SetActive(currentState);
        gameOverUI?.SetActive(currentState);
    }
}
