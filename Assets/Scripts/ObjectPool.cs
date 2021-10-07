using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using Ozel;
public class ObjectPool : Singleton<ObjectPool>
{
    private List<GameObject> smashParticles;
    private GameObject smashParticlePrefab;

    private void Start()
    {
        smashParticlePrefab = ParticleManager.Instance.SmashParticle.gameObject;
        smashParticles = new List<GameObject>();
        
    }


    public GameObject GetObject()
    {
        if (smashParticles.Count > 0)
        {
            GameObject go = smashParticles[0];
            go.SetActive(true);
            smashParticles.RemoveAt(0);
            return go;
        }
        else
        {
            GameObject temp = Instantiate(smashParticlePrefab, transform);
            smashParticles.Add(temp);
            GetObject();
            return temp;
        }


    }

    public void ReturnToPool(GameObject obj)
    {
        if (!smashParticles.Contains(obj))
        {
            obj.SetActive(false);
            smashParticles.Add(obj);
        }
    }

}
