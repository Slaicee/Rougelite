using UnityEngine;

public class Ore : MonoBehaviour
{
    public float health = 3f;// 被攻击3次后破碎
    public GameObject oreDropPrefab;// 掉落物

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0f)
        {
            DropOre();
            Destroy(gameObject);
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
                    0.5f,
                    Random.Range(-1.5f, 1.5f)
                );

                Instantiate(oreDropPrefab, this.transform.position + randomOffset, Quaternion.identity);
            }
        }
    }

}
