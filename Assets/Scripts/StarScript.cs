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
    private VibrationManager vibrationManager;
    public AudioSource unstableSound;
    public AudioSource collectSound;

    private float vibrationInterval;
    public float vibrationIntervalMax;

    public GameObject starParticle;
    private GameObject newStarParticle;

    void Start()
    {
        originalPosition = transform.position;
        playerConfig = FindAnyObjectByType<PlayerConfig>();
        starSpawner = FindAnyObjectByType<StarSpawner>();
        saveGameScript = FindAnyObjectByType<SaveGameScript>();
        vibrationManager = FindAnyObjectByType<VibrationManager>();
    }
    void OnEnable()
    {

        randomStarColors1 = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1f);
        randomStarColors2 = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1f);

        childrenStar1.GetComponent<SpriteRenderer>().color = randomStarColors1;
        childrenStar2.GetComponent<SpriteRenderer>().color = randomStarColors2;
        childrenStar1.GetComponent<TrailRenderer>().startColor = randomStarColors1;
        childrenStar2.GetComponent<TrailRenderer>().startColor = randomStarColors2;

        if (!isStable)
        {
            newStarParticle = Instantiate(starParticle, gameObject.transform.position, gameObject.transform.rotation);
        }
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
            unstableSound.Stop();

            if(newStarParticle != null)
            {
                Destroy(newStarParticle);
            }
        }
    }

    public void Shake()
    {
        Vector3 randomOffset = Random.insideUnitSphere * shakeIntensity;
        randomOffset.z = 0; // Manter a vibração apenas no plano X e Y
        transform.position = originalPosition + randomOffset; 


        vibrationInterval -= Time.deltaTime;
        if(vibrationInterval <= 0)
        {
            if(vibrationManager != null)
            {
                vibrationManager.StartVibration();
            }
            else
            {       
                unstableSound.Stop();
            }
            
            vibrationInterval = vibrationIntervalMax;
        }
    }

    void OnMouseDown()
    {
        if(!isStable)
        {
            playerConfig.moneySpecialMult += 0.02f;
            starSpawner.AddCollectedStar();
        }
        
        isStable = true;
    
        unstableSound.Stop();

        float randomPitch = Random.Range(0 ,100);
        if(randomPitch < 33)
        {
            collectSound.pitch = 1.5f;                           
        }
        else if(randomPitch < 66)
        {
            collectSound.pitch = 2; 
        }
        else if(randomPitch <= 100)
        {
            collectSound.pitch = 3; 
        }
        
        collectSound.Play();
        saveGameScript.SaveAll();
    }
}
