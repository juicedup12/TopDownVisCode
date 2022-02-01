using UnityEngine;
using topdown;
using Yarn;
using Yarn.Unity;
using Yarn.Unity.Example;

public class TutorialEventHandler : MonoBehaviour
{
    public InMemoryVariableStorage varStorage;
    public player playerRef;
    public CutsceneController cutsceneController;
    public DialogueRunner dialogueRunner;
    bool AttackTut;
    public YarnProgram tutnode;

 
    [YarnCommand ("subplayer")]
    public void SubscribeToPlayer()
    {
        //subscribe to different actions depending on the tut number
        int Tutnum = (int)varStorage.GetValue("$TutNum").AsNumber;
        Debug.Log("tutorial subscribed for tut num " + Tutnum);

        switch (Tutnum)
        {
            case 0:
                playerRef.AttackAction += CompletedTutorial;
                print("subscribing to player attack action");
                break;
            case 1:
                playerRef.DashCompleteAction += CompletedTutorial;
                print("subscribing to player dashaction");
                break;
            case 2:
                playerRef.ParryAction += CompletedTutorial;
                print("subscribing to parry action");
                break;
                    

        }
    }


    public void CompletedTutorial()
    {

        int Tutnum = (int)varStorage.GetValue("TutNum").AsNumber;
        Debug.Log("tutorial completed for tut num " +Tutnum);

        switch (Tutnum) {
            case 0:
                varStorage.SetValue("$CurrentTut", true);
                print("set AtkTut to true :" + varStorage.GetValue("$CurrentTut").AsBool);
                playerRef.AttackAction -= CompletedTutorial;
                print("unsubscribing AtkTut");
                //set the attack tutorial value to true in the storage
                break;
            case 1:
                varStorage.SetValue("$CurrentTut", true);
                print("set dash tutorial variable to true: " + varStorage.GetValue("$CurrentTut").AsBool);

                break;

        }


    }

    // Start is called before the first frame update
    void Start()
    {
        
        //cutsceneController.ActivateCutscene("img1");
        dialogueRunner.AddCommandHandler("ImgDisplay", cutsceneController.ActivateCutscene);
        dialogueRunner.AddCommandHandler("HideImg", cutsceneController.DeactivateCutscene);
        
        dialogueRunner.AddCommandHandler("RemoveAutoNode", (string[] p) => removeAuto());
    }

    // Update is called once per frame
     void Update()
    {
        
    }

    public void removeAuto()
    {
        dialogueRunner.startAutomatically = false;
        dialogueRunner.yarnScripts = null;
    }
    
}
