using System.Diagnostics;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public partial struct Test_MoveSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<Test_Data>();
    }

    public void OnUpdate(ref SystemState state)
    {
        float deltaTime = SystemAPI.Time.DeltaTime;

        foreach (var (transform, data)
            in SystemAPI.Query<RefRW<LocalTransform>, RefRO<Test_Data>>())
        {
            float3 moveDelta = data.ValueRO.direction * data.ValueRO.speed * deltaTime;
            transform.ValueRW.Position += moveDelta;
            Debug.Write(moveDelta);
        }
    }
}