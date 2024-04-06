using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "Configs/Enemy")]
public class EnemyConfig : ScriptableObject
{
    [Min(0)] public int Health;
    [Min(0)] public int Damage;
    [Min(0)] public float SpeedMove;
    [Min(0)] public float RadiusDetect;
    [Min(0)] public float RadiusAttack;

    [Space]
    public List<Item> ItemList;
}
