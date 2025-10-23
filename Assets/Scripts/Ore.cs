using UnityEngine;

public class Ore : MonoBehaviour
{
    public float health = 3f;// ������3�κ�����
    public GameObject oreDropPrefab;// ������

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
            int dropCount = Random.Range(1, 4); // �����1~3����ʯ
            for (int i = 0; i < dropCount; i++)
            {
                // �ڿ�ʯ��Χ1.5f�뾶��Χ���������
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
