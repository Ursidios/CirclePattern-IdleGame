using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class SettingsScript : MonoBehaviour
{
    public ToggleSettings[] toggleSettings;

    public SaveGameScript saveGameScript;
    public void DeleteAllProgress()
    {
        saveGameScript.DeleteSaveFiles();
        SceneManager.LoadScene(0);
        PencilConfig.trailDuration = 0.5f;
    }

    void Awake()
    {

    }
    void Update()
    {

    }

    public void UpdateObj()
    {
        foreach (var item in toggleSettings)
        {
            item.associatedGameObject.SetActive(item.toggle.isOn);
        }
    }

    public void UpdateToggles()
    {
        foreach (var item in toggleSettings)
        {
            item.isOn = item.toggle.isOn;
        }
    }
}
[Serializable]
public class ToggleSettings
{
    public string name;
    public Toggle toggle;
    public bool isOn;
    public GameObject associatedGameObject;
}
