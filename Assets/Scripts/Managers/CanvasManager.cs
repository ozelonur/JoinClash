using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.UI;
using Ozel;

public class CanvasManager : Singleton<CanvasManager>
{
    [SerializeField, Required("This GameObject is required.")] private GameObject tapPanel;
    [SerializeField, Required("This Text is required.")] private Text tapText;
    [SerializeField, Required("Diamond Text is required.")] private Text diamondText;

    private GameManager gameManager;
    private Camera UICamera;

    public GameObject TapPanel { get => tapPanel; set => tapPanel = value; }
    public Text TapText { get => tapText; set => tapText = value; }
    public Text DiamondText { get => diamondText; set => diamondText = value; }

    private void Start()
    {
        gameManager = GameManager.Instance;
        UICamera = ObjectManager.Instance.OrthographicCamera;
        gameManager.gameOver += GameOver;
        gameManager.gameComplete += GameComplete;
        
        tapText.text = Constants.TAP_TO_PLAY;
        UpdateDiamondText();
    }

    private void GameOver()
    {
        tapText.text = Constants.TRY_AGAIN;
        tapPanel.SetActive(true);
    }

    private void GameComplete()
    {
        tapText.text = Constants.NEXT_LEVEL;
        tapPanel.SetActive(true);
    }

    public void UpdateDiamondText()
    {
        DiamondText.text = PlayerPrefs.GetInt(Constants.DIAMOND, 0).ToString();
    }

    public Vector3 GetDiamondUIPosition()
    {
        Vector3 diamondPosition = UICamera.ScreenToWorldPoint(DiamondText.transform.position);
        return diamondPosition;
    }

}
