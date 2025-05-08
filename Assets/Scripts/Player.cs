using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    [SerializeField] private float maxHp = 100f;
    private float currentHp;
    [SerializeField] private Image hpBar;
    [SerializeField] private GameManager gameManager;

    [SerializeField] private float dashSpeed = 15f;  // tốc độ dash
    [SerializeField] private float dashDuration = 0.2f; // thời gian dash
    [SerializeField] private float dashCooldown = 3f; // thời gian hồi

    [SerializeField] private bool isDashing = false;
    private float dashTimeLeft;
    private float lastDashTime = -Mathf.Infinity;

    private Vector2 lastMoveDir;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    void Start()
    {


        currentHp = maxHp;
        UpdateHpBar();
    }


    void Update()
    {
        if (!isDashing)
        {
            MovePlayer();
            if (gameManager.dash == true)
            {
                if (Input.GetKeyDown(KeyCode.Space) && Time.time >= lastDashTime + dashCooldown)
                {
                    StartDash();
                }
            }
        }
        else
        {
            Dash();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            gameManager.PauseGameMenu();
        }
    }
    void MovePlayer()
    {
        Vector2 playerInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        rb.velocity = playerInput.normalized * moveSpeed;

        if (playerInput != Vector2.zero)
        {
            lastMoveDir = playerInput.normalized; // lưu hướng cuối cùng để dash

            animator.SetBool("isRun", true);
            if (playerInput.x < 0) spriteRenderer.flipX = true;
            else if (playerInput.x > 0) spriteRenderer.flipX = false;
        }
        else
        {
            animator.SetBool("isRun", false);
        }
    }

    public void TakeDamge(float damage)
    {
        currentHp -= damage;
        currentHp = Mathf.Max(currentHp, 0);
        UpdateHpBar();
        if (currentHp <= 0)
        {
            Die();
        }
    }
    private void Die()
    {
        gameManager.GameOverMenu();
    }
    private void UpdateHpBar()
    {
        if (hpBar != null)
        {
            hpBar.fillAmount = currentHp / maxHp;
        }
    }
    public void Heal(float healValue)
    {
        if (currentHp < maxHp)
        {
            currentHp += healValue;
            currentHp = Mathf.Min(currentHp, maxHp);
            UpdateHpBar();
        }
    }

    private void StartDash()
    {
        isDashing = true;
        dashTimeLeft = dashDuration;
        lastDashTime = Time.time;
        rb.velocity = lastMoveDir * dashSpeed;
        animator.SetTrigger("dash"); // nếu có animation dash
    }

    private void Dash()
    {
        dashTimeLeft -= Time.deltaTime;
        if (dashTimeLeft <= 0)
        {
            isDashing = false;
            rb.velocity = Vector2.zero;
        }
    }
    
}
