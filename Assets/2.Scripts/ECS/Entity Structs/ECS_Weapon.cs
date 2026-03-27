using Unity.Entities;

public struct WeaponTag : IComponentData { }
public struct WeaponEnabledTag : IComponentData, IEnableableComponent { }

public struct FireRate : IComponentData
{
    public float coolDown;
    public float timer;
}

public struct ManualAimTag : IComponentData { }
public struct AutoTargetTag : IComponentData { }
public struct RandomAimTag : IComponentData { }

public struct ProjectilePrefab : IComponentData
{
    public Entity projectileEntity;
}

public struct WeaponPrefab : IBufferElementData
{
    public Entity weaponEntity;
}

public struct WeaponSpawnerTag : IComponentData { }