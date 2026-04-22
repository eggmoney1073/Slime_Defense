using UnityEngine;
using DefineEnums;

[CreateAssetMenu(fileName = "LevelUpOptionData", menuName = "Scriptable Objects/LevelUpOption")]
public class LevelUpOptionData : ScriptableObject
{
    public WeaponType weaponType;
    public SelectionRarity rarity;
    public WeaponUpgradeType upgradeType;
    public float value;
    public string description;
}