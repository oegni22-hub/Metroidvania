using UnityEngine;

public class ControlAnimatorEnemies : MonoBehaviour
{
    public void GoblinLanzaAttack()
    {
        transform.parent.GetComponent<GoblinController>().LanzarLanza();
    }

    public void DemonioFireAttack()
    {
        transform.GetChild(0).gameObject.SetActive(true);
    }

    public void DemonioFireStop()
    {
        transform.GetChild(0).gameObject.SetActive(false);
    }
}
