using UnityEngine;


//causes specified events to occur when click action is triggered
namespace topdown
{

    public class GlobalItem : Item
    {
        GameObject Snow;
        public override void OnDisable()
        {
            base.OnDisable();
        }

        // Start is called before the first frame update
        public override void Start()
        {
            base.Start();
            ClickAction += activateIce;
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void activateIce()
        {
            Instantiate(Snow);
            //Gmanager.instance.ActivateGlobalEvent();
        }

    }
}
