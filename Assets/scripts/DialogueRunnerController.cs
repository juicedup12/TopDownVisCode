using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;
using topdown;

//switch from speech bubble to text box
public class DialogueRunnerController : MonoBehaviour
{
    DialogueRunner runner;
    [SerializeField]
    LineView TextBox, SpeechBubble;
    [SerializeField]
    DialogueViewBase[] TextViews;
    [SerializeField]
    player player;

    // Start is called before the first frame update
    void Awake()
    {
        runner = GetComponent<DialogueRunner>();
        runner.AddCommandHandler("Textbox", SwitchToTextBox);
        runner.AddCommandHandler<GameObject>("SpeechBubble", SetSpeechBubble);
        runner.AddCommandHandler("test", () => print("test command handler called"));
        runner.AddCommandHandler("PlayerControl", () => player.SetUIControls(false));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartDialogue(string Dialogue)
    {
        foreach (DialogueViewBase dialogueView in runner.dialogueViews)
        {
            if (dialogueView is LineView)
            {
                LineView lineView = dialogueView as LineView;
                lineView.OnContinueClicked();
            }

        }
        runner.StartDialogue(Dialogue);

    }

    public void SwitchToTextBox()
    {
        print("switching to textbox");

        player.SetUIControls(true);
        runner.SetDialogueViews(TextViews);
    }

    public void SetSpeechBubble(GameObject Actor)
    {
        print("setting speech bubble view");
        if (Actor == null) return;
        if(runner.dialogueViews.Length >0)
        if(runner.dialogueViews[0] != null && runner.dialogueViews[0] == SpeechBubble)
        { 
            print("speech npc already assigned"); return; 
        }
        runner.SetDialogueViews(new DialogueViewBase[] { SpeechBubble });
        SpeechBubble.GetComponent<WorldUIConvert>().SetTarget = Actor.transform;
    }
}
