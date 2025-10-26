using UnityEngine;
using System.Collections.Generic;

public class EnemyAI : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float detectRange = 70f;
    public float avoidRadius = 2.5f; // 敌人之间最小间隔
    public float health = 10f;       // 血量
    public float damage = 5f;        // 每次攻击造成的伤害
    public GameObject oreDropPrefab; // 矿石掉落预制体

    public Transform player;
    private Rigidbody rb;
    private static List<EnemyAI> allEnemies = new List<EnemyAI>();

    void OnEnable() => allEnemies.Add(this);
    void OnDisable() => allEnemies.Remove(this);

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (player == null) return;

        Vector3 moveDir = Vector3.zero;
        float distance = Vector3.Distance(transform.position, player.position);
        if (distance <= detectRange)
        {
            moveDir = (player.position - transform.position).normalized;
        }

        // 推开附近的敌人
        foreach (var other in allEnemies)
        {
            if (other == this) continue;
            float dist = Vector3.Distance(transform.position, other.transform.position);
            if (dist < avoidRadius)
            {
                Vector3 pushDir = (transform.position - other.transform.position).normalized;
                moveDir += pushDir * (avoidRadius - dist); // 越近推得越远
            }
        }

        if (moveDir != Vector3.zero)
        {
            transform.position += moveDir.normalized * moveSpeed * Time.deltaTime;
        }
    }

    // 受到攻击时扣血
    public void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0f)
        {
            Die();
        }
    }

    void Die()
    {
        // 触发矿石掉落
        DropOre();

        // 销毁敌人
        Destroy(gameObject);  // 删除敌人
    }

    // 矿石掉落方法
    void DropOre()
    {
        if (oreDropPrefab != null)
        {
            int dropCount = Random.Range(1, 4); // 随机掉1~3个矿石
            for (int i = 0; i < dropCount; i++)
            {
                // 在敌人周围1.5f半径范围内随机掉落矿石
                Vector3 randomOffset = new Vector3(
                    Random.Range(-1.5f, 1.5f),
                    -1.5f, // 固定y为0.25，确保掉落物不浮空
                    Random.Range(-1.5f, 1.5f)
                );

                Instantiate(oreDropPrefab, this.transform.position + randomOffset, Quaternion.identity);
            }
        }
    }

    // 用于敌人被子弹碰撞到时触发
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            Bullet bullet = other.GetComponent<Bullet>();
            if (bullet != null)
            {
                TakeDamage(bullet.damage);  // 从子弹获得伤害值
                Destroy(other.gameObject);  // 子弹销毁
            }
        }
    }
}
