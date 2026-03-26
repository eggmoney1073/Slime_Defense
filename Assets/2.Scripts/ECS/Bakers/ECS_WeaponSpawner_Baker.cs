using Unity.Entities;
using UnityEngine;

class ECS_WeaponSpawner_Baker : Baker<WeaponSpawner_Authoring>
{
    public override void Bake(WeaponSpawner_Authoring authoring)
    {
        for(int i = 0; i < authoring.WeaponPrefabs.Length; i++)
        {
            Entity weaponPrefab = GetEntity(authoring.WeaponPrefabs[i], TransformUsageFlags.Dynamic);

            AddComponent(weaponPrefab, new WeaponTag { });
            SetComponentEnabled<WeaponTag>(weaponPrefab, false);
        }
    }
}
