
using UnityEngine;

public class EnemyAnimationEvents : MonoBehaviour
{
    private EnemyController _enemyController;
    // Start is called before the first frame update
    void Start()
    {
        _enemyController = GetComponentInParent<EnemyController>();
    }

    private void Bite() 
    {
        _enemyController.BiteAction();
    }
}
