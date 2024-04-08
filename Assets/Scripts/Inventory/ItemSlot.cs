[System.Serializable]
public class ItemSlot 
{
    public int count;
    public string title;
    public SpriteData icon;

    public ItemSlot(ItemConfig itemConfig)
    {
        title = itemConfig.title;
        count = itemConfig.count;
        icon = SpriteData.FromSprite(itemConfig.icon);
    }
}
