using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "Configs/Weapon")]
public class WeaponConfig : ScriptableObject
{
    [Min(0), Tooltip("Interval between frames")] 
    public int RateOfFire;

    [Min(0)] 
    public float Scatter;

    [Space]
    public BulletConfig BulletData;
}
