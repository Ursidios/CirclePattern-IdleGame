using System.Collections.Generic;
using UnityEngine;

public class PlayerConfig : MonoBehaviour
{
    public int circleAmount;
    public GameObject circlePrefab;
    public PencilConfig pencilConfig;
    public List<GameObject> circlesInGameList;
    public Color[] circleColors;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        circleAmount = circlesInGameList.Count;

        if(Input.GetKeyDown("w"))
        {
            SpawnCircle();
        }
    }


    public void SpawnCircle()
    {
        GameObject newCircle = Instantiate(circlePrefab);
        newCircle.transform.localScale = circlesInGameList[circleAmount - 1].transform.localScale;
        

        circlesInGameList[circleAmount - 1].GetComponent<CircleDrawScript>().baseCircle = newCircle;
        circlesInGameList.Add(newCircle);
    }
}
