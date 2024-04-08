using UnityEngine;

[CreateAssetMenu(fileName = "Bullet", menuName = "Configs/Bullet")]
public class BulletConfig : ScriptableObject
{
    [Min(0)] public int damage;
    [Min(0)] public float speed;
    [Min(0)] public float distance;

    [Space]
    public Bullet bulletPrefab;
}
