using UnityEngine;
using UnityEngine.Events;

public abstract class Weapon : MonoBehaviour
{
    public static UnityEvent<int> onFire = new UnityEvent<int>();

    public int rateOfFire => _weaponConfig.rateOfFire;

    [SerializeField] protected WeaponConfig _weaponConfig;
    [SerializeField] protected Transform _trSpawnerBullet;

    protected int _clip;

    protected ObjectPool<Bullet> _objectPool;

    public void Init()
    {
        _clip = _weaponConfig.clip;
        onFire?.Invoke(_clip);

        _objectPool = new ObjectPool<Bullet>(_weaponConfig.bulletData.bulletPrefab, 20, GameController.instance.ControllerLevel.transform, true);
        foreach (Bullet bullet in _objectPool.pool)
            bullet.Init();
    }

    public void Fire() 
    {
        if (_clip <= 0) return;

        _clip--;

        Vector3 vecScatter = transform.eulerAngles;
        vecScatter.z += Random.Range(-_weaponConfig.scatter, _weaponConfig.scatter);

        _trSpawnerBullet.position = _trSpawnerBullet.position;
        _trSpawnerBullet.rotation = Quaternion.Euler(vecScatter);

        Bullet bullet = _objectPool.GetFreeElement();
        bullet.Fire(_trSpawnerBullet);

        onFire?.Invoke(_clip);
    }
}
