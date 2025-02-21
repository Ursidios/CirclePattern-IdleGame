using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


public class SaveGameScript : MonoBehaviour
{
    private string cosmeticUpgradeSavePath;
    private string upgradeSavePath;
    private string playerSavePath;
    private PlayerConfig playerConfig;
    private UpgradeManager upgradeManager;
    private StarSpawner starSpawner;

    void Start()
    {
        cosmeticUpgradeSavePath = Path.Combine(Application.persistentDataPath, "cosmeticUpgradeSavePath.json");
        upgradeSavePath = Path.Combine(Application.persistentDataPath, "upgradeData.json");
        playerSavePath = Path.Combine(Application.persistentDataPath, "playerData.json");
        playerConfig = FindObjectOfType<PlayerConfig>();
        upgradeManager = FindObjectOfType<UpgradeManager>();
        starSpawner = FindObjectOfType<StarSpawner>();
        LoadPlayer();
        LoadUpgrades();
        LoadCosmeticUpgrades();
    }

    public void SaveAll()
    {
        SaveCosmeticUpgrades();
        SaveUpgrades();
        SavePlayer();
    }
    public void SaveCosmeticUpgrades()
    {
        CosmeticUpgradeData data = new CosmeticUpgradeData
        {
            starCount = starSpawner.collectedStars,            
        };
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(cosmeticUpgradeSavePath, json);
    }
    public void LoadCosmeticUpgrades()
    {
        if (File.Exists(upgradeSavePath))
        {
            string json = File.ReadAllText(cosmeticUpgradeSavePath);
            CosmeticUpgradeData data = JsonUtility.FromJson<CosmeticUpgradeData>(json);
            
            for (int i = 0; i < data.starCount; i++)
            {
                starSpawner.SpawnStar(true);
            }
            starSpawner.collectedStars = data.starCount;
            
        }
    }

    public void SaveUpgrades()
    {
        UpgradeData data = new UpgradeData
        {
            upgrades = upgradeManager.upgrades,
            trailDuration = PencilConfig.trailDuration,
            fieldOfView = upgradeManager.uiManager.MainCamera.fieldOfView
            
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
                upgradeManager.upgrades[i].maxLevel = data.upgrades[i].maxLevel;
                upgradeManager.upgrades[i].moneyPercentageIncrease = data.upgrades[i].moneyPercentageIncrease;
                upgradeManager.upgrades[i].inflationPercentageCost = data.upgrades[i].inflationPercentageCost;
            }
            
            PencilConfig.trailDuration = data.trailDuration;
            upgradeManager.uiManager.MainCamera.fieldOfView = data.fieldOfView;
        }
    }

    public void SavePlayer()
    {
        PlayerData data = new PlayerData
        {
            money = playerConfig.money,
            moneyMult = playerConfig.moneyMult,
            circleAmount = playerConfig.circleAmount,
            circleDrawAmount = playerConfig.circleDrawAmount,
            circles = new List<CircleData>(),
            drawCircles = new List<CircleData>(),
            moneySpecial = playerConfig.moneySpecial,
            moneySpecialMult = playerConfig.moneySpecialMult
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
            
            for (int i = 0; i < data.circles.Count; i++)
            {
                Vector3 scale = new Vector3(data.circles[i].scaleX, data.circles[i].scaleY, data.circles[i].scaleZ);
                if (i != 0)
                {
                    playerConfig.SpawnCircle(scale);
                }
                playerConfig.circlesInGameList[i].transform.localScale = scale;
                playerConfig.circlesInGameList[i].GetComponent<CircleDrawScript>().speed = data.circles[i].speed;
                playerConfig.circlesInGameList[i].GetComponent<CircleDrawScript>().RotationSpeedMulti = data.circles[i].rotationSpeed;
            }
            
            for (int i = 0; i < data.drawCircles.Count; i++)
            {
                if (i != 0)
                {
                    playerConfig.SpawnDrawCircle();
                }
                playerConfig.drawCirclesInGameList[i].GetComponent<CircleDrawScript>().speed = data.drawCircles[i].speed;
                playerConfig.drawCirclesInGameList[i].GetComponent<CircleDrawScript>().RotationSpeedMulti = data.drawCircles[i].rotationSpeed;
            }
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
    }
}
[Serializable]
public class CosmeticUpgradeData
{
    public int starCount;

}

[Serializable]
public class UpgradeData
{
    public Upgrades[] upgrades;
    public float fieldOfView;
    public float trailDuration;

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
    public float moneySpecial;
    public float moneySpecialMult;
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

