using UnityEngine;

[AddComponentMenu("Utils/ModelLookAt")]
public class EnemyModel : MonoBehaviour
{
    [Tooltip("Ҫ�����Ŀ��")]
    public Transform target;

    [Tooltip("ƽ����ת�ٶȣ�Խ��Խ�죩")]
    public float rotationSpeed = 8f;

    [Tooltip("�ֲ�ŷ����ƫ�ƣ����ģ�ͱ�����ǰ������ +Z���������������")]
    public Vector3 eulerOffset = Vector3.zero;

    [Tooltip("�Ƿ�ʹ�ñ�����ת��true������������ת��false��")]
    public bool useLocalRotation = false;

    private EnemyAI enemyAI;

    void Start()
    {
        // ���û�ֶ�ָ��Ŀ�꣬�����Զ����Ҹ������ϵ�EnemyAI.player
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

        // ����ˮƽ��XZ ƽ�棩���򣬺��� Y ���ֵ
        Vector3 dir = target.position - transform.position;
        dir.y = 0f;
        if (dir.sqrMagnitude < 0.0001f) return;

        Quaternion want = Quaternion.LookRotation(dir.normalized);

        // Ӧ��ƫ�ƣ���������ģ�ͳ����᲻�� +Z �������
        want *= Quaternion.Euler(eulerOffset);

        if (useLocalRotation)
            transform.localRotation = Quaternion.Slerp(transform.localRotation, want, rotationSpeed * Time.deltaTime);
        else
            transform.rotation = Quaternion.Slerp(transform.rotation, want, rotationSpeed * Time.deltaTime);
    }
}
