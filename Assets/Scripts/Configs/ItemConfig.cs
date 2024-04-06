using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Configs/Item")]
public class ItemConfig : ScriptableObject
{
    public Sprite Icon;
    public int ID;
    public int Count = 1;
}
