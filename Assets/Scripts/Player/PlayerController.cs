using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerController : Battle, IObstacleHit
{
    public static PlayerController Instance = null;

    private BoxCollider playerCollider;

    [SerializeField, ReadOnly] private float startColliderSizeX = 0;
    [SerializeField, ReadOnly] private List<GameObject> personList;
    [SerializeField, ReadOnly] private List<GameObject> triggeredPersonList;

    private bool isCompleted = false;

    public List<GameObject> PersonList { get => personList; set => personList = value; }
    public List<GameObject> TriggeredPersonList { get => triggeredPersonList; set => triggeredPersonList = value; }

    private void Awake()
    {
        triggeredPersonList = new List<GameObject>();
        if (Instance == null)
        {
            Instance = this;
        }
    }

    protected override void Start()
    {
        base.Start();
        GetComponents();

        gameManager.gameComplete += GameCompletePlayer;

        personList = new List<GameObject>();
        personList.Add(gameObject);

        startColliderSizeX = playerCollider.size.x;
        playerCollider.size = new Vector3(400, playerCollider.size.y, playerCollider.size.z);
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(Constants.PERSON_TAG))
        {
            collision.gameObject.GetComponent<IPlayer>()?.PlayerHit();
            playerCollider.size = new Vector3(startColliderSizeX, playerCollider.size.y, playerCollider.size.z);
        }
       
    }


    private void GetComponents()
    {
        playerCollider = GetComponent<BoxCollider>();
    }

    public void HitToObstacle()
    {
        Dead();
    }

    protected override void Dead()
    {
        base.Dead();
        gameManager.CurrentGameState = GameState.Dead;
        gameManager.gameOver();
    }

    public void GameCompletePlayer()
    {
        if (!isCompleted)
        {
            isCompleted = true;
            bool isPalaceExploded = false;
            bool canLevelIncrement = true;
            personList.Add(gameObject);
            Vector3 palacePosition = Palace.Instance.PalaceDoor.transform.position;
            foreach (var person in personList)
            {
                person.GetComponent<Battle>().RunToPalace();
                person.transform.LookAt(palacePosition);
                person.transform.DOMoveZ(palacePosition.z, 3f).OnComplete(() =>
                {
                    if (!isPalaceExploded)
                    {
                        particleManager.RunSmashParticle(Palace.Instance.PalaceDoor);
                        Destroy(Palace.Instance.PalaceDoor.gameObject);
                        isPalaceExploded = true;
                    }
                    else
                    {
                        Palace.Instance.KickKing();
                        person.transform.DOMove(person.transform.position + new Vector3(Random.Range(-1f,1f), 0, Random.Range(-1f, 1f)), .6f).OnComplete(() =>
                        {
                            person.transform.LookAt(Camera.main.transform);
                            person.GetComponent<Battle>().DanceOnEnd();
                            if (canLevelIncrement)
                            {
                                canLevelIncrement = false;
                                Invoke(nameof(GameCompleted), 2f);
                            }
                        });
                    }

                });

            }

        }
        
    }

    private void GameCompleted()
    {
        gameManager.gameComplete();
        ParticleManager.Instance.RunSmashParticle(Palace.Instance.King.transform);
        Destroy(Palace.Instance.King);
    }

}
