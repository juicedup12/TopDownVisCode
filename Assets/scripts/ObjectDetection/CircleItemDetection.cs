using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace InteractableSelect
{
    class CircleItemDetection : MonoBehaviour, IObjectDetection
    {
        public float DetectSize = 1;
        int interactableLM = 1 << 18;
        [SerializeField] Transform ForwardTransform;
        List<Transform> itemTransforms;

        private void Start()
        {
            itemTransforms = new List<Transform>();
        }
        

        public bool Check()
        {

            itemTransforms.Clear();
            //will detect items around player, add items to a list
            Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, DetectSize, interactableLM);
            if (cols != null)
            {
                if (cols.Length < 1) return false;
                //each time it's checked the list is cleared
                itemTransforms.Clear();
                foreach (Collider2D c in cols)
                {
                    if(!itemTransforms.Contains(c.transform))
                    {
                        itemTransforms.Add(c.transform);
                    }
                }
                return true;

            }
            return false;
            
        }
        
        public Transform GetSelection()
        {
            //will return what item around player gets selected
            if (itemTransforms.Count > 1)
            {
                print("Many items near player: " + itemTransforms.Count);
                float closest = float.MaxValue;
                Transform currentClosest = null;
                for (int i = 0; i < itemTransforms.Count; i++)
                {
                    float CurrentDistAndDot = GetDistAndDot(itemTransforms[i]);
                    if(CurrentDistAndDot < closest)
                    {
                        closest = CurrentDistAndDot;
                        currentClosest = itemTransforms[i];
                        //PlayerInteractableTarget = cols[i].GetComponent<PlayerInteractable>();
                        //print("current dist and dot is" + CurrentDistAndDot);
                        //print("current dot is is " + currentDot);
                    }
                }
                return currentClosest;
            }
            else if (itemTransforms.Count == 1)
            {
                print("Get selection: " + itemTransforms[0]);
                return itemTransforms[0];
            }
            return null;
        }

        float GetDistAndDot(Transform t)
        {
            float currentDist = Vector3.Distance(transform.position, t.position);
            Vector3 forward = transform.TransformPoint(ForwardTransform.up);
            Vector3 toOther = transform.position - t.position;
            float currentDot = Vector3.Dot(forward, toOther.normalized);
            return currentDist + currentDot;
           
        }
    }
}

