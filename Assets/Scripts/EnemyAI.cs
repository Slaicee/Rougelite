using UnityEngine;
using System.Collections.Generic;

public class EnemyAI : MonoBehaviour
{
    public float moveSpeed = 5f;         // 敌人的移动速度
    public float detectRange = 70f;      // 敌人追踪玩家的范围
    public float avoidRadius = 2.5f;     // 敌人之间的最小间距
    public float health = 10f;           // 敌人的血量
    public float damage = 2f;            // 敌人的攻击伤害
    public GameObject oreDropPrefab;     // 矿石掉落预制体
    public float attackRange = 3f;       // 敌人的攻击范围
    public float attackCooldown = 1f;    // 攻击冷却时间
    private float lastAttackTime = 0f;   // 上次攻击时间

    public Transform player;
    private Rigidbody rb;
    private UIManager uiManager;
    private static List<EnemyAI> allEnemies = new List<EnemyAI>();

    void OnEnable() => allEnemies.Add(this);
    void OnDisable() => allEnemies.Remove(this);

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        uiManager = FindObjectOfType<UIManager>();
    }

    void Update()
    {
        if (player == null) return;

        // 追踪玩家
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

        // 移动
        if (distance >= attackRange - 0.1f)
        {
            if (moveDir != Vector3.zero)
            {
                transform.position += moveDir.normalized * moveSpeed * Time.deltaTime;
            }
        }

        // 攻击玩家逻辑
        if (distance <= attackRange && Time.time >= lastAttackTime + attackCooldown)
        {
            AttackPlayer();
            lastAttackTime = Time.time;
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

    // 近战攻击玩家
    void AttackPlayer()
    {
        if (uiManager != null)
        {
            uiManager.TakeDamage(damage);
            Debug.Log($"敌人攻击玩家，造成{damage}点伤害！");
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
