using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float lifeTime = 3f;
    public float damage = 1f;

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    // 子弹与碰撞物体触发时
    void OnTriggerEnter(Collider other)
    {
        // 检查子弹是否与敌人碰撞
        if (other.CompareTag("Enemy"))
        {
            EnemyAI enemy = other.GetComponent<EnemyAI>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage); // 伤害敌人
            }
        }

        // 检查子弹是否与矿石碰撞
        if (other.CompareTag("Ore"))
        {
            Ore ore = other.GetComponent<Ore>();  // 获取矿石组件
            if (ore != null)
            {
                ore.TakeDamage(damage);  // 给矿石伤害
            }
        }

    }
}
