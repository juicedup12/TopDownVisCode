using UnityEngine;

//need to make it so parralax ofbjects stay on camera's y axis
public class Parralax : MonoBehaviour
{
    public Vector2 ParallaxEffectMultiplier;
    [HideInInspector]
    public Vector2 OriginalParMultiplier;
    Vector3 lastcampos;
    float textureUnitSizeX;
    private float textureUnitSizey;
    public float m_PixelsPerUnit = 32;
    float multiple;
    float xNeutral = 0;
    float initialY, initialZ;
    public float xValue;
    Transform cameraTransform;
    public Vector3 CamOffset;


    // Start is called before the first frame update
    void Start()
    {
        initialY = transform.localPosition.y; initialZ = transform.localPosition.z;
        OriginalParMultiplier = ParallaxEffectMultiplier;
        //followtransform = Camera.main.transform;
        Sprite sprite = GetComponentInChildren<SpriteRenderer>().sprite;
        Texture2D texture = sprite.texture;
        textureUnitSizeX = texture.width / sprite.pixelsPerUnit;
        textureUnitSizey = texture.height / sprite.pixelsPerUnit;
        multiple = 1.0f / 16.0f;
    }

    private float RoundToMultiple(float speed)
    {
        // Using Mathf.Round at each frame is a performance killer
        
        return (int)((speed / multiple) + Mathf.Sign(speed) /2) * multiple;
    }





    // Update is called once per frame
    //private void Update()
    //{
    //    Vector2 deltamovement = followtransform.position - lastcampos;

    //    //transform.position += new Vector3(deltamovement.x * ParallaxEffectMultiplier.x, deltamovement.y * ParallaxEffectMultiplier.y);
    //    ////transform.Translate(deltamovement * ParallaxEffectMultiplier);

    //    float x = RoundToMultiple(ParallaxEffectMultiplier.x * xValue);
    //    //transform.position += new Vector3(x * ParallaxEffectMultiplier.x, 0, 0);
    //    //float y = RoundToMultiple(ParallaxEffectMultiplier.y * deltamovement.y);
    //    //transform.Translate(new Vector3(x * ParallaxEffectMultiplier.x, 0));
    //    transform.localPosition = new Vector3(-(x - xNeutral), initialY, initialZ);

    //    //lastcampos = followtransform.position;


    //    //pos.x - orbit.x (20 - 10) = 10 
    //    if (Mathf.Abs(x - xNeutral) >= (textureUnitSizeX * transform.localScale.x))
    //    {
    //        print(Mathf.Abs(x - xNeutral) + "greater than tex size " + textureUnitSizeX + " , xneutral is now : " + xValue);
    //        transform.localPosition = new Vector3((x - xNeutral) % textureUnitSizeX, initialY, initialZ);
    //        xNeutral = x;
    //        //    float offsetPosx = (followtransform.position.x - transform.position.x) % textureUnitSizeX;
    //        //    transform.position = new Vector3(followtransform.position.x + offsetPosx, transform.position.y);
    //    }
    //}

    //private void Update()
    //{



    //    float x = RoundToMultiple(ParallaxEffectMultiplier.x * xValue);
    //    float x = ParallaxEffectMultiplier.x * xValue;
    //    transform.position += new Vector3(x * ParallaxEffectMultiplier.x, 0, 0);
    //    float y = RoundToMultiple(ParallaxEffectMultiplier.y * deltamovement.y);
    //    transform.Translate(new Vector3(x * ParallaxEffectMultiplier.x, 0));
    //    transform.localPosition = new Vector3(x * ParallaxEffectMultiplier.x, 0, 0);



    //    pos.x - orbit.x(20 - 10) = 10
    //    if (Mathf.Abs(x - xNeutral) >= (textureUnitSizeX * transform.localScale.x))
    //    {
    //        print(Mathf.Abs(x - xNeutral) + "greater than tex size " + textureUnitSizeX + " , xneutral is now : " + xValue);
    //        transform.localPosition = new Vector3(-(x - xNeutral) % textureUnitSizeX, initialY, initialZ);
    //        xNeutral = x;
    //        float offsetPosx = (followtransform.position.x - transform.position.x) % textureUnitSizeX;
    //        transform.position = new Vector3(followtransform.position.x + offsetPosx, transform.position.y);
    //    }


    //}

    private void Update()
    {
        //Vector2 deltamovement = followtransform.position - lastcampos;

        //transform.position += new Vector3(deltamovement.x * ParallaxEffectMultiplier.x, deltamovement.y * ParallaxEffectMultiplier.y);
        ////transform.Translate(deltamovement * ParallaxEffectMultiplier);

        float x = RoundToMultiple(ParallaxEffectMultiplier.x * xValue);
        //float x = ParallaxEffectMultiplier.x * xValue;
        //transform.position += new Vector3(x * ParallaxEffectMultiplier.x, 0, 0);
        //float y = RoundToMultiple(ParallaxEffectMultiplier.y * deltamovement.y);
        //transform.Translate(new Vector3(x * ParallaxEffectMultiplier.x, 0));
        transform.localPosition = new Vector3(-(x - xNeutral), initialY, initialZ);

        //lastcampos = followtransform.position;


        //pos.x - orbit.x (20 - 10) = 10 
        if (Mathf.Abs(x - xNeutral) >= (textureUnitSizeX * transform.localScale.x))
        {
            print(Mathf.Abs(x - xNeutral) + "greater than tex size " + textureUnitSizeX + " , xneutral is now : " + xValue);
            transform.localPosition = new Vector3(-(x - xNeutral) % textureUnitSizeX, initialY, initialZ);
            xNeutral = x;
            //    float offsetPosx = (followtransform.position.x - transform.position.x) % textureUnitSizeX;
            //    transform.position = new Vector3(followtransform.position.x + offsetPosx, transform.position.y);
        }

    }

    //void FixedUpdate()
    //{
    //    if (followtransform != null)
    //    {
    //        Vector2 deltamovement = followtransform.position - lastcampos;

    //        //transform.position += new Vector3(deltamovement.x * ParallaxEffectMultiplier.x, deltamovement.y * ParallaxEffectMultiplier.y);
    //        ////transform.Translate(deltamovement * ParallaxEffectMultiplier);

    //        float x = RoundToMultiple(ParallaxEffectMultiplier.x * xValue);
    //        //float x = ParallaxEffectMultiplier.x * xValue;
    //        //transform.position += new Vector3(x * ParallaxEffectMultiplier.x, 0, 0);
    //        //float y = RoundToMultiple(ParallaxEffectMultiplier.y * deltamovement.y);
    //        //transform.Translate(new Vector3(x * ParallaxEffectMultiplier.x, 0));
    //        transform.localPosition = new Vector3(-(x - xNeutral), initialY, initialZ);

    //        //lastcampos = followtransform.position;


    //        //pos.x - orbit.x (20 - 10) = 10 
    //        if (Mathf.Abs(x - xNeutral) >= (textureUnitSizeX * transform.localScale.x))
    //        {
    //            print(Mathf.Abs(x - xNeutral) + "greater than tex size " + textureUnitSizeX + " , xneutral is now : " + xValue);
    //            transform.localPosition = new Vector3(-(x - xNeutral) % textureUnitSizeX, initialY, initialZ);
    //            xNeutral = x;
    //            //    float offsetPosx = (followtransform.position.x - transform.position.x) % textureUnitSizeX;
    //            //    transform.position = new Vector3(followtransform.position.x + offsetPosx, transform.position.y);
    //        }

    //    }
    //}



}
