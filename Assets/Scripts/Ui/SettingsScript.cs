using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class SettingsScript : MonoBehaviour
{
    public GameObject postProcessing;
    public Toggle postProcessingToggle;
    public void DeleteAllProgress()
    {
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene(0);
    }

    void Update()
    {
        postProcessing.SetActive(postProcessingToggle.isOn);
    }

    void OnApplicationQuit()  
    {  
        PlayerPrefs.Save();  
    }  
    
    void OnApplicationPause(bool pause)  
    {  
        if (pause)  
        {  
            PlayerPrefs.Save();  
        }  
    }
}
