[System.Serializable]
public class ItemSlot 
{
    public int id;
    public int count;

    public ItemSlot(ItemConfig itemConfig)
    {
        id = itemConfig.ID;
        count = itemConfig.Count;
    }
}
