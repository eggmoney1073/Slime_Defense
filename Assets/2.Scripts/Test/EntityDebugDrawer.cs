using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public class EntityDebugDrawer : MonoBehaviour
{
    private EntityManager _entityManager;
    private EntityQuery _query;

    private void Start()
    {
        _entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

        _query = _entityManager.CreateEntityQuery(typeof(LocalTransform));
    }

    private void OnDrawGizmos()
    {
        if (_entityManager == default) return;

        var entities = _query.ToEntityArray(Unity.Collections.Allocator.Temp);
        var transforms = _query.ToComponentDataArray<LocalTransform>(Unity.Collections.Allocator.Temp);

        for (int i = 0; i < entities.Length; i++)
        {
            Vector3 pos = transforms[i].Position;
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(pos, 0.2f);
        }

        entities.Dispose();
        transforms.Dispose();
    }
}