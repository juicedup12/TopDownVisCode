using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


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
        

        DissolveMatInstances = new List<Material>();
        
        
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

    public void StartDissolve()
    {
        //StartCoroutine(DissolveRoutine());
        //dissolvemat.SetFloat("Noise_Strength", DissolveAmount);

        int ID = Shader.PropertyToID("Noise_Strength");
        dissolvemat.DOFloat(-0.2f, ID, DissolveDur);
            
        
        print("starting dissolve");
    }

    public void DarkenMaterial()
    {
        BlendAmount += .1f;
        //foreach (Material mat in DissolveMatInstances)
        //{
        //    mat.DOFloat(BlendAmount, "blend_opacity", .5f).SetUpdate(true);
        //}
        dissolvemat.DOFloat(BlendAmount, "blend_opacity", .5f).SetUpdate(true);
    }

}
