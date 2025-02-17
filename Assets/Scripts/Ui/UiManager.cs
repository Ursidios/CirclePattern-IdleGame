using System;
using UnityEngine;

public class UiManager : MonoBehaviour
{
    public GameObject shopUI;
    public GameObject gameUI;
    public GameObject settingsUI;

    [Space (20)]
    [Header ("Camera Zoom Config")]
    public Camera MainCamera;
    public float zoomSpeed = 5f;
    private float targetFOV;
    public float zoomAmount;

    public PlayerConfig playerConfig;
    public UpgradeManager upgradeManager;


    public void EnableShopUI()
    {
        shopUI.SetActive(true);
        //gameUI.SetActive(false);
        settingsUI.GetComponent<Animator>().Play("CloseScreenAnimation");
    }
    public void EnableGameUI()
    {
        //gameUI.SetActive(true);
        shopUI.GetComponent<Animator>().Play("CloseScreenAnimation");
        settingsUI.GetComponent<Animator>().Play("CloseScreenAnimation");
    }
    public void EnableSettingsUI()
    {
        settingsUI.SetActive(true);
        //gameUI.SetActive(false);
        shopUI.GetComponent<Animator>().Play("CloseScreenAnimation");
    }
    
    public void HideCircles()
    {
        foreach(var circle in playerConfig.circlesInGameList)
        {
            circle.GetComponent<SpriteRenderer>().enabled = false;
        }

        foreach(var circle in playerConfig.drawCirclesInGameList)
        {
            circle.GetComponent<SpriteRenderer>().enabled = false;
        }
    }
    public void ShowCircles()
    {
        foreach(var circle in playerConfig.circlesInGameList)
        {
            circle.GetComponent<SpriteRenderer>().enabled = true;
        }

        foreach(var circle in playerConfig.drawCirclesInGameList)
        {
            circle.GetComponent<SpriteRenderer>().enabled = true;
        }
    }
    void Start()
    {
        if (MainCamera == null)
            MainCamera = Camera.main;
        targetFOV = MainCamera.fieldOfView;
    }

    void Update()
    {
        MainCamera.fieldOfView = Mathf.Lerp(MainCamera.fieldOfView, targetFOV, Time.deltaTime * zoomSpeed);

        if(targetFOV < 39.1003f)
        {
            targetFOV = 39.1003f;
        }
        if(targetFOV > 165.1006f)
        {
            targetFOV = 165.1006f;
        }
    
    }

    public void ZoomInButton()
    {
        targetFOV -= zoomAmount;

        upgradeManager.SavePlayerPrefs();
        playerConfig.SavePlayerPrefs();
        //upgradeManager.SavePlayerData();
    }

    public void ZoomOutButton()
    {
        targetFOV += zoomAmount;

        upgradeManager.SavePlayerPrefs();
        playerConfig.SavePlayerPrefs();
        //upgradeManager.SavePlayerData();
    }
}
