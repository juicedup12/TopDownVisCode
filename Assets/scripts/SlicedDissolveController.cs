using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;


/// <summary>
/// This script will handle the values of the sliced materials
/// </summary>
/// 

public class SlicedDissolveController : MonoBehaviour
{

    public float DissolveDur ;
    public Material dissolvemat;
    public float BlendAmount = 0;
    List<Material> DissolveMatInstances;
    public int availableDissolveMat = 0;

    private void Start()
    {
        

        //DissolveMatInstances = new List<Material>();
        //StartDissolve(dissolvemat);
        
    }

    public void GetDissolveMat(MeshRenderer meshRender)
    {
        
        //DissolveMatInstances.Add( meshRender.material);
        //availableDissolveMat++;
        //Material[] mats = new Material[2];
        
        //mats[0] = DissolveMatInstances[availableDissolveMat -1];
        //meshRender.materials = mats;
        meshRender.sharedMaterial = dissolvemat;
        meshRender.sortingLayerName = ("UI");



    }

    public void StartDissolve(Material mat, Action onComplete)
    {
        //StartCoroutine(DissolveRoutine());
        //dissolvemat.SetFloat("Noise_Strength", DissolveAmount);

        int ID = Shader.PropertyToID("Noise_Strength");
        mat.DOFloat(-0.2f, ID, DissolveDur).onComplete = () => onComplete();
            
        
        print("starting dissolve");
    }

    

    public void DarkenMaterial(Material mat)
    {
        BlendAmount += .1f;
        //foreach (Material mat in DissolveMatInstances)
        //{
        //    mat.DOFloat(BlendAmount, "blend_opacity", .5f).SetUpdate(true);
        //}
        mat.DOFloat(BlendAmount, "blend_opacity", .5f).SetUpdate(true);
    }

}
