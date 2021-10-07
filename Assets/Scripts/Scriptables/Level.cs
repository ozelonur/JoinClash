
using UnityEngine;
using Sirenix.OdinInspector;
[CreateAssetMenu(fileName = "Level", menuName = "Level", order =1)]
public class Level : ScriptableObject
{
    [SerializeField, Required("Level prefab is required!")] private GameObject levelPrefab;

    public GameObject LevelPrefab { get => levelPrefab; set => levelPrefab = value; }
}
