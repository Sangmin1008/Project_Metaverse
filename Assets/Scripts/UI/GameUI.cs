using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameUI : BaseUI
{
    [SerializeField] private TextMeshProUGUI waveText;

    private void Start()
    {
        foreach (var friend in GameManager.Friends)
        {
            if (friend != null)
            {
                Instantiate(friend, GameManager.instance.Player.transform.position + Vector3.up, Quaternion.identity);
            }
        }
        GameManager.instance.StartGame();
    }

    public void UpdateWaveText(int wave)
    {
        waveText.text = wave.ToString();
    }

    protected override UIState GetUIState()
    {
        return UIState.Game;
    }
}
