using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerController : BaseController
{
    private GameManager _gameManager;
    private Camera _camera;
    private PlayerWeaponDisplay _playerWeaponDisplay;
    private ConsoleController _consoleController;

    protected override void Awake()
    {
        base.Awake();
        _playerWeaponDisplay = GetComponent<PlayerWeaponDisplay>();
        _playerWeaponDisplay.UpdateWeaponUI(weaponHandler);
    }
    
    public void Init(GameManager gameManager)
    {
        _gameManager = gameManager;
        _camera = Camera.main;
    }

    public override void Death()
    {
        base.Death();
        _gameManager.GameOver();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var console = other.GetComponent<ConsoleController>();
        if (console != null) _consoleController = console;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        var console = other.GetComponent<ConsoleController>();
        if (console != null && _consoleController == console)
        {
            _consoleController = null;
        }
    }

    public void ChangeWeapon()
    {
        if (GameManager._hasGoldenSword)
        {
            string path = "Prefabs/Weapon/P_Golden_Sword_EquipWeapon";
            WeaponHandler newWeaponPrefab = Resources.Load<WeaponHandler>(path);

            if (newWeaponPrefab == null)
            {
                Debug.LogWarning("무기 못찾음");
                return;
            }

            if (weaponHandler != null)
            {
                Destroy(weaponHandler.gameObject);
                weaponHandler = null;
            }

            Debug.Log("무기 생성");
            weaponHandler = Instantiate(newWeaponPrefab, weaponPivot);
            WeaponPrefab = newWeaponPrefab;
            
            _playerWeaponDisplay?.UpdateWeaponUI(weaponHandler);
        }
    }

    void OnMove(InputValue inputValue)
    {
        movementDirection = (_consoleController != null && _consoleController.isActive) ? Vector2.zero : inputValue.Get<Vector2>();
        movementDirection = movementDirection.normalized;
    }

    void OnLook(InputValue inputValue)
    {
        Vector2 mousePosition = inputValue.Get<Vector2>();
        Vector2 worldPos = _camera.ScreenToWorldPoint(mousePosition);
        lookDirection = (worldPos - (Vector2)transform.position);

        if (lookDirection.magnitude < 0.9f || (_consoleController != null && _consoleController.isActive))
        {
            lookDirection = Vector2.zero;
        }
        else
        {
            lookDirection = lookDirection.normalized;
        }
    }

    void OnFire(InputValue inputValue)
    {
        if (_consoleController != null && _consoleController.isActive) return;
        if (EventSystem.current.IsPointerOverGameObject()) return;
        isAttacking = inputValue.isPressed;
    }

    void OnAction(InputValue inputValue)
    {
        if (inputValue.isPressed && _consoleController != null)
        {
            _consoleController.TriggerMessage();
        }
    }
}
