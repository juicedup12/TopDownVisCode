using UnityEngine;

public class AfterImg : MonoBehaviour
{
    [SerializeField]
    float activeTime = .1f;
    float timeacitvated;
    float alpha;
    [SerializeField]
    float alphaset = 0.8f;
    float alphamultiplier = 0.85f;

    public GameObject playerobj;

    Transform player;
    SpriteRenderer SR;
    SpriteRenderer playerSR;

    public Color color;


    private void OnEnable()
    {
        //enabled = true;
        SR = GetComponent<SpriteRenderer>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerSR = player.GetComponentInChildren<SpriteRenderer>();

        alpha = alphaset;
        SR.sprite = playerSR.sprite;
        transform.position = player.position;
        transform.rotation = player.rotation;
        timeacitvated = Time.time;
    }
    

    // Update is called once per frame
    void Update()
    {
        alpha *= alphamultiplier;
        color = new Color(1f, 1f, 1f, alpha);
        SR.color = color;

        if (Time.time >= timeacitvated + activeTime)
        {
            afterImgPool.instance.Addtopool(gameObject);
        }
    }
}
