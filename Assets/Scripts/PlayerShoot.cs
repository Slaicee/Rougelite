using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    public GameObject bulletPrefab;      // �����ӵ�Ԥ����
    public Transform firePoint;          // ���뷢���
    public float bulletSpeed = 50f;
    public float fireRate = 0.2f;        // ������
    public LayerMask groundMask;         // �����
    private float nextFireTime = 0f;
    public Transform model;              // ������

    void Update()
    {
        RotateTowardMouse();             // ģ����ת
        // �Զ����
        if (Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + fireRate;
            ShootTowardMouse();
        }
    }

    void RotateTowardMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 100f, groundMask))
        {
            Vector3 lookPos = hit.point - transform.position;
            lookPos.y = 0f; // ����ˮƽ��ת
            if (lookPos.sqrMagnitude > 0.001f)
            {
                Quaternion targetRot = Quaternion.LookRotation(lookPos);
                model.rotation = Quaternion.Slerp(model.rotation, targetRot, Time.deltaTime * 10f);
            }
        }
    }

    void ShootTowardMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 100f, groundMask))
        {
            Vector3 dir = (hit.point - firePoint.position).normalized;
            dir.y = 0; // ����ˮƽ����������ӽǣ�

            // �����ӵ�
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = dir * bulletSpeed;
            }

            // �����ӵ����򣨿�ѡ��
            bullet.transform.forward = dir;
        }
    }
}
