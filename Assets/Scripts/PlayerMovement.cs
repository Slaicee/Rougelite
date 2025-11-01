using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 20f;
    public float inputThreshold = 0.1f; // ������ֵ���ؼ�����������΢С���룩
    private Rigidbody rb;
    private Vector3 moveDir;
    public Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // ��ȡԭʼ���루δƽ����-1~1ֵ��
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        // ����ԭʼ����������δ��һ����
        Vector3 rawInput = new Vector3(x, 0f, z);

        // ���ģ��ж��������������Ƿ�С����ֵ��С������Ϊ�������롱
        if (rawInput.magnitude < inputThreshold)
        {
            moveDir = Vector3.zero; // ������ʱ���ƶ�����Ϊ�㣨ֹͣ��
            animator.SetBool("isRunning", false);
        }
        else
        {
            moveDir = rawInput.normalized; // �����㹻ʱ����һ����������
            animator.SetBool("isRunning", true);
        }
    }

    private void FixedUpdate()
    {
        // ֻ�е�moveDir��Ϊ��ʱ���ƶ�������΢С�ƶ���
        if (moveDir != Vector3.zero)
        {
            float moveDistance = moveSpeed * Time.fixedDeltaTime;
            if (!Physics.Raycast(rb.position, moveDir, moveDistance + 0.05f))
            {
                rb.MovePosition(rb.position + moveDir * moveDistance);
            }
        }
    }
}