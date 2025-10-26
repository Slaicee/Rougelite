using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerate : MonoBehaviour
{
    [Header("敌人生成设置")]
    public GameObject[] enemyPrefabs;    // 敌人预制体数组
    public int initialEnemyCount = 10;   // 初始波敌人数量
    public int enemyCountIncrease = 2;   // 每波增加的敌人数量
    public float mapSize = 70f;          // 地图范围（Plane大小一半）
    public float minSpawnDistance = 3f;  // 生成间距，防止重叠
    public float waveInterval = 20f;     // 每波生成的时间间隔（秒）

    [Header("玩家引用")]
    public Transform player;

    private List<Vector3> usedPositions = new List<Vector3>();
    private int currentWave = 0;          // 当前波数
    private float nextWaveTime;           // 下一波生成时间

    void Start()
    {
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player").transform;

        nextWaveTime = Time.time + waveInterval;  // 设置第一波的生成时间
        SpawnWave();  // 生成第一波敌人
    }
    void Update()
    {
        // 检查是否到达生成下一波敌人的时间
        if (Time.time >= nextWaveTime)
        {
            currentWave++;  // 增加波数
            nextWaveTime = Time.time + waveInterval;  // 更新下一波生成的时间
            SpawnWave();    // 生成新的敌人波
        }
    }

    void SpawnWave()
    {
        // 计算当前波数的敌人数量
        int enemyCount = initialEnemyCount + currentWave * enemyCountIncrease;

        int spawned = 0;
        int safetyLimit = 200; // 防止死循环
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

        Vector3 pos = new Vector3(x, 2f, z); 
        return pos;
    }
}
