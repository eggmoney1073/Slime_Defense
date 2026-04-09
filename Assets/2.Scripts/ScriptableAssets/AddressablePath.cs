using UnityEngine;
using DefineEnums;

[CreateAssetMenu(fileName = "AddressablePath", menuName = "Scriptable Objects/AddressablePath")]
public class AddressablePath : ScriptableObject
{
    public string PrefabAddress;
    public AddressableType Type;
}
