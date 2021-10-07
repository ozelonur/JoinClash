using UnityEngine;
using Sirenix.OdinInspector;
using Ozel;
public class ObjectManager : Singleton<ObjectManager>
{
    [SerializeField, Required("Orthographic Camera Prefab is required!")] private Camera orthographicCamera;

    public Camera OrthographicCamera { get => orthographicCamera; set => orthographicCamera = value; }
}
