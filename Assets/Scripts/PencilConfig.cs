using UnityEngine;

public class PencilConfig : MonoBehaviour
{
    public TrailRenderer trailRenderer;
    private SpriteRenderer pencilCircle;
    public Color color;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        trailRenderer = gameObject.GetComponent<TrailRenderer>();
        pencilCircle = gameObject.GetComponent<SpriteRenderer>();
        trailRenderer.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        trailRenderer.startColor = color;
        pencilCircle.color = color;
    }
}
