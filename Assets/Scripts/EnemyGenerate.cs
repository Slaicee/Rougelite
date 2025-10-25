using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerate : MonoBehaviour
{
    [Header("敌人生成设置")]
    public GameObject[] enemyPrefabs;    // 敌人预制体数组
    public int enemyCount = 10;          // 总生成数量
    public float mapSize = 70f;          // 地图范围（Plane大小一半）
    public float minSpawnDistance = 3f;  // 生成间距，防止重叠

    [Header("玩家引用")]
    public Transform player;

    private List<Vector3> usedPositions = new List<Vector3>();

    void Start()
    {
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player").transform;

        SpawnEnemies();
    }

    void SpawnEnemies()
    {
        int spawned = 0;
        int safetyLimit = 500; // 防止死循环
        int attempts = 0;

        while (spawned < enemyCount && attempts < safetyLimit)
        {
            attempts++;
            Vector3 pos = GetRandomEdgePosition();

            // 检查新生成点是否太靠近其他敌人
            bool tooClose = false;
            foreach (var usedPos in usedPositions)
            {
                if (Vector3.Distance(usedPos, pos) < minSpawnDistance)
                {
                    tooClose = true;
                    break;
                }
            }

            if (tooClose) continue;

            GameObject prefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
            GameObject enemy = Instantiate(prefab, pos, Quaternion.identity);

            // 把玩家引用传给EnemyAI
            EnemyAI ai = enemy.GetComponent<EnemyAI>();
            if (ai != null) ai.player = player;

            usedPositions.Add(pos);
            spawned++;
        }

        Debug.Log($" 成功生成 {spawned} 个敌人 (尝试 {attempts} 次)");
    }

    Vector3 GetRandomEdgePosition()
    {
        float x = 0f, z = 0f;
        int edge = Random.Range(0, 4); // 上下左右四边

        switch (edge)
        {
            case 0: // 上边
                x = Random.Range(-mapSize, mapSize);
                z = mapSize;
                break;
            case 1: // 下边
                x = Random.Range(-mapSize, mapSize);
                z = -mapSize;
                break;
            case 2: // 左边
                x = -mapSize;
                z = Random.Range(-mapSize, mapSize);
                break;
            case 3: // 右边
                x = mapSize;
                z = Random.Range(-mapSize, mapSize);
                break;
        }

        // Raycast 检查地面高度
        Vector3 pos = new Vector3(x, 20f, z);
        if (Physics.Raycast(pos, Vector3.down, out RaycastHit hit, 100f))
            pos.y = hit.point.y + 0.5f;
        else
            pos.y = 1f;

        return pos;
    }
}
