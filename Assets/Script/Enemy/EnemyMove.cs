using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float moveTimeMin = 1f;
    public float moveTimeMax = 3f;
    public float idleTimeMin = 1f;
    public float idleTimeMax = 2f;
    public float jumpForce = 6f;

    public float jumpCooldownMin = 3f; // 최소 점프 쿨타임
    public float jumpCooldownMax = 10f; // 최대 점프 쿨타임
    private float jumpTimer = 0f;

    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private float moveDirection = 1f;
    private float moveTimer = 0f;
    private bool isMoving = false;
    private bool isGrounded = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        SetNextAction();
        jumpTimer = Random.Range(jumpCooldownMin, jumpCooldownMax); // 초기 점프 쿨타임
    }

    void Update()
    {
        // 바닥 체크
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // 이동 처리
        if (isMoving)
        {
            rb.velocity = new Vector2(moveDirection * moveSpeed, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(0f, rb.velocity.y);
        }

        // 좌우 반전
        if (moveDirection < 0)
            spriteRenderer.flipX = true;
        else if (moveDirection > 0)
            spriteRenderer.flipX = false;

        // 애니메이션 파라미터
        animator.SetFloat("speed", Mathf.Abs(rb.velocity.x));
        animator.SetBool("isJumping", !isGrounded);

        // 이동 타이머 갱신
        moveTimer -= Time.deltaTime;
        if (moveTimer <= 0f)
        {
            SetNextAction();
        }

        // 점프 타이머 갱신
        jumpTimer -= Time.deltaTime;
        if (jumpTimer <= 0f && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpTimer = Random.Range(jumpCooldownMin, jumpCooldownMax); // 다음 점프까지 랜덤 쿨타임
        }
    }

    void SetNextAction()
    {
        if (Random.value < 0.5f)
        {
            isMoving = true;
            moveDirection = Random.value < 0.5f ? -1f : 1f;
            moveTimer = Random.Range(moveTimeMin, moveTimeMax);
        }
        else
        {
            isMoving = false;
            moveTimer = Random.Range(idleTimeMin, idleTimeMax);
        }
    }
}
