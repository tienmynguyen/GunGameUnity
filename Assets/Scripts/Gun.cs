
using TMPro;
using UnityEngine;

public class Gun : MonoBehaviour
{
    private float rotateOffset = 180f;
    [SerializeField] private Transform firePos;
    [SerializeField] private GameObject bulletPredfabs;
    [SerializeField] private float shotDelay = 0.15f;
    private float nextShot;
    [SerializeField] private int maxAmmo = 24;
    public int currentAmmo;
    [SerializeField] private TextMeshProUGUI ammoText;
    [SerializeField] private TextMeshProUGUI SkillEText;
    [SerializeField] private AudioManager audioManager;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private GameObject explosionPrefabs;

    [SerializeField] private float explosionCooldown = 5f; // thời gian hồi
    private float lastExplosionTime = -Mathf.Infinity;

    void Start()
    {
        currentAmmo = maxAmmo;
        UpdateAmmoText();
    }


    void Update()
    {
        RotateGun();
        Shoot();
        Reload();
        float timeSinceLastSpawn = Time.time - lastExplosionTime;
        float cooldownLeft = Mathf.Max(0f, explosionCooldown - timeSinceLastSpawn);

        if (gameManager.explosionSkill == true)
        {
            if (SkillEText != null)
            {
                if (cooldownLeft > 0f)
                {
                    SkillEText.text = $"E {cooldownLeft:F1}s";
                }
                else
                {
                    SkillEText.text = "E Ready!";
                }
            }
        }
        else
        {
            SkillEText.text = "";

        }

        if (Input.GetKeyDown(KeyCode.E) && Time.time >= lastExplosionTime + explosionCooldown)
        {

            if (gameManager.explosionSkill == true)
            {
                UsingExplosion();
                lastExplosionTime = Time.time;
            }
        }

    }
    void RotateGun()
    {
        if (Input.mousePosition.x < 0 || Input.mousePosition.x > Screen.width || Input.mousePosition.y < 0 || Input.mousePosition.y > Screen.height)
        {
            return;
        }
        Vector3 displacement = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float angle = Mathf.Atan2(displacement.y, displacement.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle + rotateOffset);
        if (angle < -90 || angle > 90)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            transform.localScale = new Vector3(1, -1, 1);
        }

    }

    void Shoot()
    {
        if (Input.GetMouseButtonDown(0) && currentAmmo > 0 && Time.time > nextShot)
        {
            nextShot = Time.time + shotDelay;
            Instantiate(bulletPredfabs, firePos.position, firePos.rotation);
            currentAmmo--;
            UpdateAmmoText();
            audioManager.PlayShootSound();
        }
    }
    void Reload()
    {
        if (Input.GetMouseButtonDown(1) && currentAmmo < maxAmmo)
        {
            currentAmmo = maxAmmo;
            UpdateAmmoText();
            audioManager.PlayReLoadSound();
        }
    }
    private void UpdateAmmoText()
    {
        if (ammoText != null)
        {
            if (currentAmmo > 0)
            {
                ammoText.text = currentAmmo.ToString();
            }
            else
            {
                ammoText.text = "EMPTY";
            }
        }
    }

    private void UsingExplosion()
    {

        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0f;
        GameObject spawnedObj = Instantiate(explosionPrefabs, mouseWorldPos, Quaternion.identity);
        Destroy(spawnedObj, 1f); // huỷ sau 1s
    }

}
