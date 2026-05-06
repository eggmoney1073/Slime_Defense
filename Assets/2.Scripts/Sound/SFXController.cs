using Unity.Entities;
using UnityEngine;

public class SFXController : MonoBehaviour
{
    [Header("Sound Settings")]
    [SerializeField] private float _hitSoundCooldown = 0.1f;
    [SerializeField] private float _deathSoundCooldown = 0.5f;

    private EntityManager _entityManager;
    private Entity _soundBufferEntity;
    private bool _isReady;

    private float _nextHitSoundTime = 0f;
    private float _nextDeathSoundTime = 0f;

    void Awake()
    {
        World world = World.DefaultGameObjectInjectionWorld;
        if (world == null) return;

        _entityManager = world.EntityManager;
    }

    void Start()
    {
        TryBindSoundBuffer();
    }

    // LateUpdate는 모든 시스템이 업데이트된 후에 실행되므로, 시스템에서 SoundRequest 버퍼에 요청을 추가한 후에 처리할 수 있습니다.
    void LateUpdate()
    {
        if (!_isReady)
        {
            TryBindSoundBuffer();
            if (!_isReady) return;
        }

        if (!_entityManager.Exists(_soundBufferEntity)) { _isReady = false; return; }

        DynamicBuffer<SoundRequest> buffer = _entityManager.GetBuffer<SoundRequest>(_soundBufferEntity);
        if (buffer.Length == 0) return;

        bool playHit = false;
        bool playDeath = false;

        for (int i = 0; i < buffer.Length; i++)
        {
            switch (buffer[i].SFXType)
            {
                case SoundManager.SFXType.EnemyHit:
                    playHit = true;
                    break;
                case SoundManager.SFXType.EnemyDeath:
                    playDeath = true;
                    break;
            }

            if (playHit && playDeath) break;
        }

        if (playHit && Time.time >= _nextHitSoundTime)
        {
            SoundManager.Instance.PlaySFX(SoundManager.SFXType.EnemyHit);
            _nextHitSoundTime = Time.time + _hitSoundCooldown;
        }
        if (playDeath && Time.time >= _nextDeathSoundTime)
        {
            SoundManager.Instance.PlaySFX(SoundManager.SFXType.EnemyDeath);
            _nextDeathSoundTime = Time.time + _deathSoundCooldown;
        }

        buffer.Clear();
    }

    private void TryBindSoundBuffer()
    {
        if (_entityManager == default) return;

        EntityQuery entityQuery = _entityManager.CreateEntityQuery(ComponentType.ReadWrite<SoundRequest>());
        if (entityQuery.IsEmptyIgnoreFilter) return;

        _soundBufferEntity = entityQuery.GetSingletonEntity();
        _isReady = true;
    }
}