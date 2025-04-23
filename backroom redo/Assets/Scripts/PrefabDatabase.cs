using UnityEngine;

[CreateAssetMenu(fileName = "PrefabDatabase", menuName = "Scriptable Objects/PrefabDatabase")]
public class PrefabDatabase : ScriptableObject
{
    public GameObject[] prefabList; // Array of prefabs to store in the database
}
