using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceController : MonoBehaviour
{
    [SerializeField] private float healthChangeDelay = 0.5f;

    private BaseController _baseController;
    private StatHandler _statHandler;
    private AnimationHandler _animationHandler;

    private float timeSinceLastChange = float.MaxValue;
    
    public int CurrentHealth { get; private set; }
    public int MaxHealth => _statHandler.Health;
    public AudioClip damageClip;

    private Action<int, int> OnChangeHealth;

    private void Awake()
    {
        _statHandler = GetComponent<StatHandler>();
        _animationHandler = GetComponent<AnimationHandler>();
        _baseController = GetComponent<BaseController>();
    }

    void Start()
    {
        CurrentHealth = _statHandler.Health;
    }

    void Update()
    {
        if (timeSinceLastChange < healthChangeDelay)
        {
            timeSinceLastChange += Time.deltaTime;
            if (timeSinceLastChange >= healthChangeDelay)
            {
                _animationHandler.InvincibilityEnd();
            }
        }
    }

    public bool ChangeHealth(int change)
    {
        if (change == 0 || timeSinceLastChange < healthChangeDelay) return false;

        timeSinceLastChange = 0f;
        CurrentHealth += change;
        CurrentHealth = CurrentHealth > MaxHealth ? MaxHealth : CurrentHealth;
        CurrentHealth = CurrentHealth < 0 ? 0 : CurrentHealth;

        OnChangeHealth?.Invoke(CurrentHealth, MaxHealth);
        
        if (change < 0)
        {
            _animationHandler.Damage();
            if (damageClip != null) SoundManager.PlayClip(damageClip);
        }
        if (CurrentHealth <= 0f) Death();

        return true;
    }

    public void AddHealthChangeEvent(Action<int, int> action)
    {
        OnChangeHealth += action;
    }

    public void RemoveHealthChangeEvent(Action<int, int> action)
    {
        OnChangeHealth -= action;
    }

    private void Death()
    {
        _baseController.Death();
    }
}
