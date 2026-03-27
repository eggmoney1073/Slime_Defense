using UnityEngine;

public class OOP_PathMaker : MonoBehaviour
{
    [SerializeField]
    Transform[] _pathArray;

    public Transform[] PathArray { get { return _pathArray; } }

    private void OnDrawGizmos()
    {
        if (_pathArray == null || _pathArray.Length < 2)
        {
            return;
        }

        Gizmos.color = Color.green;
        for (int i = 0; i < _pathArray.Length - 1; i++)
        {
            Gizmos.DrawLine(_pathArray[i].position, _pathArray[i + 1].position);
        }
    }
}
