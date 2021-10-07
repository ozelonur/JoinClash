using Sirenix.OdinInspector;
using UnityEngine;
using DG.Tweening;
using System;
using System.Collections.Generic;
using Ozel;

public class Battle : BaseController, IFinish, IEnemyHit, IDiamondCollect
{
    [SerializeField, ReadOnly] private Transform closestEnemy;
    [SerializeField, ReadOnly] private bool onFinish = false;
    [SerializeField, ReadOnly] private float health = 0;
    [SerializeField, ReadOnly] private bool inBattle = false;

    protected GameManager gameManager;
    private CanvasManager canvasManager;
    private MaterialManager materialManager;
    private ObjectManager objectManager;
    private PlayerController playerController;
    private EnemyController enemyController;
    private Action findEnemyAction;

    private bool isCollided = false;

    private List<GameObject> enemies;

    private Rigidbody unitRigidbody;
    private Collider unitCollider;

    public float Health { get => health; set => health = value; }
    public bool InBattle { get => inBattle; set => inBattle = value; }

    protected override void Start()
    {
        base.Start();
        GetInstances();
        GetComponents();
        health = PlayerMovement.Instance.PlayerSettings.PlayerHealth;

        enemies = new List<GameObject>();

        findEnemyAction += StopPhysics;
        findEnemyAction += FindEnemy;
        findEnemyAction += AttackToEnemy;
    }

    protected virtual void GetInstances()
    {
        gameManager = GameManager.Instance;
        canvasManager = CanvasManager.Instance;
        playerController = PlayerController.Instance;
        materialManager = MaterialManager.Instance;
        objectManager = ObjectManager.Instance;
    }

    private void GetComponents()
    {
        unitRigidbody = GetComponent<Rigidbody>();
        unitCollider = GetComponent<Collider>();
    }

    public virtual void TakeDamage()
    {
        Hit();
        Invoke(Constants.KICK, 1f);
        if (health > 0)
        {
            health -= 10;
        }

        if (health <= 0)
        {
            Dead();
        }
    }

    public void Finish()
    {
        if (!onFinish)
        {
            gameManager.CurrentGameState = GameState.InBattle;
            foreach (var enemy in gameManager.EnemyList)
            {
                enemy.CheckInBattle();
            }
            findEnemyAction();
            onFinish = true;
        }
    }

    private void StopPhysics()
    {
        unitCollider.material = materialManager.DefaultMaterial;
        unitRigidbody.isKinematic = true;
        unitRigidbody.velocity = Vector3.zero;
    }

    public void StartPhysics()
    {
        isCollided = false;
        unitCollider.material = materialManager.NoFriction;
        unitRigidbody.isKinematic = false;
        unitCollider.isTrigger = false;
    }

    private void FindEnemy()
    {
        if (!isCollided)
        {
            int index = UnityEngine.Random.Range(0, gameManager.EnemyList.Count);
            closestEnemy = gameManager.EnemyList[index].transform;
        }

        else
        {
            int index = UnityEngine.Random.Range(0, enemies.Count);
            closestEnemy = enemies[index].transform;
        }

        enemyController = closestEnemy.GetComponent<EnemyController>();

        if (!enemyController.InBattle)
        {
            enemyController.Attack(transform);
        }
    }

    private void AttackToEnemy()
    {
        inBattle = true;
        Vector3 battlePoint = (closestEnemy.transform.position + transform.position) / 2;
        Vector3 regularizedBattlePoint = battlePoint - ((battlePoint - transform.position).normalized * .45f);
        RunAnim();
        transform.LookAt(regularizedBattlePoint);
        transform.DOMove(regularizedBattlePoint, 3f).OnComplete(() =>
        {
            unitCollider.isTrigger = true;
            Kick();
        });

    }

    private void GiveDamage()
    {
        if (closestEnemy != null)
        {
            if (enemyController.Health > 20)
            {
                enemyController.TakeDamage();
            }

            if (enemyController.Health <= 20)
            {
                enemyController.TakeDamage();

                if (!isCollided && gameManager.EnemyList.Contains(enemyController))
                {
                    gameManager.EnemyList.Remove(enemyController);
                }
                else
                {
                    Enemies enemies = enemyController.transform.parent.transform.GetComponent<Enemies>();

                    if (enemies.EnemiesList.Contains(enemyController.gameObject))
                    {
                        enemies.EnemiesList.Remove(enemyController.gameObject);
                    }
                }

                Idle();
                if (!isCollided)
                {
                    if (gameManager.EnemyList.Count > 0)
                    {
                        unitCollider.isTrigger = false;
                        findEnemyAction();
                    }
                }
                else
                {
                    if (enemies.Count > 0)
                    {
                        unitCollider.isTrigger = false;
                        findEnemyAction();
                    }
                    else
                    {
                        CloseAllPlayersInBattle();
                    }
                }

            }
        }
        else
        {
            Idle();

            if (!isCollided)
            {
                if (gameManager.EnemyList.Count > 0)
                {
                    unitCollider.isTrigger = false;
                    findEnemyAction();
                }
            }
            else
            {
                if (enemies.Count > 0)
                {
                    unitCollider.isTrigger = false;
                    findEnemyAction();
                }
                else
                {
                    CloseAllPlayersInBattle();
                }
            }

        }
    }

    protected override void Idle()
    {
        base.Idle();
        CheckIsGameComplete();
    }

    private void CheckIsGameComplete()
    {
        if (gameManager.EnemyList.Count <= 0)
        {
            playerController.GameCompletePlayer();
        }
    }

    public void RunToPalace()
    {
        RunAnim();
    }

    public void DanceOnEnd()
    {
        Dance();
    }

    public void EnemyHit(List<GameObject> enemies)
    {
        isCollided = true;
        this.enemies = enemies;
        findEnemyAction();

        foreach (var enemy in enemies)
        {
            enemy.GetComponent<EnemyController>().CheckInBattle();
        }
    }

    private void CloseAllPlayersInBattle()
    {
        foreach (var person in playerController.PersonList)
        {
            person.GetComponent<Battle>().InBattle = false;
        }
    }

    public void DiamondCollect(GameObject diamond)
    {
        Vector3 diamondPos = Camera.main.WorldToScreenPoint(diamond.transform.position);
        diamond.transform.parent = canvasManager.transform;
        diamond.transform.position = objectManager.OrthographicCamera.ScreenToWorldPoint(diamondPos);
        
        diamond.layer = 5;
        diamond.transform.DOKill();
        Run.After(.05f,() =>
        {
            diamond.transform.DOMove(canvasManager.DiamondText.transform.position, 1f).OnComplete(() => {
                Destroy(diamond);
                gameManager.DiamondCount++;
                canvasManager.UpdateDiamondText();
            });
        });
       

    }

    protected override void Dead()
    {
        base.Dead();
        if (gameObject.name.Contains(Constants.PLAYER))
        {
            gameManager.gameOver();
        }
    }
}
