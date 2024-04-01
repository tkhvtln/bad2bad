using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class Bullet : MonoBehaviour
{
    [SerializeField] private BulletConfig _bulletConfig;

    private Transform _transform;
    private Transform _trSpawner;

    private CompositeDisposable _disposable;

    public void Init()
    {
        _transform = transform;
    }

    private void OnDestroy()
    {
        _disposable?.Dispose();
    }

    public void Fire(Transform trSpawner)
    {
        _trSpawner = trSpawner;

        _transform.position = _trSpawner.position;
        _transform.rotation = _trSpawner.rotation;


        _disposable = new CompositeDisposable();

        Observable.EveryUpdate()
            .Subscribe(_ =>
            {
                _transform.Translate(Vector2.right * Time.deltaTime * _bulletConfig.Speed);

                if (Vector3.Distance(_transform.position, _trSpawner.position) > _bulletConfig.Distance)
                {
                    _disposable.Dispose();
                    gameObject.SetActive(false);
                }
            }).AddTo(_disposable);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IDamageable damageable))
            damageable.TakeDamage(_bulletConfig.Damage);
            
        _disposable?.Dispose();
        gameObject.SetActive(false); 
    }
}
