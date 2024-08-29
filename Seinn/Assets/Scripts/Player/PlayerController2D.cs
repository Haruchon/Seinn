using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

public class PlayerController2D : MonoBehaviour
{
    private const float TWEENING_TIME = 0.5f;
    public const float MIN_DISTANCE = 150f;
    public const int MAX_HEALTH = 3;

    public EnemyUI enemyUI;

    Animator animator;
    Rigidbody2D rb2d;
    SpriteRenderer sprite_renderer;

    public GameObject playerUI;
    public GameObject gameOverUI;

    public GameObject firePoint;

    public GameObject initialMap;

    Vector2 mov;

    [SerializeField]
    private float speed = 1f;

    private int health = 3;
    private int maxHealth = 3;

    private void Awake()
    {
        Assert.IsNotNull(initialMap);
    }
    
    void Start()
    {
        animator = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
        sprite_renderer = GetComponent<SpriteRenderer>();

        Camera.main.GetComponent<MainCamera>().SetBound(initialMap);
        GameEvents.current.ActivateHearts(maxHealth);
        GameEvents.current.onDisableMovement += DisableMovement;
        GameEvents.current.onEnableMovement += EnableMovement;
        AudioManager.instance.StopSound("MenuMusic");
        AudioManager.instance.PlaySound("BackgroundMusic");
    }

    private void DisableMovement()
    {
        this.enabled = false;
    }

    private void EnableMovement()
    {
        this.enabled = true;
    }
    

    private void FixedUpdate()
    {
        findClosestEnemy();

        mov = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        firePoint.transform.position = new Vector2(transform.position.x + mov.x * 1f, transform.position.y + mov.y * 1f);

        rb2d.MovePosition(rb2d.position + mov.normalized * speed * Time.deltaTime);

        if(mov != Vector2.zero)
        {
            animator.SetBool("walking", true);
            animator.SetFloat("movX", mov.x);
            animator.SetFloat("movY", mov.y);
        }
        else
        {
            animator.SetBool("walking", false);
        }

        
    }

    public void TakeDamage()
    {
        health -= 1;
        GameEvents.current.UpdateHealth(health);
        if(health == 0)
        {
            // game over
            playerUI.SetActive(false);
            LeanTween.moveLocalY(gameOverUI, 0f, TWEENING_TIME).setOnComplete(() => { Time.timeScale = 0f; });
            GameObject.FindObjectOfType<PauseMenu>().gameOver = true;
        }
    }

    private void findClosestEnemy()
    {
        float distanceToClosestEnemy = Mathf.Infinity;
        Enemy closestEnemy = null;
        Enemy[] allEnemies = GameObject.FindObjectsOfType<Enemy>();

        foreach(Enemy currentEnemy in allEnemies)
        {
            float distanceToEnemy = (currentEnemy.transform.position - this.transform.position).sqrMagnitude;
            if(distanceToEnemy < distanceToClosestEnemy)
            {
                distanceToClosestEnemy = distanceToEnemy;
                closestEnemy = currentEnemy;
            }
        }

        if(distanceToClosestEnemy <= MIN_DISTANCE)
        {
            enemyUI.changeEnemy(closestEnemy);
        }
        else
        {
            enemyUI.changeEnemy(null);
        }        

    }
}
