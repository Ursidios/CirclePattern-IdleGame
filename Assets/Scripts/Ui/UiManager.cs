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
    public SaveGameScript saveGameScript;

    public bool isStart;
    private bool pinchDetected = false; // Controle para evitar chamadas repetidas

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
        MainCamera.fieldOfView = 171;
    }

    void Update()
    {
        if(isStart)
        {
            MainCamera.fieldOfView = Mathf.Lerp(MainCamera.fieldOfView, targetFOV, Time.deltaTime * 2);
            if(targetFOV >= MainCamera.fieldOfView)
            {
                isStart = false;
            }
        }
        else
        {
            MainCamera.fieldOfView = Mathf.Lerp(MainCamera.fieldOfView, targetFOV, Time.deltaTime * zoomSpeed);
        }

        if(targetFOV < 39.1003f)
        {
            targetFOV = 39.1003f;
        }
        if(targetFOV > 171)
        {
            targetFOV = 171;
        }

        if (Input.touchCount == 2) // Detecta dois toques na tela
        {
            Touch touch1 = Input.GetTouch(0);
            Touch touch2 = Input.GetTouch(1);

            // Calcula a distância entre os dois toques na frame atual e na anterior
            float prevDistance = (touch1.position - touch1.deltaPosition - (touch2.position - touch2.deltaPosition)).magnitude;
            float currentDistance = (touch1.position - touch2.position).magnitude;

            if (!pinchDetected)
            {
                if (currentDistance > prevDistance) // Pinça se afastando (zoom in)
                {
                    ZoomInButton();
                    pinchDetected = true;
                }
                else if (currentDistance < prevDistance) // Pinça se aproximando (zoom out)
                {
                    ZoomOutButton();
                    pinchDetected = true;
                }
            }
        }
        else
        {
            pinchDetected = false; // Reseta quando os dedos são levantados
        }
    }

    public void ZoomInButton()
    {
        targetFOV -= zoomAmount;

        saveGameScript.SaveAll();
        isStart = false;
    }

    public void ZoomOutButton()
    {
        targetFOV += zoomAmount;

        saveGameScript.SaveAll();
        isStart = false;
    }
}
