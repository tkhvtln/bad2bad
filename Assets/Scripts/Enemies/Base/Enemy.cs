using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public abstract class Enemy : MonoBehaviour, IDamageable
{
    [SerializeField] private EnemyConfig _enemyConfig;

    private int _health;
    private int _damage;

    private IntReactiveProperty _countEnemies;

    public void Init(IntReactiveProperty countEnemies)
    {
        _health = _enemyConfig.Health;
        _damage = _enemyConfig.Damage;

        _countEnemies = countEnemies;
    }

    public void TakeDamage(int damage)
    {
        _health -= damage;

        if (_health <= 0)
        {
            gameObject.SetActive(false);
            _countEnemies.Value--;
        }
    }

    protected void Attack()
    {

    }
}
