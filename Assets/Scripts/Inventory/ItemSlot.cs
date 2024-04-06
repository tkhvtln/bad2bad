[System.Serializable]
public class ItemSlot 
{
    public int ID;
    public int Count;

    public ItemSlot(ItemConfig itemConfig)
    {
        ID = itemConfig.ID;
        Count = itemConfig.Count;
    }
}
