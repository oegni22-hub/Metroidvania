using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Vector3 offset;
    [SerializeField] 
    [Tooltip("X= valor de x min, Y= valor de x max")] 
    private Vector2 limitX;
    [SerializeField] 
    [Tooltip("X= valor de y min, Y= valor de y max")] 
    private Vector2 limitY;
    
    [SerializeField]
    private ParallaxEffect[] parallaxEffect;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    void LateUpdate()
    {
        float x = Mathf.Clamp(player.position.x + offset.x, limitX.x, limitX.y);
        float y = Mathf.Clamp(player.position.y + offset.y, limitY.x, limitY.y);
        float z = player.position.z + offset.z;
        
        transform.position = new Vector3(x, y, z);
        //Actualizar la posicion parallax
        /*foreach (ParallaxEffect temp in parallaxEffect)
        {
            temp.UpdateParallaxEffect();
        }*/

        for (int i = 0; i < parallaxEffect.Length; i++)
        {
            parallaxEffect[i].UpdateParallaxEffect();
        }
    }
}
