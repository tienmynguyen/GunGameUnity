
using UnityEngine;

public class EnergyBallSkill : MonoBehaviour
{
    // Start is called before the first frame update

    private float damage;

    public void SetDamage(float dmg)
    {
        damage = dmg;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            collision.GetComponent<Enemy>().TakeDamge(damage);

        }
    }
}
