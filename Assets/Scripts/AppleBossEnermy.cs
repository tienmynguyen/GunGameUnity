
using UnityEngine;
using System.Collections;

public class AppleBossEnermy : Enemy
{
    [SerializeField] private GameObject bulletPrefabs;
    [SerializeField] private GameObject AppleBossV2;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float speedBullet = 20f;
    [SerializeField] private float speedBulletTron = 10f;
    [SerializeField] private GameObject miniEnemy;
    [SerializeField] private float CD = 1f;
    private float nextSkillTime = 0f;


    [SerializeField] private GameObject banana;

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
        Instantiate(AppleBossV2, transform.position, Quaternion.identity);
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

            for (int i = 0; i < 3; i++)
            {
                GameObject bullet = Instantiate(bulletPrefabs, firePoint.position, Quaternion.identity);
                EnemyBullet enemyBullet = bullet.AddComponent<EnemyBullet>();
                enemyBullet.setMovemenDirection(directionToPlayer * speedBullet);

                yield return new WaitForSeconds(0.1f); // khoảng cách giữa các phát bắn
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

        for (int j = 0; j < 3; j++)  // Lặp lại 3 lần
        {
            for (int i = 0; i < bulletCount; i++)
            {
                float angle = i * angleStep;
                Vector3 bulletDirection = new Vector3(Mathf.Cos(Mathf.Deg2Rad * angle), Mathf.Sin(Mathf.Deg2Rad * angle), 0);
                GameObject bullet = Instantiate(bulletPrefabs, transform.position, Quaternion.identity);
                EnemyBullet enemyBullet = bullet.AddComponent<EnemyBullet>();
                enemyBullet.setMovemenDirection(bulletDirection * speedBulletTron);
            }

            yield return new WaitForSeconds(0.1f);  // Delay giữa các vòng bắn
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
        int randomskill = Random.Range(0, 6);
        switch (randomskill)
        {
            case 0:
                BandanThuong();
                break;
            case 1:
                BandanVongTron();
                break;
            case 2:
                SinhMiniEnemy();
                break;
            case 3:
                SinhMiniEnemy();
                break;
            case 4:
                SinhMiniEnemy();
                break;


        }
    }
    private void sudungskill()
    {
        nextSkillTime = Time.time + CD;
        chonskill();
    }

}
