using UnityEngine;
using topdown;

public class Coin : JumpingItem
{
    BoxCollider2D col;
    
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        col = GetComponent<BoxCollider2D>();
        col.enabled = false;
    }

    

    public override void OnGround()
    {
        base.OnGround();
        col.enabled = true;
    }

    public override void OffGround()
    {
        base.OffGround();
        col.enabled = false;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            collision.GetComponent<player>().AddMoney(2);
            Destroy(transform.parent.gameObject);
        }
    }
}
