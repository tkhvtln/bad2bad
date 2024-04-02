using UnityEngine;

public class CameraController : MonoBehaviour
{
    #region FIELDS SERIALIZED

    [SerializeField] private Transform _trTarget;
    [SerializeField] private Vector2 _vecOffset;

    #endregion

    #region FIELDS

    private Transform _transform;

    #endregion 

    #region UNITY

    private void Update()
    {
        _transform.position = new Vector3(_trTarget.position.x + _vecOffset.x, _trTarget.position.y + _vecOffset.y, _transform.position.z);
    }

    private void OnValidate()
    {
        transform.position = new Vector3(_trTarget.position.x + _vecOffset.x, _trTarget.position.y + _vecOffset.y, transform.position.z);
    }

    #endregion

    #region METODS

    public void Init()
    {
        _transform = transform;
    }

    #endregion
}
