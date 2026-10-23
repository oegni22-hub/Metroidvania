using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    private Transform cam;
    [SerializeField] private float percentaje;
    private Vector3 previousPos;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cam = Camera.main.transform;
    }

    // Update is called once per frame
    public void UpdateParallaxEffect()
    {
        Vector3 deltaPos = cam.position - previousPos;
        transform.Translate(deltaPos * percentaje, Space.World);
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        previousPos = cam.position;
    }
}
