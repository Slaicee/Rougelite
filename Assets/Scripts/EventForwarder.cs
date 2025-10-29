using UnityEngine;

public class EventForwarder : MonoBehaviour
{
    private EnemyAI parentEnemyAI; // 父物体的EnemyAI

    void Start()
    {
        // 查找父物体（或祖先）上的EnemyAI组件
        parentEnemyAI = GetComponentInParent<EnemyAI>();
        if (parentEnemyAI == null)
        {
            Debug.LogError("子物体找不到父物体的EnemyAI组件！", this);
        }
    }

    // 动画事件直接调用这个方法（子物体上的方法）
    public void DealDamage()
    {
        // 转发给父物体的EnemyAI
        parentEnemyAI?.DealDamage();
    }
}