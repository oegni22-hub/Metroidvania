using UnityEngine;

public class ParticlesCallBack : MonoBehaviour
{
    public void OnParticleSystemStopped()
    {
        string temp = transform.parent.parent.GetComponent<Gema>().message;
        GameObject.Find("LevelManager").GetComponent<LevelManager>().ShowGemMessage(temp);
        Destroy(transform.parent.parent.gameObject);
    }
}
