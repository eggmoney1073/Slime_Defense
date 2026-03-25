using Unity.Entities;

public struct WeaponTag { }

public struct FireRate
{
    public float coolDown;
    public float timer;
}

public struct ManualAimTag { }
public struct AutoTargetTag { }
public struct RandomAimTag { }

public struct ProjectilePrefab
{
    public Entity prefab;
}