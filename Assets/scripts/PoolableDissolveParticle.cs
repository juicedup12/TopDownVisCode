using System;
using UnityEngine;

public class PoolableDissolveParticle : MonoBehaviour, IPoolable<PoolableDissolveParticle> 
{

    Action<PoolableDissolveParticle> ReturnAction;
    ParticleSystem Particle;

    public void Initialize(Action<PoolableDissolveParticle> returnAction)
    {
        ReturnAction = returnAction;
        if (Particle == null)
        {
            Particle = GetComponent<ParticleSystem>();
        }
    }

    public void ReturnToPool()
    {
        ReturnAction?.Invoke(this);
    }


    public void AssignMeshRender(Transform DissolvingHull)
    {
        var sh = Particle.shape;
        sh.shapeType = ParticleSystemShapeType.MeshRenderer;
        sh.meshRenderer = DissolvingHull.GetComponent<MeshRenderer>();
        sh.useMeshColors = false;
        transform.parent = DissolvingHull.transform;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnParticleSystemStopped()
    {
        print("particles ended");

        //Transform p = transform.parent;
        transform.parent = null;
        //Destroy(p.gameObject);
        ReturnToPool();
    }
}
