
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private AudioManager audioManager;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("EnemyBullet"))
        {
            Debug.Log("dmg -10");
            Player player = GetComponent<Player>();
            player.TakeDamge(10f);
        }
        if (collision.CompareTag("Banana"))
        {
            gameManager.AddBanana();
            gameManager.OpenChest();

            Destroy(collision.gameObject);
        }
        if (collision.CompareTag("Energy"))
        {
            gameManager.AddEnergy();
            Destroy(collision.gameObject);
            audioManager.PlayEnergySound();
        }
        if (collision.CompareTag("Coin"))
        {
            Player player = GetComponent<Player>();
            gameManager.AddCoin();
            Destroy(collision.gameObject);
            audioManager.PlayEnergySound();
        }
    }
}
