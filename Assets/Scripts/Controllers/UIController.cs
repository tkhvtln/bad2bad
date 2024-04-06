using UnityEngine;

public class UIController : MonoBehaviour
{
    public PanelMenu PanelMenu;
    public PanelGame PanelGame;
    public PanelWin PanelWin;
    public PanelDefeat PanelDefeat;
    public PanelInventory PanelInventory;

    public void Init() 
    {
        PanelMenu.Init();
        PanelGame.Init();
        PanelWin.Init();
        PanelDefeat.Init();
        PanelInventory.Init();
    }

    public void ShowPanelMenu() 
    {
        Clear();
        PanelMenu.Show();
    }

    public void ShowPanelGame() 
    {
        Clear();
        PanelGame.Show();
    }

    public void ShowPanelWin() 
    {
        Clear();
        PanelWin.Show();
    }

    public void ShowPanelDefeat() 
    {
        Clear();
        PanelDefeat.Show();
    }

    public void ShowPanelInventory() 
    {
        Clear();
        PanelInventory.Show();
    }

    public void OnButtonPlay() 
    {
        GameController.Instance.Game();
    }

    public void OnButtonNextLevel() 
    {
        GameController.Instance.LoadNextLevel();
    }

    public void OnButtonRestartLevel() 
    {
        GameController.Instance.LoadCurrentLevel();
    }

    public void OnButtonInventory()
    {
        Time.timeScale = 0;
        ShowPanelInventory();
    }

    public void OnButtonExitInventory()
    {
        Time.timeScale = 1;
        ShowPanelGame();
    }

    public void OnButtonThrowItem()
    {
        PanelInventory.ThrowItem();
    }

    public void Clear() 
    {
        PanelMenu.Hide();
        PanelGame.Hide();
        PanelWin.Hide();
        PanelDefeat.Hide();
        PanelInventory.Hide();
    }
}
