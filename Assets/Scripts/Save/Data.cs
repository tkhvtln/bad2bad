using System.Collections.Generic;

[System.Serializable]
public class Data
{
    public int level;
    public List<ItemSlot> itemSlots;

    public Data()
    {
        level = 1;
        itemSlots = new List<ItemSlot>();
    }
}
