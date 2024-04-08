using UniRx;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public abstract class Enemy : MonoBehaviour, IDamageable
{
    [SerializeField] protected EnemyConfig _enemyConfig;

    [Space]
    [SerializeField] protected Slider _sliderHealth;
    [SerializeField] protected ParticleSystem _particleBlood;

    protected int _health;
    protected int _damage;

    protected Transform _trTarget;
    protected Transform _transform;

    protected bool _isFaceLeft;

    protected int _animIdle;
    protected int _animWalk;
    protected int _animAttack;

    protected float _offsetDistance;
    protected float _distanceToTarget;

    protected Item _item;
    protected Animator _animator;
    protected NavMeshAgent _agent;

    protected Behavior _behavior;
    
    protected IntReactiveProperty _countEnemies;

    IDamageable _iDamagable;

    private void Update()
    {
        if (!GameController.instance.isGame) return;

        Detect();

        if (_trTarget == null) return;

        _distanceToTarget = Vector2.Distance(_transform.position, _trTarget.position);

        Move();
        Look();
        Attack();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _enemyConfig.radiusAttack);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _enemyConfig.radiusDetect);
    }

    public virtual void Init(IntReactiveProperty countEnemies)
    {
        _isFaceLeft = false;

        _health = _enemyConfig.health;
        _damage = _enemyConfig.damage;

        _transform = transform;
        _countEnemies = countEnemies;

        _particleBlood.gameObject.SetActive(false);

        _agent = GetComponent<NavMeshAgent>(); 
        _agent.updateUpAxis = false;
        _agent.updateRotation = false;

        _animator = GetComponent<Animator>();
        _animIdle = Animator.StringToHash(Constants.ANIM_IDLE);
        _animWalk = Animator.StringToHash(Constants.ANIM_WALK);
        _animAttack = Animator.StringToHash(Constants.ANIM_ATTACK);

        int itemIndex = Random.Range(0, _enemyConfig.itemList.Count);
        _item = Instantiate(_enemyConfig.itemList[itemIndex], _transform);
        _item.gameObject.SetActive(false);

        GameController.onCompleted.AddListener(() =>
        {
            _trTarget = null;
            SetAnimation(Behavior.IDLE);
        });
    }

    public virtual void MakeDamage()
    {
        if (_iDamagable != null) 
            _iDamagable.TakeDamage(_enemyConfig.damage);
    }

    public virtual void TakeDamage(int damage)
    {
        _health -= damage;
        _sliderHealth.value = _health * 0.01f;

        if (_health <= 0)
            Die();
    }

    private void Die()
    {
        _countEnemies.Value--;

        _particleBlood.transform.parent = transform.parent;
        _particleBlood.gameObject.SetActive(true);

        _item.transform.parent = transform.parent;
        _item.gameObject.SetActive(true);

        gameObject.SetActive(false);
    }

    protected virtual void Detect()
    {
        if (_trTarget == null)
        {
            Collider2D colliderTarget = Physics2D.OverlapCircle(_transform.position, _enemyConfig.radiusDetect, LayerMask.GetMask(Constants.LAYER_PLAYER));

            if (colliderTarget != null)
            {
                _agent.speed = _enemyConfig.speedMove;
                _trTarget = colliderTarget.transform;
                _iDamagable = _trTarget.GetComponent<IDamageable>();
            }
        }
        else

        if (_trTarget != null && (_distanceToTarget > _enemyConfig.radiusDetect))
        {
            _agent.speed = 0;
            _trTarget = null;
            _iDamagable = null;
            
            SetAnimation(Behavior.IDLE);
        }
    }

    protected virtual void Move()
    {
        bool isMinDistance = _distanceToTarget < _enemyConfig.radiusAttack + _offsetDistance;
        bool isMaxDistance = _distanceToTarget > _enemyConfig.radiusDetect;

        if (isMinDistance || isMaxDistance) return;

        _agent.SetDestination(_trTarget.position);
        _offsetDistance = 0;

        SetAnimation(Behavior.WALK);
    }

    protected virtual void Attack()
    {
        bool isMinDistance = _distanceToTarget > _enemyConfig.radiusAttack;
        if (isMinDistance) return;

        _offsetDistance = 0.1f;
        SetAnimation(Behavior.ATTACK);       
    }

    protected virtual void Look()
    {
        if (_transform.position.x > _trTarget.position.x && !_isFaceLeft)
            Flip();
        else if (_transform.position.x < _trTarget.position.x && _isFaceLeft)
            Flip();
    }

    protected virtual void Flip()
    {
        _transform.Rotate(0, 180, 0);
        _isFaceLeft = !_isFaceLeft;

        _sliderHealth.transform.eulerAngles = Vector2.right;
    }

    protected virtual void SetAnimation(Behavior behavior)
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
}
