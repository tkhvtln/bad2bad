using UnityEngine;

public class PanelGame : MonoBehaviour
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
