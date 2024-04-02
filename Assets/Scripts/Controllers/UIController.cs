using UnityEngine;

public class UIController : MonoBehaviour
{
    #region FIELD

    public PanelMenu PanelMenu;
    public PanelGame PanelGame;
    public PanelWin PanelWin;
    public PanelDefeat PanelDefeat;

    #endregion

    #region METODS

    public void Init() 
    {
        PanelMenu.Init();
        PanelGame.Init();
        PanelWin.Init();
        PanelDefeat.Init();
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

    public void Clear() 
    {
        PanelMenu.Hide();
        PanelGame.Hide();
        PanelWin.Hide();
        PanelDefeat.Hide();
    }

    #endregion
}
