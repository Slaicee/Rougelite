using UnityEngine;

public class Ore : MonoBehaviour
{
    public float health = 9f;// 被攻击3次后破碎
    public GameObject oreDropPrefab;// 掉落物

    private bool isDestroyed = false;
    private void OnTriggerEnter(Collider other)
    {
        if (isDestroyed) return; // 如果已经销毁，不再处理
        // 如果碰到子弹，调用TakeDamage方法
        if (other.CompareTag("Bullet"))
        {
            // 获取子弹的伤害值，假设子弹有一个Damage属性
            Bullet bullet = other.GetComponent<Bullet>();
            if (bullet != null)
            {
                TakeDamage(bullet.damage); // 传递子弹伤害
            }

            // 销毁子弹
            Destroy(other.gameObject);
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0f)
        {
            isDestroyed = true;  // 设置已销毁标志
            DropOre(); // 掉落矿石
            Destroy(gameObject); // 销毁矿石
        }
    }

    void DropOre()
    {
        if (oreDropPrefab != null)
        {
            int dropCount = Random.Range(1, 4); // 随机掉1~3个矿石
            for (int i = 0; i < dropCount; i++)
            {
                // 在矿石周围1.5f半径范围内随机掉落
                Vector3 randomOffset = new Vector3(
                    Random.Range(-1.5f, 1.5f),
                    -1.5f,
                    Random.Range(-1.5f, 1.5f)
                );

                Instantiate(oreDropPrefab, this.transform.position + randomOffset, Quaternion.identity);
            }
        }
    }

}
