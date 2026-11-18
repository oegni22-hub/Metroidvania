using System.Collections;
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

    private bool shaking;
    [SerializeField] private float duration, fuerza, frecuency, smoothness;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            StartCoroutine(ShakeCamara());
        }
    }

    void LateUpdate()
    {
        if (shaking)
        {
            return;
        }
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

    public void StartShake(float _duration)
    {
        duration = _duration;
        StartCoroutine(ShakeCamara());
    }

    IEnumerator ShakeCamara(/*float duration, float fuerza, float frecuency, float smoothness*/)
    {
        shaking = true;
        float timePass = 0;
        float timeFrecuency = frecuency+1;
        
        float DistanciaTargetX = 0;
        float DistanciaTargetY = 0;

        while (timePass < duration)
        {
            if (timeFrecuency >= frecuency)//Ya ha pasado el tiempo de frecuencia y hay que cambiar de objetivo del shake
            {
                DistanciaTargetX = Random.Range(-fuerza, fuerza);
                DistanciaTargetY = Random.Range(-fuerza, fuerza);
                timeFrecuency = 0;
            }
            
            float x = Mathf.Clamp(player.position.x + offset.x, limitX.x, limitX.y);
            float y = Mathf.Clamp(player.position.y + offset.y, limitY.x, limitY.y);
            float z = player.position.z + offset.z;
            Vector3 initialPos = new Vector3(x, y, z);
            
            transform.position = Vector3.Lerp(initialPos, new Vector3(initialPos.x + DistanciaTargetX, initialPos.y + DistanciaTargetY, initialPos.z), smoothness * Time.deltaTime);
            timePass += Time.deltaTime;
            timeFrecuency += Time.deltaTime;
            
            yield return null;
        }
        shaking = false;
    }
}
