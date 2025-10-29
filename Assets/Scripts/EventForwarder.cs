using UnityEngine;

public class EventForwarder : MonoBehaviour
{
    private EnemyAI parentEnemyAI; // �������EnemyAI

    void Start()
    {
        // ���Ҹ����壨�����ȣ��ϵ�EnemyAI���
        parentEnemyAI = GetComponentInParent<EnemyAI>();
        if (parentEnemyAI == null)
        {
            Debug.LogError("�������Ҳ����������EnemyAI�����", this);
        }
    }

    // �����¼�ֱ�ӵ�������������������ϵķ�����
    public void DealDamage()
    {
        // ת�����������EnemyAI
        parentEnemyAI?.DealDamage();
    }
}