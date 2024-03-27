using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private Transform _trWeapon;
    [SerializeField] private Joystick _joystick;
    [SerializeField] private Animator _animator;

    private Rigidbody2D _rb;
    private Transform _transform;
    private Vector3 _vecDirection;

    public void Init()
    {
        _transform = transform;
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        GetInput();
        Flip();
        Aim();
        AnimateLegs();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void GetInput()
    {
        _vecDirection = new Vector2(_joystick.Horizontal, _joystick.Vertical);
    }

    private void Move()
    {
        _rb.velocity = _vecDirection * _speed * Time.fixedDeltaTime;
    }

    private void Aim()
    {
        float flip = _vecDirection.x < 0 ? -90 : 90;
        float angle = _vecDirection.x + _vecDirection.y * flip;
        _trWeapon.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    private void Flip()
    {
        if (_vecDirection.x < 0)
            _transform.localScale = new Vector2(-1, 1);
        else if (_vecDirection.x > 0)
            _transform.localScale = new Vector2(1, 1); ;
    }

    private void AnimateLegs()
    {
        _animator.enabled = _vecDirection != Vector3.zero ? true : false;
    }
}
