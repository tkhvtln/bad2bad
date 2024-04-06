using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static UnityEditor.Progress;

public class Inventory : MonoBehaviour
{
    public static UnityEvent<List<ItemSlot>> OnInventory = new UnityEvent<List<ItemSlot>>();

    [SerializeField] private List<ItemSlot> _itemSlots;

    private Data _data;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Item item))
        {
            item.Collect();

            int indexItem = _itemSlots.FindIndex(x => x.ID == item.Config.ID);
            if (indexItem > -1)
                _itemSlots[indexItem].Count++;
            else
                _itemSlots.Add(new ItemSlot(item.Config));

            _data.ItemSlots = _itemSlots;
            SaveSystem.Save(GameController.Instance.Data);
            
            OnInventory?.Invoke(_itemSlots);
        }
    }

    public void Init(Data data)
    {
        _data = data;
        _itemSlots = _data.ItemSlots;

        OnInventory?.Invoke(_itemSlots);
    }

    public void RemoveItem(int index)
    {
        _itemSlots.RemoveAt(index);
        OnInventory?.Invoke(_itemSlots);

        SaveSystem.Save(GameController.Instance.Data);
    }
}
