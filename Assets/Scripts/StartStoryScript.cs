using UnityEngine;

public class StartStoryScript : MonoBehaviour
{

    public TutorialScript tutorialScript;
    public GameObject startStoryObj;
    public static bool storyOn;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (!tutorialScript.isFirstTime)
            startStoryObj.SetActive(false);
        else
        {
            if (startStoryObj.activeSelf)
            {
                storyOn = true;
            }
            else
            {
                storyOn = false;
            }
        }
    }
}
