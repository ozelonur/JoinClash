using Sirenix.OdinInspector;
using UnityEngine;
using Ozel;

public class MaterialManager : Singleton<MaterialManager>
{
    [SerializeField, Required("Person Material is required!")] private Material personMaterial;
    [SerializeField, Required("Default Material is required!")] private PhysicMaterial defaultFriction;
    [SerializeField, Required("No Friction Material is required!")] private PhysicMaterial noFriction;

    public Material PersonMaterial { get => personMaterial; set => personMaterial = value; }
    public PhysicMaterial DefaultMaterial { get => defaultFriction; set => defaultFriction = value; }
    public PhysicMaterial NoFriction { get => noFriction; set => noFriction = value; }

    private void Start()
    {
        personMaterial.color = Color.white;
    }
}
