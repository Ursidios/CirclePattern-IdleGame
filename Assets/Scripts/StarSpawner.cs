using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StarSpawner : MonoBehaviour
{
    public GameObject StarPrefab;
    private float timerToSpawn;
    public float timerToSpawnMax;
    public Vector2 spawnMinPosition;
    public Vector2 spawnMaxPosition;
    public List<GameObject> allStars;
    public int collectedStars;
    public int maxVisibleStars;
    public TutorialScript tutorialScript;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {   
        timerToSpawn = timerToSpawnMax;
    }

    // Update is called once per frame
    void Update()
    {
        timerToSpawn -= Time.deltaTime;

        if(timerToSpawn <= 0)
        {
            timerToSpawn = timerToSpawnMax;
            SpawnStar(false);
        }

        if(collectedStars >= maxVisibleStars)
        {
            allStars[0].GetComponent<StarScript>().StartStarDie();
            allStars.Remove(allStars[0]);
            collectedStars -= 1;
        }
    }
    public void AddCollectedStar()
    {
        collectedStars ++;
    }
    public void SpawnStar(bool isCollected)
    {
        float x = Random.Range(spawnMinPosition.x, spawnMaxPosition.x);
        float y = Random.Range(spawnMinPosition.y, spawnMaxPosition.y);

        GameObject newStar = Instantiate(StarPrefab, new Vector3(x, y, 0), Quaternion.identity);
        
        float randomScale = Random.Range(2, 3);
        newStar.transform.localScale = new Vector3(randomScale, randomScale, randomScale);
       
        newStar.GetComponent<StarScript>().isStable = isCollected;
        allStars.Add(newStar);


        if(!tutorialScript.isInStarTutorial)
        {
            tutorialScript.UpdateTutorialLevel(3);
            tutorialScript.firstStar = newStar;
        }
    }
}
