using Sirenix.OdinInspector;
using UnityEngine;

public class PersonMovement : BaseController
{
    private PersonController personController;
    private PlayerMovement playerMovement;
    private GameManager gameManager;

    [SerializeField, ReadOnly] private float personSpeed = 0;
    [SerializeField, ReadOnly] private bool hasOpponent = false;

    private Rigidbody personRigidbody;

    public bool HasOpponent { get => hasOpponent; set => hasOpponent = value; }
    public Rigidbody PersonRigidbody { get => personRigidbody; set => personRigidbody = value; }

    protected override void Start()
    {
        base.Start();
        GetInstances();
        GetComponents();
        personSpeed = playerMovement.PlayerSpeed - 1f;
    }

    private void FixedUpdate()
    {
        if (gameManager.CurrentGameState == GameState.Playing)
        {
            if (personController.HitToPlayer && !personController.InBattle)
            {
                float distance = Vector3.Distance(playerMovement.transform.position, transform.position);

                if (distance > 1f)
                {
                    transform.LookAt(playerMovement.transform);
                    personSpeed = playerMovement.PlayerSpeed + .1f;
                }
                else
                {
                    transform.localRotation = playerMovement.transform.localRotation;
                    personSpeed = playerMovement.PlayerSpeed;

                }
                if (playerMovement.Hold)
                {
                    personController.StartPhysics();
                    personRigidbody.velocity = Vector3.Lerp(personRigidbody.velocity, transform.forward * personSpeed, .3f);
                    RunAnim();
                }
                else if (!playerMovement.Hold && !personController.InBattle)
                {
                    personRigidbody.velocity = Vector3.zero;
                    Idle();
                }
            }

        }
        
    }

    private void GetComponents()
    {
        personController = GetComponent<PersonController>();
        PersonRigidbody = GetComponent<Rigidbody>();
        
    }

    private void GetInstances()
    {
        playerMovement = PlayerMovement.Instance;
        gameManager = GameManager.Instance;
    }

    

}
