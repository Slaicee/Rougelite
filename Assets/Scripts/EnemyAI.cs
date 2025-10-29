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
    public float attackRange = 5f;       // 敌人的攻击范围
    public float attackCooldown = 1f;    // 攻击冷却时间
    public float lastAttackTime = 0f;    // 上次攻击时间

    public Transform player;
    private Rigidbody rb;
    private UIManager uiManager;
    private static List<EnemyAI> allEnemies = new List<EnemyAI>();
    private Animator animator;

    void OnEnable() => allEnemies.Add(this);
    void OnDisable() => allEnemies.Remove(this);

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        uiManager = FindObjectOfType<UIManager>();
        animator = GetComponentInChildren<Animator>();
        if (animator == null) Debug.LogError("敌人缺少Animator组件！");
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
            animator.SetBool("IsRunning", true); // 跑步
        }
        else
        {
            animator.SetBool("IsRunning", false); // 待机
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
        animator?.SetTrigger("TakeDamage"); // 触发受击动画
        if (health <= 0f)
        {
            Die();
        }
    }

    void Die()
    {
        // 触发矿石掉落
        DropOre();
        animator?.SetTrigger("Die"); // 触发死亡动画
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
        // 仅当玩家在攻击范围内时，才触发攻击动画
        float currentDistance = Vector3.Distance(transform.position, player.position);
        if (currentDistance <= attackRange)
        {
            animator?.SetTrigger("Attack");
            lastAttackTime = Time.time; // 刷新冷却时间
        }
    }

    // 动画事件调用
    public void DealDamage()
    {
        // 1. 先检查玩家是否存在（防止玩家已死亡/销毁）
        if (player == null || uiManager == null) return;

        // 2. 计算当前敌人与玩家的实时距离
        float currentDistance = Vector3.Distance(transform.position, player.position);

        // 3. 只有当实时距离 <= 攻击范围时，才扣血
        if (currentDistance <= attackRange)
        {
            uiManager.TakeDamage(damage);
            Debug.Log($"动画触发攻击，玩家在范围内，造成{damage}点伤害！");
        }
        else
        {
            Debug.Log($"动画触发攻击，但玩家已离开范围，未造成伤害");
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
