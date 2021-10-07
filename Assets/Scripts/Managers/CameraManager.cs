using Sirenix.OdinInspector;
using UnityEngine;
using DG.Tweening;
using Ozel;

public class CameraManager : Singleton<CameraManager>
{
    [SerializeField, Required("Player is required!")] private GameObject player;
    private GameManager gameManager;


    private Vector3 offset;
    private void Start()
    {
        gameManager = GameManager.Instance;
        gameManager.gameComplete += GameComplete;
        offset = new Vector3(player.transform.position.x, transform.position.y, transform.position.z - player.transform.position.z);
    }
    private void LateUpdate()
    {
        if (gameManager.CurrentGameState == GameState.Playing || gameManager.CurrentGameState == GameState.InBattle)
        {
            transform.position = player.transform.position + offset;
        }
    }

    private void GameComplete()
    {
        transform.DOMoveX(0, 3.5f);
    }

}
