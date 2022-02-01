using UnityEngine;
using DG.Tweening;

public class JumpingItem : MonoBehaviour
{


    Vector3 StartingPos;
    public int Jumptimes;
    public float jumpPower;
    float distToGround;
    public bool NearGround;
    public float jumpDur;
    public int loops;
    public GameObject shadowObj;
    bool ClosingShadow;
    public float shadowmultiplier;
    public SpriteRenderer ShadowSprite;
    public float TransparencyMultiplier;
    float ShadowTransparency;
    public float DurationScaler;
    public float JumpScalar;

    public float TimeToDestroy;
    float timer;

    // Start is called before the first frame update
    public virtual void Start()
    {
        StartingPos = transform.position;
        Sequence seq = DOTween.Sequence();
        for (int i = 0; i < loops; i++)
        {
            seq.Append(transform.DOJump(transform.position, jumpPower, 1, jumpDur - i * DurationScaler));
            jumpPower /= JumpScalar;
            JumpScalar *= loops;

            //print("jump power is " + jumpPower);
            //print("jump scalar is " + JumpScalar);
            //print("jump dur is " + jumpDur);
        }

        timer = TimeToDestroy;

    }

    // Update is called once per frame
    public virtual void Update()
    {
        if (shadowObj != null)
        {
            distToGround = Vector3.Distance(transform.position, StartingPos);

            if (distToGround < .1)
            {
                NearGround = true;

                OnGround();
            }
            else
            {
                //if 
                NearGround = false;
                OffGround();

            }

            shadowObj.transform.localScale = new Vector3(distToGround * shadowmultiplier, distToGround * shadowmultiplier);
            ShadowTransparency = 1 - (distToGround * TransparencyMultiplier + .2f);
            ShadowSprite.color = new Color(ShadowSprite.color.r, ShadowSprite.color.g, ShadowSprite.color.b, ShadowTransparency);
        }
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            Destroy(transform.parent.gameObject);
        }

    }


    public virtual void OnGround()
    {
        print("object on Ground!");
    }

    public virtual void OffGround()
    {
        print("object off ground");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //collision.GetComponent<player>().AddMoney(25);
        }
    }
}
