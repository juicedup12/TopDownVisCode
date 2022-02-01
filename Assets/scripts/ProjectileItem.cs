using UnityEngine;
using topdown;

public class ProjectileItem : MonoBehaviour
{
    Sprite Itemsprite;
    public ItemClass Item;
    public string HitTag;
    SpriteRenderer SprRenderer;
    Transform SpriteTransfrom;

    // Start is called before the first frame update
    void Start()
    {
        SprRenderer = GetComponentInChildren<SpriteRenderer>();
        SprRenderer.sprite = Item.itemsprite;
        SpriteTransfrom = transform.GetChild(0);
    }

    // Update is called once per frame
    void Update()
    {
        SpriteTransfrom.Rotate(new Vector3(0, 0, 10));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(HitTag))
        {
            print("thrown item hit enemy");
            Enemy enemy = collision.GetComponent<Enemy>();
            if(enemy != null)
            {
                Item.UseItem(enemy);
            }
        }

        print("thrown item destroyed after hitting " + collision.gameObject.name);
        Destroy(gameObject);
    }

}
