using UnityEngine;
using UnityEditor;

public class ObjectToPrefab : MonoBehaviour
{
    [SerializeField] private GameObject yellowPrefab;
    [SerializeField] private GameObject greenPrefab;

    [SerializeField] private Transform newParent;

    [ContextMenu("Do Something")]
    public void ConvertObjectsToPrefab()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform tr = transform.GetChild(i);

            GameObject obj = null;
            if (tr.name.ToLower().Contains("yellow"))
            {
                obj = PrefabUtility.InstantiatePrefab(yellowPrefab, transform) as GameObject;
            }
            else
            {
                obj = PrefabUtility.InstantiatePrefab(greenPrefab, transform) as GameObject;
            }

            obj.transform.SetParent(newParent);

            obj.transform.position = tr.position;
            obj.transform.rotation = tr.rotation;
            obj.transform.localScale = tr.localScale;
        }
    }

}
