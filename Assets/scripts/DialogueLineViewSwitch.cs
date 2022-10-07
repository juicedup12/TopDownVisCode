using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

//switch from speech bubble to text box
public class DialogueLineViewSwitch : MonoBehaviour
{
    DialogueRunner runner;
    [SerializeField]
    LineView TextBox, SpeechBubble;
    [SerializeField]
    DialogueViewBase[] TextViews;

    // Start is called before the first frame update
    void Start()
    {
        runner = GetComponent<DialogueRunner>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SwitchToTextBox()
    {
        //runner.dialogueViews[0] = TextBox;
        SpeechBubble.OnContinueClicked();
        runner.SetDialogueViews(TextViews);
    }

    public void SwitchToSpeechBubble()
    {
        runner.SetDialogueViews(new DialogueViewBase[] { SpeechBubble });
    }
}
