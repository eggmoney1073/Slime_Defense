using System;
using Unity.VisualScripting;
using UnityEngine;

public class OOP_EnemySpawner : SingletonGameobject<OOP_EnemySpawner>
{
    [Header("Setting")]
    [SerializeField]
    float _spawnInterval = 1f;

    [Header("Reference")]
    [SerializeField]
    OOP_PathMaker _pathMaker;
    [SerializeField]
    GameObject _enemyPrefab;


    GameObjectPool<OOP_Enemy> _enemyPool;
    public GameObjectPool<OOP_Enemy> EnemyPool { get { return _enemyPool; } }

    float _checkTime = 0f;

    Vector3[] _pathPositions;
    public Vector3[] PathPositions { get { return _pathPositions; } }


    void Start()
    {
        // 이동 경로 위치 배열 초기화

        int pathCount = _pathMaker.PathArray.Length;
        _pathPositions = new Vector3[pathCount];

        for (int i = 0; i < pathCount; i++)
        {
            _pathPositions[i] = _pathMaker.PathArray[i].position;
        }

        // 적 풀 초기화

        _enemyPool = new GameObjectPool<OOP_Enemy>(10, () =>
        {
            GameObject enemyObject = Instantiate(_enemyPrefab);
            OOP_Enemy enemy = enemyObject.GetComponent<OOP_Enemy>();
            enemy.Initialize();
            enemyObject.transform.SetParent(transform);
            enemyObject.SetActive(false);
            return enemy;
        });
    }

    void Update()
    {
        _checkTime += Time.deltaTime;

        if (_checkTime > _spawnInterval)
        {
            // Spawn enemy
            _enemyPool.Get().ResetEnemy();

            _checkTime = 0f;
        }
    }
}
