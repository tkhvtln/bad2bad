using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class Weapon : MonoBehaviour
{
    public int RateOfFire => _weaponConfig.RateOfFire;

    [SerializeField] protected WeaponConfig _weaponConfig;
    [SerializeField] protected Transform _trSpawnerBullet;

    protected ObjectPool<Bullet> _objectPool;

    public void Init()
    {
        _objectPool = new ObjectPool<Bullet>(_weaponConfig.BulletData.BulletPrefab, 20, GameController.Instance.ControllerLevel.transform, true);
        foreach (Bullet bullet in _objectPool.Pool)
            bullet.Init();
    }

    public void Fire() 
    {
        Vector3 vecScatter = transform.eulerAngles;
        vecScatter.z += Random.Range(-_weaponConfig.Scatter, _weaponConfig.Scatter);

        _trSpawnerBullet.position = _trSpawnerBullet.position;
        _trSpawnerBullet.rotation = Quaternion.Euler(vecScatter);

        Bullet bullet = _objectPool.GetFreeElement();
        bullet.Fire(_trSpawnerBullet);
    }
}
