using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "Configs/Weapon")]
public class WeaponConfig : ScriptableObject
{
    [Min(0), Tooltip("Interval between frames")] 
    public int rateOfFire;

    [Min(0)] 
    public float scatter;

    [Min(0)]
    public int clip;

    [Space]
    public BulletConfig bulletData;
}
