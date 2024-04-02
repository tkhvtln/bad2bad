using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerConfig _playerConfig;

    [Space]
    [SerializeField] private Weapon _weapon;

    [Space]
    [SerializeField] private Transform _trWeapon;
    [SerializeField] private Transform _trsRadiusAttack;

    [Space]
    [SerializeField] private Joystick _joystick;
    [SerializeField] private Button _buttonFire;

    private Transform _trEnemy;
    private Transform _transform;
    private Vector3 _vecInput;

    private Rigidbody2D _rb;
    private Animator _animator;

    private bool _isFire;
    private bool _isFaceLeft;

    private int _animIdle;
    private int _animWalk;  

    private Behavior _behavior;

    public void Init()
    {
        _transform = transform;
        _rb = GetComponent<Rigidbody2D>();
        _trsRadiusAttack.localScale = Vector2.one * _playerConfig.RadiusAttack / 3;

        _animator = GetComponent<Animator>();
        _animIdle = Animator.StringToHash(Constants.ANIM_IDLE);
        _animWalk = Animator.StringToHash(Constants.ANIM_WALK);

        Fire();
        GameController.OnGame.AddListener(() => _weapon.Init());
    }

    public void ResetPlayer()
    {
        _isFire = false;

        _transform.position = Vector2.zero;
        _transform.eulerAngles = Vector2.zero;
        _trWeapon.eulerAngles = Vector2.zero;
    }

    private void Update()
    {
        Detect();
        Look();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Fire()
    {
        _buttonFire.OnPointerUpAsObservable().Subscribe(_ => _isFire = false);
        _buttonFire.OnPointerDownAsObservable().Subscribe(_ => _isFire = true);

        Observable.IntervalFrame(_weapon.RateOfFire)
            .Where(_ => _isFire && _trEnemy != null)
            .Subscribe(_ => _weapon.Fire());
    }

    private void Move()
    {
        _vecInput = new Vector2(_joystick.Horizontal, _joystick.Vertical);
        _rb.velocity = _vecInput * _playerConfig.SpeedMove * Time.fixedDeltaTime;

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
            Collider2D[] colliderTargets = Physics2D.OverlapCircleAll(_trsRadiusAttack.position, _playerConfig.RadiusAttack, LayerMask.GetMask(Constants.LAYER_ENEMY));

            if (colliderTargets.Length > 0 && colliderTargets[0].gameObject.activeSelf)
                _trEnemy = colliderTargets[0].transform;
        }

        if (_trEnemy != null && (Vector2.Distance(_trsRadiusAttack.position, _trEnemy.position) > _playerConfig.RadiusAttack || !_trEnemy.gameObject.activeSelf))
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
