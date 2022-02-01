using UnityEngine;

public class PPURounder : MonoBehaviour
{
   
    public float m_PixelsPerUnit = 32;
    static float multiple;


    // Start is called before the first frame update
    void Start()
    {
        multiple = 1.0f / 32.0f;
    }

    public static float RoundToMultiple(float speed)
    {
        // Using Mathf.Round at each frame is a performance killer
        return (int)((speed / multiple) + 0.5f) * multiple;
    }

    //returned vector 2 to be used in translate
    public static Vector2 ReturnRoundedValue(Vector2 movement, float speed)
    {
        ////transform.Translate(deltamovement * ParallaxEffectMultiplier);
        float x = RoundToMultiple(speed * movement.x);
        float y = RoundToMultiple(speed * movement.y);

        return new Vector2(x, y);
    }
}
