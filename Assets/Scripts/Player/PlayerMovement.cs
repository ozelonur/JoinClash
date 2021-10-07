using Sirenix.OdinInspector;
using UnityEngine;
using Ozel;

public class PlayerMovement : BaseController
{
    public static PlayerMovement Instance = null;
    [SerializeField, Required("Player settings is required!")] private PlayerSettings playerSettings;

    private ObjectManager objectManager;
    private GameManager gameManager;
    private Rigidbody playerRigidbody;

    private PlayerController playerController;

    [SerializeField, ReadOnly] private Vector3 difference = Vector3.zero;
    private Vector3 firstPosition;
    private Vector3 mousePosition;

    [SerializeField, ReadOnly] private float playerSpeed = 0;
    [SerializeField, ReadOnly] private bool hold = false;

    public object ShootState { get; private set; }
    public PlayerSettings PlayerSettings { get => playerSettings; set => playerSettings = value; }
    public float PlayerSpeed { get => playerSpeed; set => playerSpeed = value; }
    public bool Hold { get => hold; set => hold = value; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    protected override void Start()
    {
        base.Start();
        GetInstances();
        GetComponents();

        playerSpeed = playerSettings.ForwardSpeed;
    }

    private void Update()
    {
        firstPosition = Vector3.Lerp(firstPosition, mousePosition, .1f);
        if (gameManager.CurrentGameState == GameState.Playing)
        {
            if (Input.GetMouseButtonDown(0))
            {
                MouseDown(Input.mousePosition);
            }
            else if (Input.GetMouseButtonUp(0))
            {
                MouseUp();
            }
            else if (Input.GetMouseButton(0))
            {
                MouseHold(Input.mousePosition);

            }

            if (Input.GetKeyDown(KeyCode.T))
            {
                playerSpeed = playerSpeed + 2;
                print(playerSpeed);
            }

            if (Input.GetKeyDown(KeyCode.Y))
            {
                playerSpeed = playerSpeed - 2;
                print(playerSpeed);
            }

            transform.position = new Vector3(Mathf.Clamp(transform.position.x, -PlayerSettings.XRange, PlayerSettings.XRange), transform.position.y, transform.position.z);

            Vector3 currentRotation = transform.localRotation.eulerAngles;
            currentRotation.y = ConvertAngle.ConvertToAngle180(currentRotation.y);
            currentRotation.y = Mathf.Clamp(currentRotation.y, -PlayerSettings.YAngleRange, PlayerSettings.YAngleRange);
            transform.localRotation = Quaternion.Euler(currentRotation);

        }

    }

    private void FixedUpdate()
    {
        if (gameManager.CurrentGameState == GameState.Playing && Hold && !playerController.InBattle)
        {
            playerController.StartPhysics();
            playerRigidbody.velocity = transform.forward * playerSpeed;
            transform.Rotate(0, difference.x, 0);
            RunAnim();
        }
        else if (gameManager.CurrentGameState == GameState.Dead || (!Hold && !playerController.InBattle))
        {
            playerRigidbody.velocity = Vector3.zero;
            Idle();
        }
    }

    private void GetInstances()
    {
        objectManager = ObjectManager.Instance;
        gameManager = GameManager.Instance;
        playerController = PlayerController.Instance;
    }

    private void GetComponents()
    {
        playerRigidbody = GetComponent<Rigidbody>();
    }

    private void MouseHold(Vector3 inputPosition)
    {
        mousePosition = objectManager.OrthographicCamera.ScreenToWorldPoint(inputPosition);
        difference = mousePosition - firstPosition;
        difference *= PlayerSettings.Sensivity;
    }

    private void MouseUp()
    {
        Hold = false;
        difference = Vector3.zero;
    }

    private void MouseDown(Vector3 inputPosition)
    {
        Hold = true;
        mousePosition = objectManager.OrthographicCamera.ScreenToWorldPoint(inputPosition);
        firstPosition = mousePosition;
    }

}
