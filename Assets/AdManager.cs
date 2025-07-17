using System.Threading;
using UnityEngine;

public class AdManager : MonoBehaviour
{
    public GameObject Ad;

    private float TimerSpawn;
    public float TimerSpawnStart;
    private float TimerDisable;
    public float TimerDisableStart;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        TimerSpawn = TimerSpawnStart;
        TimerDisable = TimerDisableStart;
    }

    // Update is called once per frame
    void Update()
    {
        if (!Ad.activeSelf)
        {
            TimerSpawn -= Time.deltaTime;
        }
        else
        {
            TimerDisable -= Time.deltaTime;
        }

        if (TimerSpawn <= 0)
        {
            Ad.SetActive(true);
            TimerSpawn = TimerSpawnStart;
        }

        if (TimerDisable <= 0)
        {
            Ad.SetActive(false);
            TimerDisable = TimerDisableStart;
        }
    }
}
