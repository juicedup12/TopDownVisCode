using UnityEngine;

public class MovementPixelPerfect : MonoBehaviour
{
    public float speed = 1.5f;
    public float m_PixelsPerUnit = 32;
    float multiple;

    // Start is called before the first frame update
    void Start()
    {
        multiple = 1.0f / 32.0f;   
    }

    private float RoundToMultiple(float speed)
    {
        // Using Mathf.Round at each frame is a performance killer
        return (int)((speed / multiple) + 0.5f) * multiple;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //float h = Input.GetAxisRaw("Horizontal");
        //float v = Input.GetAxisRaw("Vertical");
        float h = .5f;

        Vector3 moveto = new Vector3(h, 0);
        float t = RoundToMultiple(speed * Time.deltaTime);
        //Vector2 moveVec = new Vector2((transform.position.x + h * speed * Time.deltaTime), (transform.position.y + v * speed * Time.deltaTime));

        //transform.position = new Vector2(Mathf.Round(moveVec.x * m_PixelsPerUnit) / m_PixelsPerUnit ,
        //   Mathf.Round(moveVec.y * m_PixelsPerUnit) / m_PixelsPerUnit);

        transform.position = new Vector3(transform.position.x + h* t, 0);
    }
}
