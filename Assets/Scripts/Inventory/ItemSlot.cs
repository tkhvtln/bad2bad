[System.Serializable]
public class ItemSlot 
{
    public int id;
    public int count;
    public SpriteData icon;

    public ItemSlot(ItemConfig itemConfig)
    {
        id = itemConfig.ID;
        count = itemConfig.Count;
        icon = SpriteData.FromSprite(itemConfig.Icon);
    }
}
