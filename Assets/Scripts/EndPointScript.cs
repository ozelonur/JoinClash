using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndPointScript : MonoBehaviour
{
    private GameManager gameManager;

    void Start()
    {
        GetInstances();
    }

    private void GetInstances()
    {
        gameManager = GameManager.Instance;
    }

    private void OnTriggerEnter(Collider other)
    {
        gameManager.CurrentGameState = GameState.InBattle;
        other.gameObject.GetComponent<IFinish>().Finish();
    }
}
