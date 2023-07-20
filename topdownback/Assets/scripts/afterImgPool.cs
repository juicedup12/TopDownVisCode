using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class afterImgPool : MonoBehaviour
{
    public GameObject afterimgprefab;
    Queue<GameObject> availableobj = new Queue<GameObject>();

    public static afterImgPool instance { get; private set; }


    private void Awake()
    {
        instance = this;
        growpool();
    }

    void growpool()
    {
        for (int i = 0; i < 10; i++)
        {
            var instancetoadd = Instantiate(afterimgprefab);
            instancetoadd.transform.SetParent(transform);
            Addtopool(instancetoadd);
        }
    }

    public void Addtopool(GameObject instance)
    {
        instance.SetActive(false);
        availableobj.Enqueue(instance);
    }

    public GameObject getfrompool()
    {

        if (availableobj.Count == 0)
        {
            growpool();
        }

        var instance = availableobj.Dequeue();
        instance.SetActive(true);
        return instance;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
