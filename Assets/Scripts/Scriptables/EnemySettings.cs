using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(menuName = "Enemy Settings", fileName = "EnemySettings", order = 1)]
public class EnemySettings : ScriptableObject
{
    [SerializeField, InfoBox("Enemy health must be bigger than zero!", InfoMessageType.Warning, "IsEnemyHealthZero")] private float enemyHealth;
    [SerializeField, InfoBox("Given damage must be bigger than zero!", InfoMessageType.Warning, "IsGivenDamageZero")] private float givenDamage;

    public float EnemyHealth { get => enemyHealth; }
    public float GivenDamage { get => givenDamage; }

    private bool IsEnemyHealthZero()
    {
        return EnemyHealth == 0;
    }

    private bool IsGivenDamageZero()
    {
        return GivenDamage == 0;
    }
}
