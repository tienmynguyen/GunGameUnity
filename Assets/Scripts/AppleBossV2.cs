
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class AppleBossV2 : Enemy
{
    [SerializeField] private GameObject bulletPrefabs;
    [SerializeField] private GameObject greenCirclePrefab;
    [SerializeField] private GameObject redCirclePrefab;
    [SerializeField] private GameManager gameManager;
    [SerializeField] public int totalCircles = 6;
    [SerializeField] public float radius = 2f;
    [SerializeField] public float rotateSpeed = 50f;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float speedBullet = 30f;
    [SerializeField] private float speedBulletTron = 30f;
    [SerializeField] private float hpValue = 100f;
    [SerializeField] private GameObject miniEnemy;
    [SerializeField] private float CD = 2f;
    private float nextSkillTime = 0f;
    private List<GameObject> orbitingCircles = new List<GameObject>();

    [SerializeField] private GameObject banana;

    protected override void Start()
    {
        base.Start();
        CreateAlternatingCircles();
        if (gameManager == null)
        {
            gameManager = FindObjectOfType<GameManager>();
        }
    }
    protected override void Update()
    {
        base.Update();
        RotateCircles();
        if (Time.time >= nextSkillTime)
        {
            sudungskill();
        }
    }
    protected override void Die()
    {
        gameManager.Wingame();
        base.Die();
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
        StartCoroutine(Ban3Vien());
    }

    private IEnumerator Ban3Vien()
    {
        if (player != null)
        {
            Vector3 directionToPlayer = player.transform.position - firePoint.position;
            directionToPlayer.Normalize();

            for (int i = 0; i < 5; i++)
            {
                GameObject bullet = Instantiate(bulletPrefabs, firePoint.position, Quaternion.identity);
                EnemyBullet enemyBullet = bullet.AddComponent<EnemyBullet>();
                enemyBullet.setMovemenDirection(directionToPlayer * speedBullet);

                yield return new WaitForSeconds(0.5f); // khoảng cách giữa các phát bắn
            }
        }
    }
    private void BandanVongTron()
    {
        StartCoroutine(BanVongTronLienTuc());
    }

    private IEnumerator BanVongTronLienTuc()
    {
        const int bulletCount = 12;
        float angleStep = 360f / bulletCount;

        for (int j = 0; j < 5; j++)  // Lặp lại 3 lần
        {
            for (int i = 0; i < bulletCount; i++)
            {
                float angle = i * angleStep;
                Vector3 bulletDirection = new Vector3(Mathf.Cos(Mathf.Deg2Rad * angle), Mathf.Sin(Mathf.Deg2Rad * angle), 0);
                GameObject bullet = Instantiate(bulletPrefabs, transform.position, Quaternion.identity);
                EnemyBullet enemyBullet = bullet.AddComponent<EnemyBullet>();
                enemyBullet.setMovemenDirection(bulletDirection * speedBulletTron);
            }

            yield return new WaitForSeconds(0.5f);  // Delay giữa các vòng bắn
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

    private void chonskill()
    {
        int randomskill = Random.Range(0, 3);
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


        }
    }
    private void sudungskill()
    {
        nextSkillTime = Time.time + CD;
        chonskill();
    }
    private void CreateAlternatingCircles()
    {
        for (int i = 0; i < totalCircles; i++)
        {
            float angle = i * Mathf.PI * 2f / totalCircles;
            Vector3 offset = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * radius;

            bool isGreen = (i % 2 == 0);
            GameObject prefabToUse = isGreen ? greenCirclePrefab : redCirclePrefab;
            GameObject circle = Instantiate(prefabToUse, transform.position + offset, Quaternion.identity);

            OrbitingTrigger triggerScript = circle.GetComponent<OrbitingTrigger>();
            if (triggerScript != null)
            {
                triggerScript.enemy = this;
                triggerScript.isGreen = isGreen;
                triggerScript.player = GameObject.FindWithTag("Player").GetComponent<Player>();
            }

            circle.transform.parent = transform;
            orbitingCircles.Add(circle);
        }
    }

    private void RotateCircles()
    {
        foreach (GameObject circle in orbitingCircles)
        {
            circle.transform.RotateAround(transform.position, Vector3.forward, rotateSpeed * Time.deltaTime);
        }
    }

}
