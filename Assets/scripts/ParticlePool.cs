using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlePool : MonoBehaviour
{

    public GameObject DissolvingParticlePrefab;
    GenericObjectPool<PoolableDissolveParticle> DissolveParticles;

    private void Awake()
    {
        DissolveParticles = new GenericObjectPool<PoolableDissolveParticle>(DissolvingParticlePrefab, 10);
    }


    // Start is called before the first frame update
    void Start()
    {
        //DissolveParticles.Pull(Vector3.up);
    }

    public void RetrieveParticle(Transform DissolvingHull)
    {

        DissolveParticles.Pull().AssignMeshRender(DissolvingHull);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
