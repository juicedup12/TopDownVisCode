using UnityEngine;
using DG.Tweening;

namespace topdown
{
    public class spearProjectile : MonoBehaviour
    {

        public bullet Bullet;
        public Ease TweenEase;
        public float TimeToReach;
        public float SetDistance;
        public int vibrito;
        public float elasticity;

        Tween tween;

        // Start is called before the first frame update
        void Start()
        {
            Bullet = GetComponent<bullet>();
            tween = transform.DOPunchPosition(transform.right * SetDistance, TimeToReach, vibrito, elasticity );
            tween.onKill = delegate { Bullet.enabled = true; };
        }

    }
}
