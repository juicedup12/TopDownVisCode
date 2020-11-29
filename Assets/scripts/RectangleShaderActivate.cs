using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RectangleShaderActivate : MonoBehaviour
{
    SpriteRenderer sprRendr;
    Material mat;
    float noiseStr = 1;
    public float DissolveRate = .5f;
    public bool activateDissolve = false;


    // Start is called before the first frame update
    void Start()
    {
        sprRendr = GetComponent<SpriteRenderer>();
        mat = sprRendr.material;
        mat.SetFloat("Noise_Strength", 1);
    }

    // Update is called once per frame
    void Update()
    {
        if (activateDissolve)
        {
            if (noiseStr > 0)
            {
                noiseStr -= DissolveRate * Time.deltaTime;
                mat.SetFloat("Noise_Strength", noiseStr);
            }
            if (noiseStr <= 0)
            {
                activateDissolve = false;
                noiseStr = 1;
            }
        }
    }


    public void SetRectRegion(Vector2 rectpos)
    {
        mat.SetVector("Square_Pos", rectpos);
    }
}
