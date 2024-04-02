using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public abstract class Enemy : MonoBehaviour, IDamageable
{
    [SerializeField] protected EnemyConfig _enemyConfig;

    protected int _health;
    protected int _damage;

    protected Transform _trTarget;
    protected Transform _transform;

    protected bool _isFaceLeft;

    private int _animIdle;
    private int _animWalk;
    private int _animAttack;

    private float _offsetDistance;

    protected Behavior _behavior;
    protected Animator _animator;

    private IntReactiveProperty _countEnemies;

    public void Init(IntReactiveProperty countEnemies)
    {
        _isFaceLeft = false;

        _health = _enemyConfig.Health;
        _damage = _enemyConfig.Damage;

        _transform = transform;
        _countEnemies = countEnemies;

        _animator = GetComponent<Animator>();
        _animIdle = Animator.StringToHash(Constants.ANIM_IDLE);
        _animWalk = Animator.StringToHash(Constants.ANIM_WALK);
        _animAttack = Animator.StringToHash(Constants.ANIM_ATTACK);
    }

    private void Update()
    {
        if (!GameController.Instance.IsGame) return;

        Detect();

        if (_trTarget == null) return;

        Move();
        Look();
        Attack();
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

    private void Detect()
    {
        if (_trTarget == null)
        {
            Collider2D colliderTarget = Physics2D.OverlapCircle(_transform.position, _enemyConfig.RadiusDetect, LayerMask.GetMask(Constants.LAYER_PLAYER));

            if (colliderTarget != null)
                _trTarget = colliderTarget.transform;
        }

        if (_trTarget != null && (Vector2.Distance(_transform.position, _trTarget.position) > _enemyConfig.RadiusDetect))
        {
            _trTarget = null;
            SetAnimation(Behavior.IDLE);
        }
    }

    private void Move()
    {
        bool isMinDistance = Vector2.Distance(_transform.position, _trTarget.position) < _enemyConfig.RadiusAttack + _offsetDistance;
        bool isMaxDistance = Vector2.Distance(_transform.position, _trTarget.position) > _enemyConfig.RadiusDetect;

        if (isMinDistance || isMaxDistance) return;

        Vector3 vecTarget = (_transform.position - _trTarget.position).normalized;
        _transform.position -= vecTarget * _enemyConfig.SpeedMove * Time.deltaTime;

        _offsetDistance = 0;
        SetAnimation(Behavior.WALK);
    }

    protected void Attack()
    {    
        bool isMinDistance = Vector2.Distance(_transform.position, _trTarget.position) > _enemyConfig.RadiusAttack;
        
        if (isMinDistance) return;

        _offsetDistance = 0.1f;
        SetAnimation(Behavior.ATTACK);
    }

    protected void Look()
    {
        if (_transform.position.x > _trTarget.position.x && !_isFaceLeft)
            Flip();
        else if (_transform.position.x < _trTarget.position.x && _isFaceLeft)
            Flip();
    }

    protected void Flip()
    {
        _transform.Rotate(0, 180, 0);
        _isFaceLeft = !_isFaceLeft;
    }

    private void SetAnimation(Behavior behavior)
    {
        if (_behavior == behavior) return;

        _behavior = behavior;

        switch (_behavior)
        {
            case Behavior.WALK:
                _animator.SetTrigger(_animWalk);
                break;

            case Behavior.ATTACK:
                _animator.SetTrigger(_animAttack);
                break;

            default:
                _animator.SetTrigger(_animIdle);
                break;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _enemyConfig.RadiusAttack);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _enemyConfig.RadiusDetect);
    }
}
