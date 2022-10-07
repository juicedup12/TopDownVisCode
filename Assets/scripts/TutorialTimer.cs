using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TutorialTimer : MonoBehaviour
{
    float time;
    [SerializeField]
    TextMeshProUGUI TextMesh;
    [SerializeField] int TrackedObjects;

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
            gameObject.SetActive(false);
        }
    }

}
