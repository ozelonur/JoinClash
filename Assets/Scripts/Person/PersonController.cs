using Sirenix.OdinInspector;
using UnityEngine;

public class PersonController : Battle, IPlayer, IObstacleHit
{
    [SerializeField, ReadOnly] private bool hitToPlayer = false;
    [SerializeField, ReadOnly] private bool isCollided = false;


    private PlayerController playerController;

    public bool HitToPlayer { get => hitToPlayer; set => hitToPlayer = value; }

    protected override void Start()
    {
        base.Start();
        GetInstances();

    }

    protected override void GetInstances()
    {
        base.GetInstances();
        playerController = PlayerController.Instance;
    }


    private void OnCollisionEnter(Collision collision)
    {
        collision.gameObject.GetComponent<IPlayer>()?.PlayerHit();

    }
    public void PlayerHit()
    {
        if (!isCollided)
        {
            playerController.PersonList.Add(gameObject);
            GetComponentInChildren<Renderer>().material.color = Color.yellow;
            hitToPlayer = true;
            isCollided = true;
        }
    }

    public void HitToObstacle()
    {
        Dead();
        playerController.PersonList.Remove(gameObject);
    }

   

}
