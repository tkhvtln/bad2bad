using UnityEngine;

public class PanelWin : MonoBehaviour
{
    public void Init() 
    {

    }

    public void Show() 
    {
        gameObject.SetActive(true);
    }

    public void Hide() 
    {
        gameObject.SetActive(false);
    }
}
