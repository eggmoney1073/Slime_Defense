using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class Test_Baker : Baker<Test_Authoring>
{
    public override void Bake(Test_Authoring authoring)
    {
        Debug.Log("BAKER EXECUTED: " + authoring.name);

        var entity = GetEntity(TransformUsageFlags.Dynamic);

        AddComponent(entity, new Test_Data
        {
            speed = authoring._speed,
            direction = (float3)authoring._direction
        });
    }
}
