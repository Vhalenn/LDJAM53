using UnityEngine;
using UnityEditor;

public class HandleVisualzier : MonoBehaviour
{
    [SerializeField] private GizmoType type;
    [SerializeField] private Vector3 size = Vector3.one * 5;
    [SerializeField] private Color color;

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Handles.color = color;

        if (type == GizmoType.WireDisc)
        {
            Handles.DrawWireDisc(transform.position, Vector3.up, size.x);
        }
        else if (type == GizmoType.WireSquare)
        {
            Matrix4x4 rotationMatrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
            Handles.matrix = rotationMatrix;
            Handles.DrawWireCube(Vector3.zero, size);
        }
    }
#endif

    public enum GizmoType
    {
        WireDisc,
        WireSquare,
        Line,
    }

}
