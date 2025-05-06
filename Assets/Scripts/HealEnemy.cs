using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealEnemy : Enemy
{
    [SerializeField] private float heal = 20f;
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
    protected override void Die()
    {
        if (player != null)
        {
            player.Heal(heal);
        }
        base.Die();
    }
}
