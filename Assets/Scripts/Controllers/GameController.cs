using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController Instance;
    public bool IsGame => _isGame;

    public UIController ControllerUI;
    public SaveController ControllerSave;
    public SoundController ControllerSound;
    public PlayerController ControllerPlayer;
    public CameraController ControllerCamera;

    private bool _isGame;
    private bool _isSceneLoaded;

    void Awake() 
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }

    void Start()
    {
        ControllerSave.Load();
        ControllerCamera.Init();
        ControllerPlayer.Init();
        ControllerSound.Init();
        ControllerUI.Init();

        LoadCurrentLevel();
    }

    public void Game() 
    {
        _isGame = true;
        ControllerUI.ShowPanelGame();
    }

    public void Win()
    {
        _isGame = false;
        ControllerUI.ShowPanelWin();
    }

    public void Defeat() 
    {
        _isGame = false;
        ControllerUI.ShowPanelDefeat();
    }

    public void LoadCurrentLevel() 
    {
        UnloadScene();
        LoadScene();
    }

    public void LoadNextLevel() 
    {
        UnloadScene();

        ControllerSave.DataPlayer.Level = ++ControllerSave.DataPlayer.Level >= SceneManager.sceneCountInBuildSettings ? 1 : ControllerSave.DataPlayer.Level;
        ControllerSave.Save();

        LoadScene();
    }

    private void LoadScene()
    {
        if (!_isSceneLoaded)
        {
            _isSceneLoaded = true;
            SceneManager.LoadSceneAsync(ControllerSave.DataPlayer.Level, LoadSceneMode.Additive);
        }

        ControllerUI.ShowPanelMenu();
    }

    private void UnloadScene()
    {
        if (_isSceneLoaded)
        {
            _isSceneLoaded = false;
            SceneManager.UnloadSceneAsync(ControllerSave.DataPlayer.Level);
        }
    }
}
