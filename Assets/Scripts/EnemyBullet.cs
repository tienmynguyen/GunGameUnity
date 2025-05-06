
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    private Vector3 movemenDirection;

    void Start()
    {
        Destroy(gameObject, 5f);
    }


    void Update()
    {
        if (movemenDirection == Vector3.zero) return;
        transform.position += movemenDirection * Time.deltaTime;
    }
    public void setMovemenDirection(Vector3 direction)
    {
        movemenDirection = direction;
    }
}
