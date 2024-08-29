using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{

    public float speed = 5f;
    public float damage = 1f;
    public Rigidbody2D rb;
    private Vector2 direction = new Vector2(0, 0);
    public SpriteRenderer sr;
    public CircleCollider2D cc;
    public GameObject impactEffect;



    public void ChangeDirection(Vector2 dir)
    {
        direction = dir;
    }


    // Start is called before the first frame update
    void Start()
    {
        rb.velocity = direction.normalized * speed;
    }


    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerController2D player= collision.GetComponent<PlayerController2D>();
            if (player != null)
            {
                player.TakeDamage();
            }

            StartCoroutine(collisionEffect());
        }
        else if (collision.CompareTag("Enemy"))
        {
            return;
        }
        else if (!collision.CompareTag("Player") && !collision.CompareTag("Enemy") && !collision.CompareTag("Projectile"))
        {
            StartCoroutine(collisionEffect());
        }
    }

    IEnumerator collisionEffect()
    {
        cc.enabled = false;
        rb.velocity = transform.right * 0;
        sr.enabled = false;
        GameObject iEffect = Instantiate(impactEffect, transform.position, transform.rotation);
        yield return new WaitForSeconds(0.3f);
        Destroy(iEffect);
        //yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }
}
