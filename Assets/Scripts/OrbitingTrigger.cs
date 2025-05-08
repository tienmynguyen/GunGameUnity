using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitingTrigger : MonoBehaviour
{
    public bool isGreen = false; // Gán true nếu là hình tròn xanh

    [SerializeField] public float damage = 25f;

    public Enemy enemy;    // Gán từ AppleBossV2 khi tạo
    public Player player;  // Gán thủ công trong Inspector hoặc từ GameObject.Find

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerBullet"))
        {
            if (isGreen)
            {
                // Gây sát thương cho người chơi
                if (player != null)
                {
                    player.TakeDamge(damage);
                    Debug.Log("Player bị trúng bởi hình tròn xanh!");
                }
            }
            else
            {
                // Gây sát thương cho enemy
                if (enemy != null)
                {
                    enemy.TakeDamge(damage);
                    Debug.Log("Enemy bị trúng bởi hình tròn đỏ!");
                }
            }

            Destroy(collision.gameObject); // Xoá đạn
        }
    }
}
