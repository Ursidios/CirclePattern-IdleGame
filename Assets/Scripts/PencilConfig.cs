using UnityEngine;

public class PencilConfig : MonoBehaviour
{
    public TrailRenderer trailRenderer;
    private SpriteRenderer pencilCircle;
    public Color color;
    public bool isMainDraw;
    public static float trailDuration;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        trailRenderer = gameObject.GetComponent<TrailRenderer>();
        pencilCircle = gameObject.GetComponent<SpriteRenderer>();

        trailDuration = 0.5f;

        if(isMainDraw)
        {
            trailRenderer.enabled = true;
        }
        else
        {
           trailRenderer.enabled = false; 
        }

        trailRenderer.time = trailDuration;
    }
    public void OnEnable()
    {
        trailRenderer.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        trailRenderer.startColor = color;
        pencilCircle.color = color;
        trailRenderer.time = trailDuration;
    
    }
}
