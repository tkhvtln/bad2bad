using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelDefeat : MonoBehaviour
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
