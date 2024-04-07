using TMPro;
using UnityEngine;

public class PanelGame : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textBulletClip;

    public void Init() 
    {
        Weapon.onFire.AddListener(clip => _textBulletClip.text = $"{clip}");
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
