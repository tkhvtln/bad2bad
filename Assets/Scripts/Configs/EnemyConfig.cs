using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "Configs/Enemy")]
public class EnemyConfig : ScriptableObject
{
    [Min(0)] public int health;
    [Min(0)] public int damage;
    [Min(0)] public float speedMove;
    [Min(0)] public float radiusDetect;
    [Min(0)] public float radiusAttack;

    [Space]
    public List<Item> itemList;
}
