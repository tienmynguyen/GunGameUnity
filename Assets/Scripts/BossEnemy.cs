
using UnityEngine;

public class BossEnemy : Enemy
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private GameObject bulletPrefabs;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float speedBullet = 20f;
    [SerializeField] private float speedBulletTron = 10f;
    [SerializeField] private float hpValue = 100f;
    [SerializeField] private GameObject miniEnemy;
    [SerializeField] private float CD = 2f;
    private float nextSkillTime = 0f;


    [SerializeField] private GameObject banana;
    protected override void Start()
    {
        if (gameManager == null)
        {
            gameManager = FindObjectOfType<GameManager>();
        }
        base.Start();
    }
    protected override void Update()
    {
        base.Update();
        if (Time.time >= nextSkillTime)
        {
            sudungskill();
        }
    }
    protected override void Die()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            Destroy(enemy);
        }
        Instantiate(banana, transform.position, Quaternion.identity);
        base.Die();
        gameManager.BossDefeat();


    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (player != null)
            {
                player.TakeDamge(enterDamage);
            }
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (player != null)
            {
                player.TakeDamge(stayDamage);
            }
        }
    }

    private void BandanThuong()
    {
        if (player != null)
        {
            Vector3 directionToPlayer = player.transform.position - firePoint.position;
            directionToPlayer.Normalize();
            GameObject bullet = Instantiate(bulletPrefabs, firePoint.position, Quaternion.identity);
            EnemyBullet enemyBullet = bullet.AddComponent<EnemyBullet>();
            enemyBullet.setMovemenDirection(directionToPlayer * speedBullet);
        }
    }
    private void BandanVongTron()
    {
        const int bulletCount = 12;
        float angleStep = 360f / bulletCount;
        for (int i = 0; i < bulletCount; i++)
        {
            float angle = i * angleStep;
            Vector3 bulletDirection = new Vector3(Mathf.Cos(Mathf.Deg2Rad * angle), Mathf.Sin(Mathf.Deg2Rad * angle), 0);
            GameObject bullet = Instantiate(bulletPrefabs, transform.position, Quaternion.identity);
            EnemyBullet enemyBullet = bullet.AddComponent<EnemyBullet>();
            enemyBullet.setMovemenDirection(bulletDirection * speedBulletTron);
        }
    }

    private void HoiMau(float hpAmount)
    {
        currentHp = Mathf.Min(currentHp + hpAmount, maxHp);
        UpdateHpBar();

    }
    private void SinhMiniEnemy()
    {
        Instantiate(miniEnemy, transform.position, Quaternion.identity);
    }
    private void DichChuyen()
    {
        if (player != null)
        {
            transform.position = player.transform.position;
        }
    }
    private void chonskill()
    {
        int randomskill = Random.Range(0, 5);
        switch (randomskill)
        {
            case 0:
                BandanThuong();
                break;
            case 1:
                BandanVongTron();
                break;
            case 2:
                HoiMau(hpValue);
                break;
            case 3:
                SinhMiniEnemy();
                break;
            case 4:
                DichChuyen();
                break;
        }
    }
    private void sudungskill()
    {
        nextSkillTime = Time.time + CD;
        chonskill();
    }

}
