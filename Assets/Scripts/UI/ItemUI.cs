using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour
{
    public static UnityEvent<int, bool> onSelect = new UnityEvent<int, bool>();

    public ItemSlot ItemSlot { get; set; }

    [SerializeField] private Image _icon;
    [SerializeField] private Image _background;
    [SerializeField] private TextMeshProUGUI _count;

    public void Set(ItemSlot itemSlot)
    {
        ItemSlot = itemSlot;

        _icon.gameObject.SetActive(true);

        _icon.sprite = SpriteData.ToSprite(itemSlot.icon);
        _icon.rectTransform.sizeDelta = new Vector2(_icon.sprite.rect.width * 5, _icon.sprite.rect.height * 5);

        string count = itemSlot.count <= 1 ? "" : $"{itemSlot.count}";
        _count.text = $"{count}";
    }

    public void Clear()
    {
        _icon.gameObject.SetActive(false);

        _icon.sprite = null;
        _count.text = "";
    }

    public void Select()
    {
        _background.color = Color.red;
    }

    public void Unselect()
    {
        _background.color = Color.white;
    }

    public void OnButtonSelect()
    {
        onSelect?.Invoke(transform.GetSiblingIndex(), ItemSlot != null);
        Select();
    }
}
