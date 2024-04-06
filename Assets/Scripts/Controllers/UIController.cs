using UnityEngine;

public class UIController : MonoBehaviour
{
    public PanelMenu panelMenu;
    public PanelGame panelGame;
    public PanelWin panelWin;
    public PanelDefeat panelDefeat;
    public PanelInventory panelInventory;

    public void Init() 
    {
        panelMenu.Init();
        panelGame.Init();
        panelWin.Init();
        panelDefeat.Init();
        panelInventory.Init();
    }

    public void ShowPanelMenu() 
    {
        Clear();
        panelMenu.Show();
    }

    public void ShowPanelGame() 
    {
        Clear();
        panelGame.Show();
    }

    public void ShowPanelWin() 
    {
        Clear();
        panelWin.Show();
    }

    public void ShowPanelDefeat() 
    {
        Clear();
        panelDefeat.Show();
    }

    public void ShowPanelInventory() 
    {
        Clear();
        panelInventory.Show();
    }

    public void OnButtonPlay() 
    {
        GameController.instance.Game();
    }

    public void OnButtonNextLevel() 
    {
        GameController.instance.LoadNextLevel();
    }

    public void OnButtonRestartLevel() 
    {
        GameController.instance.LoadCurrentLevel();
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
        panelInventory.ThrowItem();
    }

    public void Clear() 
    {
        panelMenu.Hide();
        panelGame.Hide();
        panelWin.Hide();
        panelDefeat.Hide();
        panelInventory.Hide();
    }
}
