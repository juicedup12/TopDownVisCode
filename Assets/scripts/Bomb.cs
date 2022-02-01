using UnityEngine;
using topdown;

public class Bomb : JumpingItem
{
    BoxCollider2D boxcol;
    Animator anim;
    bool firstHit = false;

    // Start is called before the first frame update
    public override void Start()
    {
        boxcol = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
        base.Start();

    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }

    public override void OnGround()
    {
        
        if (firstHit)
        {
            boxcol.enabled = true;
            anim.SetTrigger("explode");
        }
        firstHit = true;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            collision.GetComponent<player>().TakeDamage(transform.position);

        }
    }


    public void DestroySelf()
    {
        Destroy(transform.parent.gameObject);
    }
}
