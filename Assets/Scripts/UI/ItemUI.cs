using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour
{
    public static UnityEvent<int, bool> OnSelect = new UnityEvent<int, bool>();

    public ItemSlot ItemSlot { get; set; }

    [SerializeField] private Image _icon;
    [SerializeField] private Image _background;
    [SerializeField] private TextMeshProUGUI _count;

    public void Set(ItemSlot itemSlot)
    {
        ItemSlot = itemSlot;

        //_icon.sprite = itemSlot.Icon;
        _count.text = $"{itemSlot.ID}/{itemSlot.Count}";
    }

    public void Clear()
    {
        //_icon.sprite = null;
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
        OnSelect?.Invoke(transform.GetSiblingIndex(), ItemSlot != null);
        Select();
    }
}
