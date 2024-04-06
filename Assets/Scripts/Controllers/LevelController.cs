using UniRx;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    [SerializeField] private Transform _trEnemies;

    private IntReactiveProperty _countEnemies;

    void Start()
    {
        GameController.instance.ControllerLevel = this;

        _countEnemies = new IntReactiveProperty(_trEnemies.childCount);
        _countEnemies.Where(_ => _countEnemies.Value <= 0).Subscribe(x => GameController.instance.Win());

        foreach (Transform tr in _trEnemies)
            tr.GetComponent<Enemy>().Init(_countEnemies);
    }  
}
