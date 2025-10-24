using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    public GameObject bulletPrefab;      // 拖入子弹预制体
    public Transform firePoint;          // 拖入发射点
    public float bulletSpeed = 25f;
    public float fireRate = 0.2f;        // 射击间隔
    public LayerMask groundMask;         // 地面层
    private float nextFireTime = 0f;

    void Update()
    {
        // 自动射击
        if (Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + fireRate;
            ShootTowardMouse();
        }
    }

    void ShootTowardMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 100f, groundMask))
        {
            Vector3 dir = (hit.point - firePoint.position).normalized;
            dir.y = 0; // 保持水平射击（俯视视角）

            // 生成子弹
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = dir * bulletSpeed;
            }

            // 调整子弹朝向（可选）
            bullet.transform.forward = dir;
        }
    }
}
