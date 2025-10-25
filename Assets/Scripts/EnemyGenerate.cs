using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerate : MonoBehaviour
{
    [Header("������������")]
    public GameObject[] enemyPrefabs;    // ����Ԥ��������
    public int enemyCount = 10;          // ����������
    public float mapSize = 70f;          // ��ͼ��Χ��Plane��Сһ�룩
    public float minSpawnDistance = 3f;  // ���ɼ�࣬��ֹ�ص�

    [Header("�������")]
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
        int safetyLimit = 500; // ��ֹ��ѭ��
        int attempts = 0;

        while (spawned < enemyCount && attempts < safetyLimit)
        {
            attempts++;
            Vector3 pos = GetRandomEdgePosition();

            // ��������ɵ��Ƿ�̫������������
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

            // ��������ô���EnemyAI
            EnemyAI ai = enemy.GetComponent<EnemyAI>();
            if (ai != null) ai.player = player;

            usedPositions.Add(pos);
            spawned++;
        }

        Debug.Log($" �ɹ����� {spawned} ������ (���� {attempts} ��)");
    }

    Vector3 GetRandomEdgePosition()
    {
        float x = 0f, z = 0f;
        int edge = Random.Range(0, 4); // ���������ı�

        switch (edge)
        {
            case 0: // �ϱ�
                x = Random.Range(-mapSize, mapSize);
                z = mapSize;
                break;
            case 1: // �±�
                x = Random.Range(-mapSize, mapSize);
                z = -mapSize;
                break;
            case 2: // ���
                x = -mapSize;
                z = Random.Range(-mapSize, mapSize);
                break;
            case 3: // �ұ�
                x = mapSize;
                z = Random.Range(-mapSize, mapSize);
                break;
        }

        // Raycast ������߶�
        Vector3 pos = new Vector3(x, 20f, z);
        if (Physics.Raycast(pos, Vector3.down, out RaycastHit hit, 100f))
            pos.y = hit.point.y + 0.5f;
        else
            pos.y = 1f;

        return pos;
    }
}
