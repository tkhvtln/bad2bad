using UniRx;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

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

    protected Item _item;
    protected Animator _animator;
    protected Behavior _behavior;
    
    protected IntReactiveProperty _countEnemies;

    IDamageable _iDamagable;

    private void Update()
    {
        if (!GameController.Instance.IsGame) return;

        Detect();

        if (_trTarget == null) return;

        Move();
        Look();
        Attack();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _enemyConfig.RadiusAttack);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _enemyConfig.RadiusDetect);
    }

    public void Init(IntReactiveProperty countEnemies)
    {
        _isFaceLeft = false;

        _health = _enemyConfig.Health;
        _damage = _enemyConfig.Damage;

        _transform = transform;
        _countEnemies = countEnemies;

        _particleBlood.gameObject.SetActive(false);

        _animator = GetComponent<Animator>();
        _animIdle = Animator.StringToHash(Constants.ANIM_IDLE);
        _animWalk = Animator.StringToHash(Constants.ANIM_WALK);
        _animAttack = Animator.StringToHash(Constants.ANIM_ATTACK);

        int itemIndex = Random.Range(0, _enemyConfig.ItemList.Count);
        _item = Instantiate(_enemyConfig.ItemList[itemIndex], _transform);
        _item.gameObject.SetActive(false);

        GameController.OnCompleted.AddListener(() =>
        {
            _trTarget = null;
            SetAnimation(Behavior.IDLE);
        });
    }

    public void MakeDamage()
    {
        if (_iDamagable != null) 
            _iDamagable.TakeDamage(_enemyConfig.Damage);
    }

    public void TakeDamage(int damage)
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

    private void Detect()
    {
        if (_trTarget == null)
        {
            Collider2D colliderTarget = Physics2D.OverlapCircle(_transform.position, _enemyConfig.RadiusDetect, LayerMask.GetMask(Constants.LAYER_PLAYER));

            if (colliderTarget != null)
            {
                _trTarget = colliderTarget.transform;
                _iDamagable = _trTarget.GetComponent<IDamageable>();
            }
        }

        if (_trTarget != null && (Vector2.Distance(_transform.position, _trTarget.position) > _enemyConfig.RadiusDetect))
        {
            _trTarget = null;
            _iDamagable = null;

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

        _sliderHealth.transform.eulerAngles = Vector2.right;
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
}
