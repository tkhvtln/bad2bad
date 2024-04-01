using UnityEngine;

[CreateAssetMenu(fileName = "Player", menuName = "Configs/Player")]
public class PlayerConfig : ScriptableObject
{
    [Min(0)] public float SpeedMove;
    [Min(0)] public float RadiusAttack;
}
