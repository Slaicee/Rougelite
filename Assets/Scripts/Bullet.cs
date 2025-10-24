using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float lifeTime = 3f;
    public float damage = 1f;

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") || other.CompareTag("Ore"))
        {
            // ������Ե��õ��˻��ʯ�������߼�
            Debug.Log($"{other.name} �����У�");
            Destroy(gameObject);
        }
    }
}
