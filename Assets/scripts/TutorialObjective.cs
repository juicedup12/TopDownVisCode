using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;
using TMPro;

public class TutorialObjective : MonoBehaviour
{
    float time;
    [SerializeField]
    TextMeshProUGUI TextMesh;
    [SerializeField] int TrackedObjects;
    [SerializeField] InMemoryVariableStorage YarnVariables;
    [SerializeField] DialogueRunnerController runnerController;

    // Start is called before the first frame update
    void Start()
    {
        time = 0;
        TextMesh.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        TextMesh.text = FormatTime(time * 1000);
    }

    public string FormatTime(float time)
    {
        int minutes = (int)time / 60000;
        int seconds = (int)time / 1000 - 60 * minutes;
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void RemoveTrackedObject()
    {
        TrackedObjects--;

        print("tracked object removed, remaining : " + TrackedObjects);
        if (TrackedObjects < 1)
        {
            ObjectsCleared();
        }
    }

    void ObjectsCleared()
    {
        YarnVariables.SetValue("$Finished", true);
        YarnVariables.SetValue("$FinishTime", time);
        runnerController.StartDialogue("TutBubble");
        gameObject.SetActive(false);
    }

}
