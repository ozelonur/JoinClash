using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ozel;

public class Enemies : MonoBehaviour
{
    [SerializeField, ReadOnly] private List<GameObject> enemies;

    private PlayerController playerController;

    private Collider unitCollider;

    private Run run;

    public List<GameObject> EnemiesList { get => enemies; set => enemies = value; }

    private void Start()
    {
        playerController = PlayerController.Instance;
        GetComponents();
        enemies = new List<GameObject>();
        for (int i = 0; i < transform.childCount; i++)
        {
            enemies.Add(transform.GetChild(i).gameObject);
        }
    }

    private void GetComponents()
    {
        unitCollider = GetComponent<Collider>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(Constants.PERSON))
        {
            if (!playerController.TriggeredPersonList.Contains(other.gameObject))
            {
                playerController.TriggeredPersonList.Add(other.gameObject);
            }

            if (run != null)
            {
                run.Abort();
            }

            Run.After(.5f, () =>
            {
                other.GetComponent<IEnemyHit>()?.EnemyHit(enemies);
                Destroy(unitCollider);
            });
        }
    }
}
