using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController instance;
    public static UnityEvent onGame = new UnityEvent();
    public static UnityEvent onCompleted = new UnityEvent();

    public bool isGame { get; private set; }
    public Data data { get; set; }
    public LevelController ControllerLevel { get; set; }

    public UIController uiController;
    public SaveController saveController;
    public PlayerController playerController;

    [Space]
    [SerializeField] private Inventory _inventory;

    private bool _isSceneLoaded;

    void Awake() 
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }

    void Start()
    {
        data = SaveSystem.Load();

        //saveController.Load();
        uiController.Init();
        playerController.Init();

        _inventory.Init(data);

        LoadCurrentLevel();
    }

    public void Game() 
    {
        isGame = true;
        uiController.ShowPanelGame();

        onGame?.Invoke();
    }

    public void Win()
    {
        isGame = false;
        uiController.ShowPanelWin();

        onCompleted?.Invoke();
    }

    public void Defeat() 
    {
        isGame = false;
        uiController.ShowPanelDefeat();

        onCompleted?.Invoke();
    }

    public void LoadCurrentLevel() 
    {
        UnloadScene();
        LoadScene();
    }

    public void LoadNextLevel() 
    {
        UnloadScene();

        //saveController.dataPlayer.Level = ++saveController.dataPlayer.Level >= SceneManager.sceneCountInBuildSettings ? 1 : saveController.dataPlayer.Level;
        //saveController.Save();
        data.level = ++data.level >= SceneManager.sceneCountInBuildSettings ? 1 : data.level;
        SaveSystem.Save(data);

        LoadScene();
    }

    private void LoadScene()
    {
        if (!_isSceneLoaded)
        {
            _isSceneLoaded = true;
            //SceneManager.LoadSceneAsync(saveController.dataPlayer.Level, LoadSceneMode.Additive);
            SceneManager.LoadSceneAsync(data.level, LoadSceneMode.Additive);
        }

        playerController.ResetPlayer();
        uiController.ShowPanelMenu();     
    }

    private void UnloadScene()
    {
        if (_isSceneLoaded)
        {
            _isSceneLoaded = false;
            //SceneManager.UnloadSceneAsync(saveController.dataPlayer.Level);
            SceneManager.UnloadSceneAsync(data.level);
        }
    }
}
