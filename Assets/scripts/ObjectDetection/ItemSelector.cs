using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey;

namespace topdown
{
    public class ItemSelector : MonoBehaviour
    {
        //Dictionary<GameObject, string> items;
        public float DetectSize;
        List<itemVals> items;
        float closest;

        // Start is called before the first frame update
        void Start()
        {
            //items = new Dictionary<GameObject, string>();
            items = new List<itemVals>();
            closest = float.MaxValue;

        }

        // Update is called once per frame
        void Update()
        {
            TargetInteractableOjects();
        }


        void TargetInteractableOjects()
        {

            //clear it incase we leave detection range
            for (int i = 0; i < items.Count; i++)
            {
                items[i] = new itemVals(items[i].ItemName, string.Empty);
            }

            //get all interactable collisions around player
            int interactableLM = 1 << 18;
            //print("layer mask is " + interactableLM);
            Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, DetectSize, interactableLM);
            if (cols != null)
            {
                ////check for closest and most infront of player


                if (cols.Length > 1)
                {
                    //print("Many items near player");
                    for (int i = 0; i < cols.Length; i++)
                    {


                        float currentDist = Vector3.Distance(transform.position, cols[i].transform.position);
                        Vector3 forward = transform.up;
                        Vector3 toOther = transform.position - cols[i].transform.position;
                        float currentDot = Vector3.Dot(forward, toOther.normalized);
                        float CurrentDistAndDot = currentDist + currentDot;

                        //cols[i].GetComponent<PlayerInteractable>().DotText.text = "dot is " + currentDot.ToString() + "\n dist is" + currentDist + "both is " + CurrentDistAndDot;
                        string text = $"dot is {currentDot} \n dist is {currentDist} both is {CurrentDistAndDot}";
                        handleDebug(cols[i].gameObject, text);
                        if (CurrentDistAndDot < closest)
                        {
                            closest = CurrentDistAndDot;
                            //PlayerInteractableTarget = cols[i].GetComponent<PlayerInteractable>();
                            handleDebug(cols[i].gameObject, "is targeted");
                            //print("current dist and dot is" + CurrentDistAndDot);
                            //print("current dot is is " + currentDot);
                        }
                    }
                }
                else if (cols.Length == 1)
                {
                    Vector3 forward = transform.up;
                    Vector3 toOther = transform.position - cols[0].transform.position;
                    float currentDot = Vector3.Dot(forward, toOther.normalized);
                    //float currentDist = Vector3.Distance(transform.position, cols[0].transform.position);
                    handleDebug(cols[0].gameObject, currentDot.ToString() + "\n is only targeted");
                    //PlayerInteractableTarget = cols[0].GetComponent<PlayerInteractable>();
                    //print("one item near player, "+ PlayerInteractableTarget.gameObject.name);
                }
                //    if
                //    (cols.Length == 0)
                //    {
                //        foreach(KeyValuePair<GameObject, string> entry in items)
                //        {

                //            handleDebug(entry.Key, "");
                //        }
                //        //print("collisons to items is " + cols.Length);
                //        //PlayerInteractableTarget = null;
                //        //InteractUIBehavior.instance.HideUI();
                //    }
                //}
                //else
                //{

                //    //PlayerInteractableTarget = null;
                //    //InteractUIBehavior.instance.HideUI();
                //}
            }

            //if
            //(PlayerInteractableTarget != null)
            //{
            //    InteractUIBehavior.instance.DisplayUI(() => print("on item"), PlayerInteractableTarget.transform.position, "To inspect");
            //}
            //PlayerInteractableTarget = null;
        }

        void handleDebug(GameObject go, string text)
        {
            //if(!items.ContainsKey(go))
            //{
            //    //itemVals item = new itemVals(go.name, val);
            //    items.Add(go, text);
            //    CMDebug.TextUpdater(() => { string valRef; items.TryGetValue(go, out valRef); return valRef.ToString(); }, go.transform.position); 
            //}
            //else if(items.ContainsKey(go))
            //{
            //    items[go] = text;
            //}

            for (int i = 0; i < items.Count; i++)
            {
                //check if the item is already added
                if (items[i].ItemName == go.name)
                {
                    //update item
                    items[i] = new itemVals(go.name, text);

                    //no need to keep checking if we already found it
                    return;
                }
            }
            //if no return then we can add it
            items.Add(new itemVals(go.name, text));
            int count = items.Count;
            CMDebug.TextUpdater(() => { return items[count - 1].ItemDetails; }, go.transform.position);


        }
    }
}
