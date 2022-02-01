using UnityEngine;
using UnityEngine.UI;
using System;
using Yarn.Unity;
using Yarn;

public class CutsceneController : MonoBehaviour
{
    [Serializable]
    public struct NamedImage
    {
        public string name;
        public Sprite image;
    }
    public NamedImage[] pictures;
    public Image DisplayingImage;
    public DialogueRunner dialogueRunner;
    public YarnProgram tutnode;


    // Start is called before the first frame update
    void Start()
    {
        dialogueRunner.Add(tutnode);
        //dialogueRunner.StartDialogue("TestYarnStart");
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void ActivateCutscene(string[] imgName)
    {

        foreach(NamedImage img in pictures)
        {
            if(img.name == imgName[0])
            {
                DisplayingImage.gameObject.SetActive(true);
                DisplayingImage.sprite = img.image;
                return;
            }
        }
    }

    public void DeactivateCutscene(string[] strings)
    {
        DisplayingImage.gameObject.SetActive(false);
        DisplayingImage.sprite = null;
    }
}
