using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 7f;
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private Animator animator;
    private bool isGrounded;
    private SpriteRenderer spriteRenderer;
    private bool isAttacking = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    void FixedUpdate()
    {
        // 바닥에 닿았는지 확인
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }

    void Update()
    {
        // 공격 입력 처리 (지면에 있을 때만 가능)
        if (Input.GetKeyDown(KeyCode.Z) && !isAttacking && isGrounded)
        {
            isAttacking = true;
            animator.SetBool("isAttacking", true);
            rb.velocity = new Vector2(0f, rb.velocity.y); // 공격 중에는 이동을 멈춤
        }

        // 공격 중이 아니면 이동 가능
        if (!isAttacking)
        {
            // 좌우 이동 처리
            float horizontal = 0f;
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                horizontal = -1f;
                spriteRenderer.flipX = true; // 왼쪽으로 이동시 스프라이트 반전
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                horizontal = 1f;
                spriteRenderer.flipX = false;
            }

            // 이동 처리
            rb.velocity = new Vector2(horizontal * moveSpeed, rb.velocity.y);
        }

        // 점프 처리 (지면에 있을 때만 가능)
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && !isAttacking) // 공격 중에는 점프도 막음
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

        // 애니메이션 상태 처리
        animator.SetFloat("speed", Mathf.Abs(rb.velocity.x));  // 이동 속도에 따른 애니메이션 전환
        animator.SetBool("isJumping", !isGrounded);  // 점프 중이면 점프 애니메이션
    }

    // 애니메이션 이벤트에서 호출 (애니메이션 마지막에 이벤트 추가해야 함)
    public void EndAttack()
    {
        isAttacking = false;  // 공격 모션 종료 시 이동 가능
        animator.SetBool("isAttacking", false);
    }
}
