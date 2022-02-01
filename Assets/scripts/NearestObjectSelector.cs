using UnityEngine;


//this subtype is for detecting whatever object is nearest and most infront to the player
public class NearestObjectSelector : MonoBehaviour, ISelector
{


    public float Circleradius;
    public int LayerNum;
    public MonoBehaviour GetSelection(Transform transform, MonoBehaviour obj)
    {
         //get all interactable collisions around player
            MonoBehaviour nearestobj = null;
            LayerMask interactableLM = 1 >> LayerNum;
            Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, Circleradius, interactableLM);
            if(cols != null)
            {
                //check for closest and most infront of player
                float closest = float.MaxValue;

                if(cols.Length > 1)
                {
                    for (int i = 0; i < cols.Length; i++)
                    {
                        float currentDist = Vector3.Distance(transform.position, cols[i].transform.position);
                        Vector3 forward = transform.TransformDirection(Vector3.forward);
                        Vector3 toOther = cols[i].transform.position - transform.position;
                        float currentDot = Vector3.Dot(forward, toOther) -1;
                        float CurrentDistAndDot = currentDist + currentDot;
                        if(CurrentDistAndDot < closest)
                        nearestobj = cols[i].GetComponent<PlayerInteractable>();
                    }
                }
                else
                {
                    nearestobj = cols[0].GetComponent<PlayerInteractable>();
                }
            }
            else
            {
                nearestobj = null;
            }
        return nearestobj;
    }

}