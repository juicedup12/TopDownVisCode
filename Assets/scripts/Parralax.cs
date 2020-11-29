using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parralax : MonoBehaviour
{
    public Vector2 ParallaxEffectMultiplier;
    public Vector2 OriginalParMultiplier;
    public Transform followtransform;
    Vector3 lastcampos;
    float textureUnitSizeX;
    private float textureUnitSizey;
    public float m_PixelsPerUnit = 32;
    float multiple;


    // Start is called before the first frame update
    void Start()
    {
        OriginalParMultiplier = ParallaxEffectMultiplier;
        //followtransform = Camera.main.transform;
        lastcampos = followtransform.position;
        Sprite sprite = GetComponent<SpriteRenderer>().sprite;
        Texture2D texture = sprite.texture;
        textureUnitSizeX = texture.width / sprite.pixelsPerUnit;
        textureUnitSizey = texture.height / sprite.pixelsPerUnit;
        multiple = 1.0f / 32.0f;
    }

    private float RoundToMultiple(float speed)
    {
        // Using Mathf.Round at each frame is a performance killer
        
        return (int)((speed / multiple) + Mathf.Sign(speed) /2) * multiple;
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 deltamovement = followtransform.position - lastcampos;
        //transform.position += new Vector3(deltamovement.x * ParallaxEffectMultiplier.x, deltamovement.y * ParallaxEffectMultiplier.y);
        ////transform.Translate(deltamovement * ParallaxEffectMultiplier);
      
        float x = RoundToMultiple(ParallaxEffectMultiplier.x * deltamovement .x);
        float y = RoundToMultiple(ParallaxEffectMultiplier.y * deltamovement.y);
        transform.Translate(new Vector3(x , y));
        
        
        lastcampos = followtransform.position;

        if (Mathf.Abs(followtransform.position.x - transform.position.x) >= (textureUnitSizeX * transform.localScale.x ))
        {
            float offsetPosx = (followtransform.position.x - transform.position.x)  % textureUnitSizeX ;
            transform.position = new Vector3(followtransform.position.x + offsetPosx, transform.position.y);
        }


    }


    public void AssignFollow(GameObject Followobj)
    {
        followtransform = Followobj.transform;
        lastcampos = Followobj.transform.position;
    }
    
}
