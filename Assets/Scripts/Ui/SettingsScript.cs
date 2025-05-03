using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class SettingsScript : MonoBehaviour
{
    //public bool[] toggleBool;
    public Toggle[] toggleComponents;
    public GameObject[] toggleAssociatedGameObject;

    public SaveGameScript saveGameScript;
    public void DeleteAllProgress()
    {
        saveGameScript.DeleteSaveFiles();
        SceneManager.LoadScene(0);
        PencilConfig.trailDuration = 0.5f;
    }
    void Start()
    {
        Application.targetFrameRate = 60; // ou 90, 120, ou -1 para ilimitado
        QualitySettings.vSyncCount = 0;   // desativa o VSync (essencial no Android)
    }
    void Awake()
    {

    }
    void Update()
    {
        for (int i = 0; i < toggleComponents.Length; i++)
        {
            toggleAssociatedGameObject[i].SetActive(toggleComponents[i].isOn);
        }
    }

    public void UpdateObj(bool[] toggleBools)
    {

        for (int i = 0; i < toggleComponents.Length; i++)
        {
            toggleComponents[i].isOn = toggleBools[i];
        }

    }
}
