using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SimpleInputs;
using UnityEngine.Windows;

public class PauseControl : MonoBehaviour
{
    [SerializeField] private Inputs inputs;
    [SerializeField] private GameObject pauseCanvas;

    private bool isGame;

    void Start()
    {
        isGame = true;

        inputs.PlayerInputs.Escape.OnStarted.AddListener(SwitchPause);
    }

    public void SwitchPause()
    {
        isGame =!isGame;

        SetPause(!isGame);
    }

    public void SetPause(bool isPause)
    {
        inputs.gameState = isPause ? GameState.Menu : GameState.Game;
        inputs.PlayerInputs.inputLocker.IsEnabled = isPause;
        pauseCanvas.SetActive(isPause);
    }
}
