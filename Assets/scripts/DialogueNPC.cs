using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Yarn.Unity;


public class DialogueNPC : MonoBehaviour, Iinteractable
{
    [SerializeField]
    UnityEvent DialogueEvent;
    [SerializeField] 
    YarnProject yarnProject;
    [SerializeField]
    DialogueRunnerController RunnerController;

    [SerializeField]
    bool IsSpeechBubble;

    public void interact()
    {
        if(IsSpeechBubble)
        {
            //continue the dialogue

            RunnerController.StartDialogue("Interact");
        }
        //set dialogue to text box and provide a yarnporject
    }

    [YarnCommand("Action")]
    public void DialogueAction()
    {
        print("Dialogue Action called on NPC");
        DialogueEvent?.Invoke();
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
