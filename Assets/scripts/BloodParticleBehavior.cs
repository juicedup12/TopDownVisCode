using UnityEngine;
using DG.Tweening;

public class BloodParticleBehavior : MonoBehaviour
{

    bool StartingToFollow;
    bool StayOnbar;
    Transform targetpos;
    public float randomdirspeed;
    public Ease randomdirease;
    Material bloodmat;
    SpriteRenderer img;
    public Ease FlashEase;
    
    public float xspeed, yspeed, xaccel, yaccel, friction;

    // Start is called before the first frame update
    void Start()
    {
        float randomx = Random.value;
        float randomy = Random.value;

        Vector3 moveto = new Vector3(Random.Range(randomx * -2, randomx * 2), Random.Range(randomy * -2, randomy * 2));
        Tween movetween = transform.DOMove(transform.position + moveto, randomdirspeed).SetEase(randomdirease);
        movetween.SetUpdate(true);
        movetween.onComplete = ()  => StartingToFollow = true;
        targetpos = PsychoMeterController.instance.transform;
        print("target pos is " + targetpos);
        //transform.position = Vector3.MoveTowards(transform.position, screencorner, .5f);
        img = GetComponent<SpriteRenderer>();
        bloodmat = img.material;
        //img.material = bloodmat;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (StartingToFollow)
        {
            if (targetpos.position.x > transform.position.x + .2)
                xspeed = xspeed + xaccel;
            else if (targetpos.position.x < transform.position.x - .2)
                xspeed = xspeed - xaccel;
            xspeed = xspeed * friction;
            if (targetpos.position.y > transform.position.y + .2)
                yspeed = yspeed + yaccel;
            else if (targetpos.position.y < transform.position.y - .2)
                yspeed = yspeed - yaccel;
            yspeed = yspeed * friction;

            transform.position += new Vector3(xspeed, yspeed);

            float dist = Vector2.Distance(transform.position, targetpos.position);
            if(dist < .2f && StartingToFollow)
            {
                //Destroy(gameObject);
                print("blood particle reached ");
                Tween flashtween = bloodmat.DOColor(Color.magenta * 4, Shader.PropertyToID("_color"), 1f).SetEase(FlashEase) ;
                flashtween.onComplete = () => { print("done flash tweening"); Destroy(gameObject); };
                StartingToFollow = false;
                StayOnbar = true;
                //turn off trail renderer
            }

        }
    }

    private void FixedUpdate()
    {

        if (StayOnbar)
        {
            transform.position = targetpos.position;

        }
    }
}
