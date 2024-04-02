using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    #region STATICS

    public static GameController Instance;
    public static UnityEvent OnGame = new UnityEvent();
    public static UnityEvent OnCompleted = new UnityEvent();

    #endregion

    #region GETTETS

    public bool IsGame => _isGame;
    public LevelController ControllerLevel { get; set; }

    #endregion

    #region FIELDS

    public UIController ControllerUI;
    public SaveController ControllerSave;
    public PlayerController ControllerPlayer;
    public CameraController ControllerCamera;

    private bool _isGame;
    private bool _isSceneLoaded;

    #endregion

    #region UNITY

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
        ControllerUI.Init();

        LoadCurrentLevel();
    }

    #endregion

    #region METODS

    public void Game() 
    {
        _isGame = true;
        ControllerUI.ShowPanelGame();

        OnGame?.Invoke();
    }

    public void Win()
    {
        _isGame = false;
        ControllerUI.ShowPanelWin();

        OnCompleted?.Invoke();
    }

    public void Defeat() 
    {
        _isGame = false;
        ControllerUI.ShowPanelDefeat();

        OnCompleted?.Invoke();
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

        ControllerPlayer.ResetPlayer();
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

    #endregion
}
