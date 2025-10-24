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
            // 这里可以调用敌人或矿石的受伤逻辑
            Debug.Log($"{other.name} 被击中！");
            Destroy(gameObject);
        }
    }
}
