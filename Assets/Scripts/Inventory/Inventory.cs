using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static UnityEditor.Progress;

public class Inventory : MonoBehaviour
{
    public static UnityEvent<List<ItemSlot>> onInventory = new UnityEvent<List<ItemSlot>>();

    [SerializeField] private List<ItemSlot> _itemSlots;

    private Data _data;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Item item))
        {
            item.Collect();

            int indexItem = _itemSlots.FindIndex(x => x.id == item.Config.ID);
            if (indexItem > -1)
                _itemSlots[indexItem].count++;
            else
                _itemSlots.Add(new ItemSlot(item.Config));

            _data.itemSlots = _itemSlots;
            SaveSystem.Save(GameController.instance.data);
            
            onInventory?.Invoke(_itemSlots);
        }
    }

    public void Init(Data data)
    {
        _data = data;
        _itemSlots = _data.itemSlots;

        onInventory?.Invoke(_itemSlots);
    }

    public void RemoveItem(int index)
    {
        _itemSlots.RemoveAt(index);
        onInventory?.Invoke(_itemSlots);

        SaveSystem.Save(GameController.instance.data);
    }
}
