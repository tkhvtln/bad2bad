using System.Collections.Generic;

[System.Serializable]
public class Data
{
    public int Level;
    public List<ItemSlot> ItemSlots;

    public Data()
    {
        Level = 1;
        ItemSlots = new List<ItemSlot>();
    }
}
