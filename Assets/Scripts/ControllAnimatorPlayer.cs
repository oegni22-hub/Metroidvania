using UnityEngine;

public class ControllAnimatorPlayer : MonoBehaviour
{
    private PlayerController player;

    void Start()
    {
        player = transform.parent.GetComponent<PlayerController>();
    }

    public void FinishAttack1()
    {
        player.FinishAttack1();
    }

    public void FinishAttack2()
    {
        player.FinishAttack2();
    }
}
