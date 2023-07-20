using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;
using topdown;
using UnityEngine.Events;

//class for starting dialogue and listening to player input
public class DialoguePlayerListener : MonoBehaviour
{

    [SerializeField]
    DialogueRunner Dialogue;
    [SerializeField]
    UnityEvent DialogueCommand;

    // Start is called before the first frame update
    void Start()
    {
    }

    private void OnDisable()
    {
        Dialogue.onDialogueComplete.RemoveAllListeners();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartDialogue(string Node)
    {

        //Dialogue.Stop();
        //_player.SetWait();
        //Dialogue.StartDialogue(Node);
        //Dialogue.onDialogueComplete.AddListener( () => _player.SetWait());
        //Dialogue.AddCommandHandler("invoke", () => DialogueCommand.Invoke());
        
    }

}
