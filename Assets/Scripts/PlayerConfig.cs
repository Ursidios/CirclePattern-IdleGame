using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PlayerConfig : MonoBehaviour
{
    public float money;
    public float moneyMult;
    float accumulatedTime = 0f;
    [Space(20)]
    [Header("New Circle Config")]
    public int circleAmount;
    public int circleDrawAmount;
    public GameObject circlePrefab;
    public GameObject circleDrawPrefab;
    public PencilConfig pencilConfig;
    public List<GameObject> circlesInGameList;
    public List<GameObject> drawCirclesInGameList;
    public Color[] circleColors;
    public int colorSelector;
    private string savePath;

    void Start()
    {
        savePath = Path.Combine(Application.persistentDataPath, "playerData.json");
        LoadPlayerPrefs();
        
        circleAmount = circlesInGameList.Count;
        circleDrawAmount = drawCirclesInGameList.Count;
    }

    public void LoadPlayerPrefs()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            PlayerData data = JsonUtility.FromJson<PlayerData>(json);
            
            money = data.money;
            moneyMult = data.moneyMult;
            circleAmount = data.circleAmount;
            circleDrawAmount = data.circleDrawAmount;
            
            for (int i = 0; i < data.circles.Count; i++)
            {
                Vector3 scale = new Vector3(data.circles[i].scaleX, data.circles[i].scaleY, data.circles[i].scaleZ);
                if (i != 0)
                {
                    SpawnCircle(scale);
                }
                circlesInGameList[i].transform.localScale = scale;
                circlesInGameList[i].GetComponent<CircleDrawScript>().speed = data.circles[i].speed;
                circlesInGameList[i].GetComponent<CircleDrawScript>().RotationSpeedMulti = data.circles[i].rotationSpeed;
            }

            for (int i = 0; i < data.drawCircles.Count; i++)
            {
                if (i != 0)
                {
                    SpawnDrawCircle();
                }
                drawCirclesInGameList[i].GetComponent<CircleDrawScript>().speed = data.drawCircles[i].speed;
                drawCirclesInGameList[i].GetComponent<CircleDrawScript>().RotationSpeedMulti = data.drawCircles[i].rotationSpeed;
            }
        }
    }

    public void SavePlayerPrefs()
    {
        PlayerData data = new PlayerData();
        data.money = money;
        data.moneyMult = moneyMult;
        data.circleAmount = circleAmount;
        data.circleDrawAmount = circleDrawAmount;

        data.circles = new List<CircleData>();
        foreach (var circle in circlesInGameList)
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

        data.drawCircles = new List<CircleData>();
        foreach (var drawCircle in drawCirclesInGameList)
        {
            data.drawCircles.Add(new CircleData()
            {
                speed = drawCircle.GetComponent<CircleDrawScript>().speed,
                rotationSpeed = drawCircle.GetComponent<CircleDrawScript>().RotationSpeedMulti
            });
        }

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(savePath, json);
    }

    void Update()
    {
        MoneyRules(moneyMult);
    }

    public void IncreaseMoneyMult(float amount)
    {
        moneyMult += amount;
        SavePlayerPrefs();
    }

    public void MoneyRules(float mult)
    {
        money += Time.deltaTime * mult;
    }

    public void SpawnCircle(Vector3 circleScale)
    {
        GameObject newCircle = Instantiate(circlePrefab, Vector3.zero, Quaternion.identity);

        if (circleScale == new Vector3(1,1,1))
        {
            newCircle.transform.localScale = circlesInGameList[circleAmount - 1].transform.localScale + circleScale;
        }
        else
        {
            newCircle.transform.localScale = circleScale;
        }
        
        newCircle.GetComponent<SpriteRenderer>().color = circleColors[colorSelector];
        newCircle.GetComponent<CircleDrawScript>().speed = 1;
        newCircle.GetComponent<CircleDrawScript>().RotationSpeedMulti = 1;
        
        circlesInGameList.Add(newCircle);
        circleAmount = circlesInGameList.Count;
        circlesInGameList[circleAmount - 2].GetComponent<CircleDrawScript>().baseCircle = newCircle;
        colorSelector = (colorSelector + 1) % circleColors.Length;
    }

    public void SpawnDrawCircle()
    {
        GameObject newCircle = Instantiate(circleDrawPrefab, Vector3.zero, Quaternion.identity);
        newCircle.GetComponent<CircleDrawScript>().speed = 1;
        newCircle.GetComponent<CircleDrawScript>().RotationSpeedMulti = 1;

        circleDrawAmount = drawCirclesInGameList.Count;
        drawCirclesInGameList.Add(newCircle);
    }
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
