using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour, IDamageable
{
    [SerializeField] private PlayerConfig _playerConfig;

    [Space]
    [SerializeField] private Weapon _weapon;
    [SerializeField] private ParticleSystem _particleBlood;

    [Space]
    [SerializeField] private Transform _trWeapon;
    [SerializeField] private Transform _trsRadiusAttack;

    [Space]
    [SerializeField] private Joystick _joystick;
    [SerializeField] private Button _buttonFire;
    [SerializeField] private Slider _sliderHealth;

    private Transform _trEnemy;
    private Transform _transform;
    private Vector3 _vecInput;

    private Rigidbody2D _rb;
    private Animator _animator;

    private bool _isFire;
    private bool _isFaceLeft;

    private int _health;

    private int _animIdle;
    private int _animWalk;  

    private Behavior _behavior;

    private void Update()
    {
        if (!GameController.instance.isGame) return;

        Detect();
        Look();
    }

    private void FixedUpdate()
    {
        if (!GameController.instance.isGame) return;

        Move();
    }

    public void Init()
    {
        _transform = transform;
        _rb = GetComponent<Rigidbody2D>();
        _trsRadiusAttack.localScale = Vector2.one * _playerConfig.radiusAttack / 3;

        _animator = GetComponent<Animator>();
        _animIdle = Animator.StringToHash(Constants.ANIM_IDLE);
        _animWalk = Animator.StringToHash(Constants.ANIM_WALK);

        Fire();
        ResetPlayer();

        GameController.onGame.AddListener(() => _weapon.Init());
        GameController.onCompleted.AddListener(() => _isFire = false);
    }

    public void ResetPlayer()
    {
        _isFire = false;
        _isFaceLeft = false;

        _health = _playerConfig.health;
        _sliderHealth.value = 1;

        _transform.position = Vector2.zero;
        _transform.eulerAngles = Vector2.zero;

        _trWeapon.localScale = Vector2.one;
        _trWeapon.eulerAngles = Vector2.zero;

        _vecInput = Vector2.zero;
        _rb.velocity = Vector2.zero;
        _joystick.ResetJoystick();

        gameObject.SetActive(true);

        _particleBlood.transform.parent = _transform;
        _particleBlood.transform.localPosition = Vector2.zero;
        _particleBlood.gameObject.SetActive(false);

        SetAnimation(Behavior.IDLE);
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
        _particleBlood.transform.parent = _transform.parent;
        _particleBlood.gameObject.SetActive(true);

        gameObject.SetActive(false);

        GameController.instance.Defeat();
    }

    private void Fire()
    {
        _buttonFire.OnPointerUpAsObservable().Subscribe(_ => _isFire = false);
        _buttonFire.OnPointerDownAsObservable().Subscribe(_ => _isFire = true);

        Observable.IntervalFrame(_weapon.rateOfFire)
            .Where(_ => _isFire && _trEnemy != null)
            .Subscribe(_ => _weapon.Fire());
    }

    private void Move()
    {
        _vecInput = new Vector2(_joystick.Horizontal, _joystick.Vertical);
        _rb.velocity = _vecInput * _playerConfig.speedMove * Time.fixedDeltaTime;

        if (_vecInput != Vector3.zero)  
            SetAnimation(Behavior.WALK);
        else
            SetAnimation(Behavior.IDLE);
    }

    private void Look()
    {
        if (_trEnemy == null)
        {
            if (_vecInput != Vector3.zero)
                Aim(_vecInput);

            if (_vecInput.x < 0 && !_isFaceLeft)
                Flip();
            else if (_vecInput.x > 0 && _isFaceLeft)
                Flip();
        }
        else
        {
            Aim(_trEnemy.position - _trWeapon.position);

            if (_transform.position.x < _trEnemy.position.x && _isFaceLeft)
                Flip();
            else if (_transform.position.x > _trEnemy.position.x && !_isFaceLeft)
                Flip();
        }
    }

    private void Flip()
    {       
        _transform.Rotate(0, 180, 0);
        _isFaceLeft = !_isFaceLeft;

        _sliderHealth.transform.eulerAngles = Vector2.right;
    }

    private void Aim(Vector3 vecTarget)
    {
        Vector2 vecDirectionTarget = vecTarget.normalized;
        float angle = Mathf.Atan2(vecDirectionTarget.y, vecDirectionTarget.x) * Mathf.Rad2Deg;
        _trWeapon.eulerAngles = new Vector3(0, 0, angle);

        Vector2 vecWeaponScale = Vector2.one;
        if (angle > 90 || angle < -90) 
            vecWeaponScale.y = -1;
        else 
            vecWeaponScale.y = +1;

        _trWeapon.localScale = vecWeaponScale;
    }

    private void Detect()
    {
        if (_trEnemy == null)
        {
            Collider2D[] colliderTargets = Physics2D.OverlapCircleAll(_trsRadiusAttack.position, _playerConfig.radiusAttack, LayerMask.GetMask(Constants.LAYER_ENEMY));

            if (colliderTargets.Length > 0 && colliderTargets[0].gameObject.activeSelf)
                _trEnemy = colliderTargets[0].transform;
        }

        if (_trEnemy != null && (Vector2.Distance(_trsRadiusAttack.position, _trEnemy.position) > _playerConfig.radiusAttack || !_trEnemy.gameObject.activeSelf))
            _trEnemy = null;
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

            default:
                _animator.SetTrigger(_animIdle);
                break;         
        }
    }
}
