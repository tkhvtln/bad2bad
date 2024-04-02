using UniRx;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    #region FIELDS SERIALIZED

    [SerializeField] private Transform _trEnemies;

    #endregion

    #region UNITY

    void Start()
    {
        GameController.Instance.ControllerLevel = this;

        _countEnemies = new IntReactiveProperty(_trEnemies.childCount);
        _countEnemies.Where(_ => _countEnemies.Value <= 0).Subscribe(x => GameController.Instance.Win());

        foreach (Transform tr in _trEnemies)
            tr.GetComponent<Enemy>().Init(_countEnemies);
    }

    #endregion

    #region FIELDS

    private IntReactiveProperty _countEnemies;

    #endregion
}
