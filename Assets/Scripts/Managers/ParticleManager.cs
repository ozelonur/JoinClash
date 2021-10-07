using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;
using Ozel;

public class ParticleManager : Singleton<ParticleManager>
{
    [SerializeField, Required("Smash Particle Required")] private ParticleSystem smashParticle;

    public ParticleSystem SmashParticle { get => smashParticle; set => smashParticle = value; }

    private ObjectPool objectPool;

    private void Start()
    {
        objectPool = ObjectPool.Instance;
    }

    public void RunSmashParticle(Transform character)
    {
        ParticleSystem particle = objectPool.GetObject().GetComponent<ParticleSystem>();
        if (particle.gameObject != null)
        {
            particle.transform.position = new Vector3(character.position.x, character.position.y + .25f, character.position.z);
            particle.gameObject.SetActive(true);
        }

        if (character.name.Contains(Constants.ENEMY))
        {
            ParticleSystem.MainModule mainModule = particle.main;
            mainModule.startColor = Color.red;
        }

        Run.After(1, () =>
        {
            objectPool.ReturnToPool(particle.gameObject);
        });

    }

}
