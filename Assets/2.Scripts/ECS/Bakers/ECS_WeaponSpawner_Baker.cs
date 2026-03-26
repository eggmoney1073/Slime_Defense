using Unity.Entities;
using UnityEngine;

class ECS_WeaponSpawner_Baker : Baker<WeaponSpawner_Authoring>
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
                weapon = weaponPrefab
            });
        }

        Debug.Log("무기 스포너 베이커 완료");
    }
}
