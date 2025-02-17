using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class SettingsScript : MonoBehaviour
{
    public GameObject postProcessing;
    public Toggle postProcessingToggle;
    public SaveGameScript saveGameScript;
    public void DeleteAllProgress()
    {
        saveGameScript.DeleteSaveFiles();
        SceneManager.LoadScene(0);
    }

    void Update()
    {
        postProcessing.SetActive(postProcessingToggle.isOn);
    }

    void OnApplicationQuit()  
    {  
        saveGameScript.SaveAll();
    }  
    
    void OnApplicationPause(bool pause)  
    {  
        if (pause)  
        {  
            saveGameScript.SaveAll();
        }  
    }
}
