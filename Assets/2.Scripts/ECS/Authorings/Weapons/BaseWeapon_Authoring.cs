using UnityEngine;

public class BaseWeapon_Authoring : MonoBehaviour
{
    public DefineEnums.WeaponType weaponType;
    public GameObject projectilePrefab;
    public float cooldown;
    public float damage;
    public int pierceCount;
    public int projectileCount;
}
