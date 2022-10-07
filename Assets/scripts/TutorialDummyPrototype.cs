using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace topdown
{
    public class TutorialDummyPrototype : Prop
    {
        [SerializeField]
        private ObjectPool PropPool;
        private SpriteRenderer sRender;
        private Material Mat;
        [SerializeField]
        private Material FlashMat;
        [SerializeField] ParticleSystem particles;
        [SerializeField] Sprite BrokenSprite;
        [SerializeField] UnityEvent OnBreak;

        // Start is called before the first frame update
        void Start()
        {
            sRender = GetComponent<SpriteRenderer>();
            Mat = sRender.material;
        }

        // Update is called once per frame
        void Update()
        {

        }

        public override void Break()
        {
            
            HitEffectManager.instance.ShakeCamera(2, .1f);
            print("prototype break");
            //instantiate big piece

            //instantiate small pieces
            print("pieces length is " + pieces.Length);
            //for (int i = 0; i < pieces.Length; i++)
            //{
            //    GameObject Piece = PropPool.GetPooledObject();
            //    if (Piece != null)
            //    {
            //        Piece.transform.position = transform.position;
            //        Piece.transform.rotation = transform.rotation;
            //        Piece.SetActive(true);

            //        Piece.GetComponent<PropPiece>().SetPropObject(((Vector2)transform.position - pos) * 3, pieces[i]);
            //    }
            //}
            //Destroy(gameObject);
            StartCoroutine(Flash());
            ParticleSystem.EmitParams emit = new ParticleSystem.EmitParams();
            emit.position = transform.position;
            emit.velocity = ((Vector2)transform.position - HitPos) * 3;
            particles.Emit(emit, 3);
            GetComponent<Collider2D>().enabled = false;
            sRender.sprite = BrokenSprite;
            OnBreak.Invoke();
        }

        IEnumerator Flash()
        {
            sRender.material = FlashMat;
            yield return new WaitForSeconds(.1f);
            sRender.material = Mat;
        }

    }
}
