using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSettings", menuName = "PlayerSettings", order = 1)]
public class PlayerSettings : ScriptableObject
{
    [SerializeField, InfoBox("Forward speed must be greater than 0!", InfoMessageType.Warning, "IsForwardSpeedZero")] private float forwardSpeed;
    [SerializeField, InfoBox("Sensivity must be greater than 0!", InfoMessageType.Warning, "IsSensivityZero")] private float sensivity;
    [SerializeField, InfoBox("X Range must be greater than 0!", InfoMessageType.Warning, "IsXRangeZero")] private float xRange;
    [SerializeField, InfoBox("Y Angle Range must be different than 0!", InfoMessageType.Warning, "IsYAngleRangeZero")] private float yAngleRange;
    [SerializeField, InfoBox("Given Damage must be bigger than zero!", InfoMessageType.Warning, "IsGivenDamageZero")] private float givenDamage;
    [SerializeField, InfoBox("Player Health must be bigger than zero!", InfoMessageType.Warning, "IsPlayerHealthZero")] private float playerHealth;

    public float ForwardSpeed { get => forwardSpeed; }
    public float Sensivity { get => sensivity; }
    public float XRange { get => xRange; }
    public float YAngleRange { get => yAngleRange; }
    public float GivenDamage { get => givenDamage; }
    public float PlayerHealth { get => playerHealth; }

    private bool IsForwardSpeedZero()
    {
        return forwardSpeed == 0;
    }

    private bool IsSensivityZero()
    {
        return sensivity == 0;
    }

    private bool IsXRangeZero()
    {
        return xRange == 0;
    }

    private bool IsYAngleRangeZero()
    {
        return yAngleRange == 0;
    }

    private bool IsGivenDamageZero()
    {
        return givenDamage == 0;
    }

    private bool IsPlayerHealthZero()
    {
        return PlayerHealth == 0;
    }
}
