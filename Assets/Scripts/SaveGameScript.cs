using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;


public class SaveGameScript : MonoBehaviour
{
    private string cosmeticUpgradeSavePath;
    private string upgradeSavePath;
    private string playerSavePath;
    private string settingsSavePath;
    private PlayerConfig playerConfig;
    private UpgradeManager upgradeManager;
    private StarSpawner starSpawner;
    private CosmeticUpgradesManager cosmeticUpgradesManager;
    private UIScript uIScript;
    private SettingsScript settingsScript;
    private TutorialScript tutorialScript;

    void Start()
    {
        cosmeticUpgradeSavePath = Path.Combine(Application.persistentDataPath, "cosmeticUpgradeSavePath.json");
        upgradeSavePath = Path.Combine(Application.persistentDataPath, "upgradeData.json");
        playerSavePath = Path.Combine(Application.persistentDataPath, "playerData.json");
        settingsSavePath = Path.Combine(Application.persistentDataPath, "settingsSavePath.json");
        
        playerConfig = FindObjectOfType<PlayerConfig>();
        upgradeManager = FindObjectOfType<UpgradeManager>();
        starSpawner = FindObjectOfType<StarSpawner>();
        cosmeticUpgradesManager = FindObjectOfType<CosmeticUpgradesManager>();
        uIScript = FindObjectOfType<UIScript>();
        settingsScript = FindObjectOfType<SettingsScript>();
        tutorialScript = FindObjectOfType<TutorialScript>();

        LoadPlayer();
        LoadUpgrades();
        LoadCosmeticUpgrades();
        LoadSettings(); 
    }
    void Awake()
    {
          
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            LoadPlayer();
            LoadUpgrades();
            LoadCosmeticUpgrades();
            LoadSettings(); 
        }
    }
    public void SaveAll()
    {
        SaveCosmeticUpgrades();
        SaveUpgrades();
        SavePlayer();
        SaveSettings();
    }
    public void SaveCosmeticUpgrades()
    {
        CosmeticUpgradeData data = new CosmeticUpgradeData
        {
            starCount = starSpawner.collectedStars,
            drawCircleCount = cosmeticUpgradesManager.cosmeticUpgrades[0].level,
            shootingStarActivate = cosmeticUpgradesManager.isShootingStar,
            cosmeticUpgrades = cosmeticUpgradesManager.cosmeticUpgrades,
            isInfiniteMode = cosmeticUpgradesManager.isInfiniteMode
        };
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(cosmeticUpgradeSavePath, json);

    }
    public void LoadCosmeticUpgrades()
    {
        if (File.Exists(cosmeticUpgradeSavePath))
        {
            string json = File.ReadAllText(cosmeticUpgradeSavePath);
            CosmeticUpgradeData data = JsonUtility.FromJson<CosmeticUpgradeData>(json);
            cosmeticUpgradesManager.isInfiniteMode = data.isInfiniteMode;

            if (cosmeticUpgradesManager.isInfiniteMode)
            {
                foreach (var item in cosmeticUpgradesManager.cosmeticUpgrades)
                {
                    item.maxLevel = 1000;
                }

                cosmeticUpgradesManager.allUpgradesInMax = false;
            }
            cosmeticUpgradesManager.cosmeticUpgrades[1].level = data.cosmeticUpgrades[1].level;
            
            print(data.drawCircleCount + " 2");
            for (int i = 0; i < data.starCount; i++)
            {
                starSpawner.SpawnStar(true);
            }

            for (int i = 0; i < data.drawCircleCount; i++)
            {
                starSpawner.collectedStars = data.starCount;
                cosmeticUpgradesManager.isShootingStar = data.shootingStarActivate;
                cosmeticUpgradesManager.MoreDraw(true);
            }
            for (int i = 0; i < data.cosmeticUpgrades[1].level; i++)
            {
                cosmeticUpgradesManager.ShootingStar(true);
            }
        }
    }

    public void SaveUpgrades()
    {
        UpgradeData data = new UpgradeData
        {
            upgrades = upgradeManager.upgrades,
            trailDuration = PencilConfig.trailDuration,
            fieldOfView = upgradeManager.uiManager.MainCamera.fieldOfView,
            timeSpeedDuration = uIScript.timeSpeedTimerMax,
            isInfiniteMode = upgradeManager.isInfiniteMode
        };
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(upgradeSavePath, json);
    }

    public void LoadUpgrades()
    {
        if (File.Exists(upgradeSavePath))
        {
            string json = File.ReadAllText(upgradeSavePath);
            UpgradeData data = JsonUtility.FromJson<UpgradeData>(json);

            for (int i = 0; i < upgradeManager.upgrades.Length; i++)
            {
                upgradeManager.upgrades[i].moneyCost = data.upgrades[i].moneyCost;
                upgradeManager.upgrades[i].level = data.upgrades[i].level;
            }

            PencilConfig.trailDuration = data.trailDuration;
            upgradeManager.uiManager.MainCamera.fieldOfView = data.fieldOfView;
            uIScript.timeSpeedTimerMax = data.timeSpeedDuration;
            upgradeManager.isInfiniteMode = data.isInfiniteMode;
        }
    }

    public void SavePlayer()
    {
        List<Vector3> newCirclePositionList = new List<Vector3>();
        List<Quaternion> newCircleRotationList = new List<Quaternion>();

        foreach (var item in playerConfig.circlesInGameList)
        {
            newCirclePositionList.Add(item.transform.position);
            newCircleRotationList.Add(item.transform.rotation);
        }

        PlayerData data = new PlayerData
        {
            money = playerConfig.money,
            moneyMult = playerConfig.moneyMult,
            circleAmount = playerConfig.circleAmount,
            circleDrawAmount = playerConfig.circleDrawAmount,
            circles = new List<CircleData>(),
            drawCircles = new List<CircleData>(),
            moneySpecial = playerConfig.moneySpecial,
            moneySpecialMult = playerConfig.moneySpecialMult,
            isFirstTime = tutorialScript.isFirstTime,
            circlePositions = newCirclePositionList,
            circleRotations = newCircleRotationList
        };
        
        foreach (var circle in playerConfig.circlesInGameList)
        {
            data.circles.Add(new CircleData()
            {
                speed = circle.GetComponent<CircleDrawScript>().speed,
                rotationSpeed = circle.GetComponent<CircleDrawScript>().RotationSpeedMulti,
                scaleX = circle.transform.localScale.x,
                scaleY = circle.transform.localScale.y,
                scaleZ = circle.transform.localScale.z
            });
        }

        foreach (var drawCircle in playerConfig.drawCirclesInGameList)
        {
            data.drawCircles.Add(new CircleData()
            {
                speed = drawCircle.GetComponent<CircleDrawScript>().speed,
                rotationSpeed = drawCircle.GetComponent<CircleDrawScript>().RotationSpeedMulti
            });
        }

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(playerSavePath, json);
    }

    public void LoadPlayer()
    {
        if (File.Exists(playerSavePath))
        {
            string json = File.ReadAllText(playerSavePath);
            PlayerData data = JsonUtility.FromJson<PlayerData>(json);
            
            playerConfig.money = data.money;
            playerConfig.moneyMult = data.moneyMult;
            playerConfig.circleAmount = data.circleAmount;
            playerConfig.circleDrawAmount = data.circleDrawAmount;
            playerConfig.moneySpecial = data.moneySpecial;
            playerConfig.moneySpecialMult = data.moneySpecialMult;
            tutorialScript.isFirstTime = data.isFirstTime;
            
            for (int i = 0; i < data.circles.Count; i++)
            {
                Vector3 scale = new Vector3(data.circles[i].scaleX, data.circles[i].scaleY, data.circles[i].scaleZ);
                if (i != 0)
                {
                    playerConfig.SpawnCircle(scale, data.circlePositions[i], data.circleRotations[i]);
                }
                playerConfig.circlesInGameList[i].transform.localScale = scale;
                playerConfig.circlesInGameList[i].GetComponent<CircleDrawScript>().speed = data.circles[i].speed;
                playerConfig.circlesInGameList[i].GetComponent<CircleDrawScript>().RotationSpeedMulti = data.circles[i].rotationSpeed;
                playerConfig.circlesInGameList[i].gameObject.transform.position = data.circlePositions[i];
                playerConfig.circlesInGameList[i].gameObject.transform.rotation = data.circleRotations[i];
            }
            
            for (int i = 0; i < data.drawCircles.Count; i++)
            {
                playerConfig.drawCirclesInGameList[i].GetComponent<CircleDrawScript>().speed = data.drawCircles[i].speed;
                playerConfig.drawCirclesInGameList[i].GetComponent<CircleDrawScript>().RotationSpeedMulti = data.drawCircles[i].rotationSpeed;
            }
        }
    }
    
    void OnApplicationFocus(bool hasFocus)
    {
        if (!hasFocus) // Perdeu o foco, pode estar sendo fechado
        {
            SaveAll();
        }
    }
    public void DeleteSaveFiles()
    {
        if (File.Exists(upgradeSavePath))
        {
            File.Delete(upgradeSavePath);
            Debug.Log("Upgrade save file deleted.");
        }
        
        if (File.Exists(playerSavePath))
        {
            File.Delete(playerSavePath);
            Debug.Log("Player save file deleted.");
        }
        
        if (File.Exists(cosmeticUpgradeSavePath))
        {
            File.Delete(cosmeticUpgradeSavePath);
            Debug.Log("Cosmetic save file deleted.");
        }
    }

    public void SaveSettings()
    {
        bool[] newBoolArray = new bool[settingsScript.toggleComponents.Length];

        for (int i = 0; i < settingsScript.toggleComponents.Length; i++)
        {
            newBoolArray[i] = settingsScript.toggleComponents[i].isOn;
        }

        SettingsData data = new SettingsData
        {
            toggleSettings = newBoolArray


        };


        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(settingsSavePath, json);
    }

    public void LoadSettings()
    {
        if (File.Exists(settingsSavePath))
        {
            string json = File.ReadAllText(settingsSavePath);
            SettingsData data = JsonUtility.FromJson<SettingsData>(json);

            // bool[] newBoolArray = new bool[data.toggleSettings.Length];


            //     newBoolArray = data.toggleSettings;
            

            settingsScript.UpdateObj(data.toggleSettings);
        }
    }
}
[Serializable]
public class CosmeticUpgradeData
{
    public int starCount;
    public int drawCircleCount;
    public bool shootingStarActivate;
    public CosmeticUpgrades[] cosmeticUpgrades;
    public bool isInfiniteMode;
}

[Serializable]
public class UpgradeData
{
    public Upgrades[] upgrades;
    public float fieldOfView;
    public float trailDuration;
    public float timeSpeedDuration;
    public bool isInfiniteMode;

}
[Serializable]
public class PlayerData
{
    public float money;
    public float moneyMult;
    public int circleAmount;
    public int circleDrawAmount;
    public List<CircleData> circles;
    public List<CircleData> drawCircles;
    public List<Vector3> circlePositions;
    public List<Quaternion> circleRotations;
    public float moneySpecial;
    public float moneySpecialMult;
    public bool isFirstTime;
}

[Serializable]
public class CircleData
{
    public float speed;
    public float rotationSpeed;
    public float scaleX;
    public float scaleY;
    public float scaleZ;
}

[Serializable]
public class SettingsData
{
    public bool[] toggleSettings;
}