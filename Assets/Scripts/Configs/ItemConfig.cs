using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Configs/Item")]
public class ItemConfig : ScriptableObject
{
    public Sprite icon;
    public string title;
    public int count = 1;
}
