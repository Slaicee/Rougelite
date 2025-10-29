using UnityEngine;

[AddComponentMenu("Utils/ModelLookAt")]
public class EnemyModel : MonoBehaviour
{
    [Tooltip("要朝向的目标")]
    public Transform target;

    [Tooltip("平滑旋转速度（越大越快）")]
    public float rotationSpeed = 8f;

    [Tooltip("局部欧拉角偏移（如果模型本身正前方向不是 +Z，用这个做修正）")]
    public Vector3 eulerOffset = Vector3.zero;

    [Tooltip("是否使用本地旋转（true）还是世界旋转（false）")]
    public bool useLocalRotation = false;

    private EnemyAI enemyAI;

    void Start()
    {
        // 如果没手动指向目标，尝试自动查找父物体上的EnemyAI.player
        if (target == null && transform.parent != null)
        {
            enemyAI = transform.parent.GetComponent<EnemyAI>();
            if (enemyAI != null)
                target = enemyAI.player;
        }
    }

    void LateUpdate()
    {
        if (target == null) return;

        // 计算水平（XZ 平面）方向，忽略 Y 轴差值
        Vector3 dir = target.position - transform.position;
        dir.y = 0f;
        if (dir.sqrMagnitude < 0.0001f) return;

        Quaternion want = Quaternion.LookRotation(dir.normalized);

        // 应用偏移（用于修正模型朝向轴不是 +Z 的情况）
        want *= Quaternion.Euler(eulerOffset);

        if (useLocalRotation)
            transform.localRotation = Quaternion.Slerp(transform.localRotation, want, rotationSpeed * Time.deltaTime);
        else
            transform.rotation = Quaternion.Slerp(transform.rotation, want, rotationSpeed * Time.deltaTime);
    }
}
