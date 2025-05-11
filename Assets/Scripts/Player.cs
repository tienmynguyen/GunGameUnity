using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    [SerializeField] private float maxHp = 100f;
    [SerializeField] private float currentHp;
    [SerializeField] private Image hpBar;
    [SerializeField] private GameManager gameManager;

    [SerializeField] private float dashSpeed = 15f;  // tốc độ dash
    [SerializeField] private float dashDuration = 0.2f; // thời gian dash
    [SerializeField] private float dashCooldown = 3f; // thời gian hồi

    [SerializeField] private bool isDashing = false;
    private float dashTimeLeft;
    private float lastDashTime = -Mathf.Infinity;

    private Vector2 lastMoveDir;


    [SerializeField] private GameObject skillBallPrefab;
    [SerializeField] private Transform firePoint; // Vị trí trước mặt Player
    [SerializeField] private float maxScale = 3f;
    [SerializeField] private float growSpeed = 1f;

    private GameObject chargingBall;
    private float chargeStartTime;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        InvokeRepeating("RegenerateHealth", 1f, 1f);

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


        if (gameManager.energyBallSkill == true)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                StartChargingSkill();
            }

            if (Input.GetKey(KeyCode.LeftShift))
            {
                ChargeSkill();
            }

            if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                ReleaseSkill();
            }
        }

    }
    private void RegenerateHealth()
    {
        Heal(1f); // Hồi 1 HP mỗi lần gọi
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
        // animator.SetTrigger("dash"); // nếu có animation dash
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



    void StartChargingSkill()
    {
        chargingBall = Instantiate(skillBallPrefab, firePoint.position, Quaternion.identity);
        chargingBall.transform.localScale = Vector3.one * 0.1f;
        chargeStartTime = Time.time;
    }

    void ChargeSkill()
    {
        if (chargingBall == null) return;

        float scale = Mathf.Min(maxScale, chargingBall.transform.localScale.x + growSpeed * Time.deltaTime);
        chargingBall.transform.localScale = Vector3.one * scale;

        // Quả cầu luôn nằm trước mặt Player
        chargingBall.transform.position = firePoint.position;
    }

    void ReleaseSkill()
    {
        if (chargingBall == null) return;

        float chargeTime = Time.time - chargeStartTime;
        float damage = chargingBall.transform.localScale.x * 10f;

        // Tính hướng tới chuột
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        Vector3 dir = (mousePos - chargingBall.transform.position).normalized;

        // Thêm Rigidbody2D để bay
        Rigidbody2D rb = chargingBall.GetComponent<Rigidbody2D>();
        rb.velocity = dir * 10f;

        //Gán damage
        chargingBall.GetComponent<EnergyBallSkill>().SetDamage(damage);
        Destroy(chargingBall, 5f);
        chargingBall = null;
    }

}
