using UnityEngine;

public class Item : MonoBehaviour, ICollectable
{
    public ItemConfig config => _itemConfig;

    [SerializeField] private ItemConfig _itemConfig;

    public void Collect()
    {
        gameObject.SetActive(false);
    }
}
