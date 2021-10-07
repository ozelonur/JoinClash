using Sirenix.OdinInspector;
using UnityEngine;
using Ozel;

public class Palace : Singleton<Palace>
{
    [SerializeField, Required("Palace door required!")] private Transform palaceDoor;
    [SerializeField, Required("King is required!")] private GameObject king;

    private AnimationManager animationManager;

    public Transform PalaceDoor { get => palaceDoor; }
    public GameObject King { get => king; set => king = value; }

    private bool isKingKicked = false;

    private void Start()
    {
        animationManager = AnimationManager.Instance;
    }

    public void KickKing()
    {
        if (!isKingKicked)
        {
            animationManager.SetAnimation(king.GetComponent<Animator>(), AnimationState.Fly);
            isKingKicked = true;
            Rigidbody kingRigidbody = King.AddComponent<Rigidbody>();
            kingRigidbody.useGravity = true;
            kingRigidbody.AddForce(new Vector3(0, 750, 335f));
           
        }
    }

}
