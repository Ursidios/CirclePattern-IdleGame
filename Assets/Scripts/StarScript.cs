using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;

public class StarScript : MonoBehaviour
{
    public bool isStable;
    public float rotationSpeed = 100f; // Velocidade da rotação
    public float shakeIntensity = 0.1f; // Intensidade da vibração
    public float shakeDuration = 0.5f; // Duração da vibração
    private Vector3 originalPosition;

    public GameObject childrenStar1;
    public GameObject childrenStar2;

    public Color randomStarColors1;
    public Color randomStarColors2;

    public PlayerConfig playerConfig;
    private StarSpawner starSpawner;
    private SaveGameScript saveGameScript;

    void Start()
    {
        originalPosition = transform.position;
        playerConfig = FindAnyObjectByType<PlayerConfig>();
        starSpawner = FindAnyObjectByType<StarSpawner>();
        saveGameScript = FindAnyObjectByType<SaveGameScript>();
    }
    void OnEnable()
    {

        randomStarColors1 = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1f);
        randomStarColors2 = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1f);

        childrenStar1.GetComponent<SpriteRenderer>().color = randomStarColors1;
        childrenStar2.GetComponent<SpriteRenderer>().color = randomStarColors2;
        childrenStar1.GetComponent<TrailRenderer>().startColor = randomStarColors1;
        childrenStar2.GetComponent<TrailRenderer>().startColor = randomStarColors2;

    }
    void Update()
    {
        if (!isStable)
        {
            Shake();
        }
        else
        {
            transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
        }
    }

    public void Shake()
    {
        Vector3 randomOffset = Random.insideUnitSphere * shakeIntensity;
        randomOffset.z = 0; // Manter a vibração apenas no plano X e Y
        transform.position = originalPosition + randomOffset; 
    }

    void OnMouseDown()
    {
        if(!isStable)
        {
            playerConfig.moneySpecialMult += 0.2f;
            starSpawner.AddCollectedStar();
        }
        isStable = true;
        saveGameScript.SaveAll();
    }
}
