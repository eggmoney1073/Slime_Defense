using Unity.Entities;
using UnityEngine;

class WeaponSpawner_Baker : Baker<WeaponSpawner_Authoring>
{
    public override void Bake(WeaponSpawner_Authoring authoring)
    {
        Entity entity = GetEntity(TransformUsageFlags.None);
        AddComponent<WeaponSpawnerTag>(entity);

        DynamicBuffer<WeaponPrefab> weaponPrefabs = AddBuffer<WeaponPrefab>(entity);

        for (int i = 0; i < authoring.WeaponPrefabs.Length; i++)
        {
            Entity weaponPrefab = GetEntity(authoring.WeaponPrefabs[i], TransformUsageFlags.Dynamic);

            weaponPrefabs.Add(new WeaponPrefab
            {
                weaponEntity = weaponPrefab
            });
        }
    }
}
