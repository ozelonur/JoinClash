using UnityEngine;
using Sirenix.OdinInspector;
using DG.Tweening;

public class EnemyController : BaseController
{
    private GameManager gameManager;
    private MaterialManager materialManager;
    [SerializeField, Required("Enemy settings is required!")] private EnemySettings enemySettings;

    [SerializeField, ReadOnly] private float health = 0;
    [SerializeField, ReadOnly] private bool isDead = false;
    [SerializeField, ReadOnly] private bool inBattle = false;

    [SerializeField, ReadOnly] private Transform opponent;

    private Battle opponentBattle;
    private PlayerController playerController;
    private Transform closestOpponent;

    private Collider enemyCollider;
    private Rigidbody enemyRigidbody;

    public float Health { get => health; set => health = value; }
    public bool IsDead { get => isDead; set => isDead = value; }
    public bool InBattle { get => inBattle; set => inBattle = value; }

    protected override void Start()
    {
        base.Start();
        GetInstances();
        GetComponents();
        if (!transform.parent.name.Contains(Constants.ENEMIES))
        {
            gameManager.EnemyList.Add(GetComponent<EnemyController>());
            health = enemySettings.EnemyHealth;
        }
    }

    private void GetComponents()
    {
        enemyCollider = GetComponent<Collider>();
        enemyRigidbody = GetComponent<Rigidbody>();
    }
    public void Attack(Transform opponent)
    {
        this.opponent = opponent;
        opponentBattle = this.opponent.GetComponent<Battle>();
        enemyCollider.material = materialManager.DefaultMaterial;
        enemyRigidbody.isKinematic = true;
        enemyRigidbody.velocity = Vector3.zero;

        Vector3 battlePoint = (transform.position + opponent.transform.position) / 2;
        RunAnim();
        transform.LookAt((battlePoint - ((battlePoint - transform.position).normalized * .45f)));
        transform.DOMove((battlePoint - ((battlePoint - transform.position).normalized * .45f)), 3f).OnComplete(() => 
        {
            Kick();
        });
        inBattle = true;
    }

    public void CheckInBattle()
    {
        if (!inBattle)
        {
            if (CheckMyParent())
            {
                int index = Random.Range(0, playerController.TriggeredPersonList.Count);
                closestOpponent = playerController.TriggeredPersonList[index].transform;
            }
            else
            {
                int index = Random.Range(0, playerController.PersonList.Count);
                closestOpponent = playerController.PersonList[index].transform;
            }

            this.opponent = closestOpponent;
            opponentBattle = this.opponent.GetComponent<Battle>();

            Vector3 battlePoint = (transform.position + closestOpponent.transform.position) / 2;
            Vector3 regularizedBattlePoint = battlePoint - ((battlePoint - transform.position).normalized * .25f);

            RunAnim();
            transform.LookAt(regularizedBattlePoint);
            transform.DOMove(regularizedBattlePoint, 3f).OnComplete(() =>
            {
                Kick();
            });
            inBattle = true;
        }
    }

    private void GetInstances()
    {
        gameManager = GameManager.Instance;
        playerController = PlayerController.Instance;
        materialManager = MaterialManager.Instance;
    }

    private void GiveDamage()
    {
        if (opponent != null)
        {
            if (opponentBattle)
            {
                if (opponentBattle.Health > 10)
                {
                    opponentBattle.TakeDamage();
                }
                if (opponentBattle.Health <= 10)
                {
                    if (playerController.PersonList.Count > 0)
                    {
                        inBattle = false;
                        CheckInBattle();
                    }
                    else
                    {
                        Idle();
                    }
                }
            }
        }
    }

    public void TakeDamage()
    {
        Hit();
        Invoke(Constants.KICK, 1f);
       
        if (health > 0)
        {
            health -= 20;
        }

        if (health <= 0)
        {
            isDead = true;
            Dead();
           
        }
    }

    private bool CheckMyParent()
    {
        if (transform.parent.name.Contains(Constants.ENEMIES))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

}
