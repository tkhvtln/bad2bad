using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelInventory : MonoBehaviour
{
    [SerializeField] private Inventory _inventory;
    [SerializeField] private Button _btnThrow;

    [Space]
    [SerializeField] private List<ItemUI> _itemsUI;

    public int indexItemSelected { get; set; }

    public void Init()
    {
        _btnThrow.gameObject.SetActive(false);

        Inventory.onInventory.AddListener(Refresh);
        ItemUI.onSelect.AddListener((index, isSlot) =>
        {
            indexItemSelected = index;
            foreach (var itemUI in _itemsUI)
                itemUI.Unselect();

            _btnThrow.gameObject.SetActive(isSlot);
        });
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide() 
    {
        gameObject.SetActive(false);
    }

    public void Refresh(List<ItemSlot> itemSlot)
    {
        for (int i = 0; i < _itemsUI.Count; i++)
            _itemsUI[i].Clear();

        for (int i = 0; i < itemSlot.Count; i++)
            _itemsUI[i].Set(itemSlot[i]);       
    }

    public void ThrowItem()
    {
        _btnThrow.gameObject.SetActive(false);
        _inventory.RemoveItem(indexItemSelected);
        _itemsUI[indexItemSelected].ItemSlot = null;
    }
}
