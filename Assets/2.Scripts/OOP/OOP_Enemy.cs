using UnityEngine;

public class OOP_Enemy : MonoBehaviour
{
    [SerializeField]
    float _moveSpeed = 2f;

    Vector3[] _nodePositions;
    Vector3 _currentTargetNodePosition;

    int _currentNodeIndex = 1;

    void Update()
    {
        // 목표로 이동
        transform.position = Vector3.MoveTowards(transform.position, _currentTargetNodePosition, _moveSpeed * Time.deltaTime);

        // 목표에 가까워지면 다음 목표로 변경
        if (Vector3.Distance(transform.position, _currentTargetNodePosition) < 0.1f)
        {
            _currentNodeIndex++;
            if(_currentNodeIndex >= _nodePositions.Length)
            {
                OOP_EnemySpawner.Instance.EnemyPool.Set(this);
                gameObject.SetActive(false);
                return;
            }
            _currentTargetNodePosition = _nodePositions[_currentNodeIndex];
        }
    }

    public void Initialize()
    {
        _nodePositions = new Vector3[OOP_EnemySpawner.Instance.PathPositions.Length];
        _nodePositions = OOP_EnemySpawner.Instance.PathPositions;
    }

    public void ResetEnemy()
    {
        gameObject.SetActive(true);
        transform.position = _nodePositions[0];
        _currentNodeIndex = 1;
        _currentTargetNodePosition = _nodePositions[_currentNodeIndex];
    }
}
