using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace topdown
{
    public class UnityEventInteraction : MonoBehaviour, Iinteractable
    {
        public UnityEvent OnInteract;

        public void interact()
        {
            print("Unity event called");
            OnInteract?.Invoke();
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
}
