using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform _trTarget;
    [SerializeField] private Vector2 _vecOffset;

    private Transform _transform;

    public void Start()
    {
        _transform = transform;
    }

    private void Update()
    {
        _transform.position = new Vector3(_trTarget.position.x + _vecOffset.x, _trTarget.position.y + _vecOffset.y, _transform.position.z);
    }

    private void OnValidate()
    {
        transform.position = new Vector3(_trTarget.position.x + _vecOffset.x, _trTarget.position.y + _vecOffset.y, transform.position.z);
    }
}
