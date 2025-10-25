using UnityEngine;
using System.Collections.Generic;

public class EnemyAI : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float detectRange = 70f;
    public float avoidRadius = 2.5f; // 敌人之间最小间隔

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
}
