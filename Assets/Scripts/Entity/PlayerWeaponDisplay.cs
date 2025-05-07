using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerWeaponDisplay : MonoBehaviour
{
    [SerializeField] private Image SelectWeaponUI;

    public void UpdateWeaponUI(WeaponHandler weaponHandler)
    {
        if (weaponHandler != null && weaponHandler.WeaponRenderer != null && weaponHandler.WeaponRenderer.sprite != null)
        {
            SelectWeaponUI.sprite = weaponHandler.WeaponRenderer.sprite;
            SelectWeaponUI.SetNativeSize();
        }
    }
}
