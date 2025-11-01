using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 20f;
    public float inputThreshold = 0.1f; // 输入阈值（关键参数，过滤微小输入）
    private Rigidbody rb;
    private Vector3 moveDir;
    public Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // 获取原始输入（未平滑的-1~1值）
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        // 构建原始输入向量（未归一化）
        Vector3 rawInput = new Vector3(x, 0f, z);

        // 核心：判断输入向量长度是否小于阈值，小于则视为“无输入”
        if (rawInput.magnitude < inputThreshold)
        {
            moveDir = Vector3.zero; // 无输入时，移动方向为零（停止）
            animator.SetBool("isRunning", false);
        }
        else
        {
            moveDir = rawInput.normalized; // 输入足够时，归一化保持匀速
            animator.SetBool("isRunning", true);
        }
    }

    private void FixedUpdate()
    {
        // 只有当moveDir不为零时才移动（避免微小移动）
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