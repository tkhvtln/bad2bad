using UnityEngine;

[CreateAssetMenu(fileName = "Bullet", menuName = "Configs/Bullet")]
public class BulletConfig : ScriptableObject
{
    [Min(0)] public int Damage;
    [Min(0)] public float Speed;
    [Min(0)] public float Distance;

    [Space]
    public Bullet BulletPrefab;
}
