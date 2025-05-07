using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    
    [CanBeNull] public PlayerController Player { get; private set; }
    private ResourceController _playerResourceController;

    [SerializeField] private int currentWaveIndex = 0;

    public EnemyManager EnemyManager { get; private set; }
    
    public UIManager UIManager { get; private set; }

    private int bestScore = 0;
    public int BestScore
    {
        get => bestScore;
    }
    public int CurrentScore
    {
        get => currentWaveIndex;
    }

    private const string BestScoreKey = "BestScore";
    private static HashSet<string> _destroyedObjects = new HashSet<string>();
    public static List<GameObject> Friends = new List<GameObject>();
    public static bool _hasGoldenSword = false;

    private void Awake()
    {
        instance = this;
        Player = FindObjectOfType<PlayerController>();
        Player.Init(this);

        UIManager = FindObjectOfType<UIManager>();

        _playerResourceController = Player.GetComponent<ResourceController>();
        _playerResourceController.RemoveHealthChangeEvent(UIManager.ChangePlayerHP);
        _playerResourceController.AddHealthChangeEvent(UIManager.ChangePlayerHP);

        EnemyManager = GetComponentInChildren<EnemyManager>();
        EnemyManager.Init(this);

        bestScore = PlayerPrefs.GetInt(BestScoreKey, 0);
    }

    private void Start()
    {
        if (_hasGoldenSword) Player?.ChangeWeapon();
    }

    public void StartGame()
    {
        StartNextWave();
    }

    private void StartNextWave()
    {
        currentWaveIndex += 1;
        UIManager.ChangeWave(currentWaveIndex);
        EnemyManager.StartWave(1 + currentWaveIndex / 5);
    }

    public void EndOfWave()
    {
        StartNextWave();
    }

    public void GameOver()
    {
        EnemyManager.StopWave();
        UpdateScore();
        UIManager.SetGameOver();
    }

    private void UpdateScore()
    {
        if (bestScore < currentWaveIndex)
        {
            bestScore = currentWaveIndex;
            PlayerPrefs.SetInt(BestScoreKey, bestScore);
        }
    }
    
    public static bool IsObjectDestroyed(string objectID)
    {
        return _destroyedObjects.Contains(objectID);
    }
    
    public static void MarkObjectAsDestroyed(string objectID)
    {
        if (!_destroyedObjects.Contains(objectID))
        {
            _destroyedObjects.Add(objectID);
        }
    }

    public static void LoadFriendPrefab(string prefabName)
    {
        string path = $"Prefabs/Friend/{prefabName}";
        GameObject prefab = Resources.Load<GameObject>(path);
        
        if (prefab == null)
        {
            Debug.LogWarning(path);
        }
        Friends.Add(prefab);
    }
}
