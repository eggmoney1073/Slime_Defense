using Unity.Entities;
using UnityEngine;

public class SFXController : MonoBehaviour
{
    [Header("Sound Settings")]
    [SerializeField] private float _hitSoundCooldown = 0.1f;
    [SerializeField] private float _deathSoundCooldown = 0.5f;

    private float _nextHitSoundTime = 0f;
    private float _nextDeathSoundTime = 0f;

    void LateUpdate()
    {
        World world = World.DefaultGameObjectInjectionWorld;

        if (world == null || !world.IsCreated)
            return;

        EntityManager entityManager = world.EntityManager;

        EntityQuery query = entityManager.CreateEntityQuery(ComponentType.ReadWrite<SoundRequest>());

        if (query.IsEmptyIgnoreFilter)
        {
            query.Dispose();
            return;
        }

        Entity soundBufferEntity = query.GetSingletonEntity();
        query.Dispose();

        if (!entityManager.Exists(soundBufferEntity))
            return;

        DynamicBuffer<SoundRequest> buffer = entityManager.GetBuffer<SoundRequest>(soundBufferEntity);

        if (buffer.Length == 0)
            return;

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

            if (playHit && playDeath)
                break;
        }

        if (SoundManager.Instance == null)
        {
            buffer.Clear();
            return;
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
}