using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform _trTarget;

    private Transform _transform;

    public void Init()
    {
        _transform = transform;
    }

    private void Update()
    {
        transform.position = new Vector3(_trTarget.position.x, _trTarget.position.y + 1, _transform.position.z);
    }
}
