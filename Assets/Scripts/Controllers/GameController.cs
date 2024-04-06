using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController Instance;
    public static UnityEvent OnGame = new UnityEvent();
    public static UnityEvent OnCompleted = new UnityEvent();

    public bool IsGame { get; private set; }
    public Data Data { get; set; }
    public LevelController ControllerLevel { get; set; }

    public UIController ControllerUI;
    public SaveController ControllerSave;
    public PlayerController ControllerPlayer;

    [Space]
    [SerializeField] private Inventory _inventory;

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
        Data = SaveSystem.Load();

        //ControllerSave.Load();
        ControllerUI.Init();
        ControllerPlayer.Init();

        _inventory.Init(Data);

        LoadCurrentLevel();
    }

    public void Game() 
    {
        IsGame = true;
        ControllerUI.ShowPanelGame();

        OnGame?.Invoke();
    }

    public void Win()
    {
        IsGame = false;
        ControllerUI.ShowPanelWin();

        OnCompleted?.Invoke();
    }

    public void Defeat() 
    {
        IsGame = false;
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

        //ControllerSave.DataPlayer.Level = ++ControllerSave.DataPlayer.Level >= SceneManager.sceneCountInBuildSettings ? 1 : ControllerSave.DataPlayer.Level;
        //ControllerSave.Save();
        Data.Level = ++ControllerSave.DataPlayer.Level >= SceneManager.sceneCountInBuildSettings ? 1 : ControllerSave.DataPlayer.Level;
        SaveSystem.Save(Data);

        LoadScene();
    }

    private void LoadScene()
    {
        if (!_isSceneLoaded)
        {
            _isSceneLoaded = true;
            //SceneManager.LoadSceneAsync(ControllerSave.DataPlayer.Level, LoadSceneMode.Additive);
            SceneManager.LoadSceneAsync(Data.Level, LoadSceneMode.Additive);
        }

        ControllerPlayer.ResetPlayer();
        ControllerUI.ShowPanelMenu();     
    }

    private void UnloadScene()
    {
        if (_isSceneLoaded)
        {
            _isSceneLoaded = false;
            //SceneManager.UnloadSceneAsync(ControllerSave.DataPlayer.Level);
            SceneManager.UnloadSceneAsync(Data.Level);
        }
    }
}
