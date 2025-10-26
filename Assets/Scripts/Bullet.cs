using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float lifeTime = 3f;
    public float damage = 1f;

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    // �ӵ�����ײ���崥��ʱ
    void OnTriggerEnter(Collider other)
    {
        // ����ӵ��Ƿ��������ײ
        if (other.CompareTag("Enemy"))
        {
            EnemyAI enemy = other.GetComponent<EnemyAI>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage); // �˺�����
            }
        }

        // ����ӵ��Ƿ����ʯ��ײ
        if (other.CompareTag("Ore"))
        {
            Ore ore = other.GetComponent<Ore>();  // ��ȡ��ʯ���
            if (ore != null)
            {
                ore.TakeDamage(damage);  // ����ʯ�˺�
            }
        }

    }
}
