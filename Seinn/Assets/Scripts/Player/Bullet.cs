using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Bullet : MonoBehaviour
{

    public const string MSG_CORRECT = "Es la solucion!!";
    public const string MSG_INCORRECT = "Solución incorrecta :(";
    public const string COLOR_INCORRECT = "Operación incorrecta :(";
    public const string CANT_TAKE_DMG = "No puedes dañar al enemigo aún!!";

    private const int CORRECT_EXP = 10;
    private const int INCORRECT_EXP = -5;

    public float speed = 20f;
    public double damage = 0f;
    public Rigidbody2D rb;
    private Vector2 direction = new Vector2(0,0);
    public SpriteRenderer sr;
    public CircleCollider2D cc;
    public GameObject impactEffect;

    public int operation; //0 = Sum, 1 = Dif, 2 = Mult, 3 = Div


    public void ChangeDirectionAndDamage(object[] args)
    {
        Vector2 dir = (Vector2)args[0];
        double dmg = (double)args[1];
        direction = dir;
        damage = dmg;
    }


    // Start is called before the first frame update
    void Start()
    {
        rb.velocity = direction * speed;
    }

    
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            if (enemy != null)
            {
                if (enemy.canTakeDmg)
                {
                    if (enemy.operation == operation)
                    {
                        if (Utils.CheckEquality(enemy.value, damage) && enemy.enemyAI.currentState != EnemyState.stagger)
                        {
                            enemy.TakeDamage(1);
                            enemy.messageEffect(new Color32(31, 204, 42, 254), MSG_CORRECT);
                            GameEvents.current.UpdateScore(CORRECT_EXP);
                        }
                        else if (!Utils.CheckEquality(enemy.value, damage) && enemy.enemyAI.currentState != EnemyState.stagger)
                        {
                            enemy.messageEffect(new Color32(255, 14, 31, 254), MSG_INCORRECT);
                            enemy.RespawnNumbers();
                            GameEvents.current.UpdateScore(INCORRECT_EXP);
                        }
                    }
                    else
                    {
                        enemy.messageEffect(new Color32(255, 14, 31, 254), COLOR_INCORRECT);
                        enemy.RespawnNumbers();
                        GameEvents.current.UpdateScore(INCORRECT_EXP);
                    }
                }
                else
                {
                    enemy.messageEffect(new Color32(255, 14, 31, 254), CANT_TAKE_DMG);
                    enemy.RespawnNumbers();
                }               
            }
            StartCoroutine(collisionEffect());
        }
        else if (collision.CompareTag("Number"))
        {
            //Debug.Log("Pasa");
            Number number = collision.GetComponent<Number>();
            // move this into the Number tag condition
            double newDamage = 0f;
            switch (operation)
            {
                case 0:
                    newDamage = number.value.number.ToDouble() + damage;
                    break;

                case 1:
                    newDamage = number.value.number.ToDouble() - damage;
                    break;

                case 2:
                    newDamage = number.value.number.ToDouble() * damage;
                    break;

                case 3:
                    newDamage = number.value.number.ToDouble() / damage;
                    break;

                default:
                    Debug.Log("Algo malo pasó en la colision de una bala");
                    break;
            }

            object[] tempStorage = new object[2];
            tempStorage[0] = operation;
            tempStorage[1] = newDamage;
            GameObject.FindGameObjectWithTag("Player").gameObject.GetComponent<Weapon>().UpdateSingleNumber(tempStorage);
            GameObject.FindGameObjectWithTag("Player").gameObject.GetComponent<Weapon>().UpdateCanvas();

            number.DestroyNumber();
            StartCoroutine(collisionEffect());
        }
        else if (!collision.CompareTag("Projectile") && !collision.CompareTag("Player"))
        {
            StartCoroutine(collisionEffect());
        }
    }

    IEnumerator collisionEffect()
    {
        AudioManager.instance.PlaySound("HitSound");
        cc.enabled = false;
        rb.velocity = transform.right * 0;
        sr.enabled = false;           
        GameObject iEffect = Instantiate(impactEffect, transform.position, transform.rotation);
        yield return new WaitForSeconds(0.3f);
        Destroy(iEffect);
        Destroy(gameObject);
    }
}
