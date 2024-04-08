using UnityEngine;

[CreateAssetMenu(fileName = "Player", menuName = "Configs/Player")]
public class PlayerConfig : ScriptableObject
{
    [Min(0)] public int health;
    [Min(0)] public float speedMove;
    [Min(0)] public float radiusAttack;
}
