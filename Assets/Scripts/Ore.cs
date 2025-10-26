using UnityEngine;

public class Ore : MonoBehaviour
{
    public float health = 9f;// ������3�κ�����
    public GameObject oreDropPrefab;// ������

    private bool isDestroyed = false;
    private void OnTriggerEnter(Collider other)
    {
        if (isDestroyed) return; // ����Ѿ����٣����ٴ���
        // ��������ӵ�������TakeDamage����
        if (other.CompareTag("Bullet"))
        {
            // ��ȡ�ӵ����˺�ֵ�������ӵ���һ��Damage����
            Bullet bullet = other.GetComponent<Bullet>();
            if (bullet != null)
            {
                TakeDamage(bullet.damage); // �����ӵ��˺�
            }

            // �����ӵ�
            Destroy(other.gameObject);
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0f)
        {
            isDestroyed = true;  // ���������ٱ�־
            DropOre(); // �����ʯ
            Destroy(gameObject); // ���ٿ�ʯ
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
                    -1.5f,
                    Random.Range(-1.5f, 1.5f)
                );

                Instantiate(oreDropPrefab, this.transform.position + randomOffset, Quaternion.identity);
            }
        }
    }

}
