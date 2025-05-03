using UnityEngine;

public class CircleDrawScript : MonoBehaviour
{
    public GameObject baseCircle;
    public GameObject drawPencil;
    public float baseCircleRadius;
    public float speed = 1f;
    public float RotationSpeedMulti;
    public Vector3 center = Vector3.zero;
    public float angle = 0f;  
    
    void Start()
    {
        if (baseCircle != null)
        {
            center = baseCircle.transform.position;
            baseCircleRadius = baseCircle.transform.localScale.x / 2 - gameObject.transform.localScale.x / 2;

            Vector3 offset = transform.position - center;
            angle = Mathf.Atan2(offset.y, offset.x); // ângulo inicial com base na posição
        }
    }
    void FixedUpdate()
    {
        if(baseCircle != null)
        {
            center = baseCircle.transform.position;
            baseCircleRadius = baseCircle.transform.localScale.x / 2 - gameObject.transform.localScale.x / 2;
        
        }
        else
        {
            baseCircleRadius = 0;
        }

        angle += speed * Time.fixedDeltaTime;
        //if (angle >= 360f) angle -= 360f;

        // Calcula a posição circular
        float x = center.x + Mathf.Cos(angle) * baseCircleRadius;
        float y = center.y + Mathf.Sin(angle) * baseCircleRadius;

        // Atualiza a posição do objeto
        transform.position = new Vector3(x, y, transform.position.z);

        // Calcula a rotação do objeto no próprio eixo
        float rotationSpeed = speed / baseCircleRadius * RotationSpeedMulti; // Velocidade proporcional ao raio
        transform.Rotate(Vector3.forward, -rotationSpeed * Time.fixedDeltaTime * Mathf.Rad2Deg);

    }
}
