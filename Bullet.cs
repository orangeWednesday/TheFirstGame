using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour
{
    [SerializeField, Header("銃弾のスピード")]
    private float speed;

    [SerializeField, Header("復活時間")]
    public float respawnTime;

    private Vector3 startPosition;
    private Rigidbody2D rb;

    [SerializeField, Header("攻撃力")]
    private int _enemyAttackPower;

    void Start()
    {
        startPosition = transform.position;
        rb = GetComponent<Rigidbody2D>();

        // 最初から左に飛ばす
        MoveLeft();
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            rb.linearVelocity = Vector2.zero; // 動きを止める
            gameObject.SetActive(false);
            Invoke(nameof(Respawn), respawnTime);
        }

        else if (collision.gameObject.tag == "Player")
        {
            rb.linearVelocity = Vector2.zero; // 動きを止める
            gameObject.SetActive(false);
            Invoke(nameof(Respawn), respawnTime);
        }
    }

    public void Respawn()
    {
        transform.position = startPosition;
        rb.linearVelocity = Vector2.zero;
        gameObject.SetActive(true);

        MoveLeft(); // 復活してすぐ左に飛ばす
    }

    public void MoveLeft()
    {
        rb.linearVelocity = Vector2.left * speed;
    }

    public void PlayerDamage(Player player)
    {
        player.Damage(_enemyAttackPower);
    }
}

