using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState
{
    idle,
    attack,
    stagger
}

public class EnemyAI : MonoBehaviour{

    public EnemyState currentState;

    Rigidbody2D rb2d;
    Animator animator;

    [SerializeField]
    private float speed = 4f;
    private Vector3 startingPosition;
    private Vector3 roamPosition;
    public Transform firePoint;
    public GameObject enemyAttack;

    public bool facingLeft;

    public static Vector3 GetRandomDir()
    {
        return new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f)).normalized;
    }

    private void Awake()
    {
        facingLeft = false;
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    private void Start()
    {
        StartCoroutine(EnemyAttack());
        currentState = EnemyState.idle;
        startingPosition = transform.position;
        roamPosition = GetRoamPosition();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        //Move To roam position
        if(currentState != EnemyState.attack && currentState != EnemyState.stagger && currentState == EnemyState.idle)
        {
            MoveToRoamPosition(roamPosition);
        }
        
        float reachedPositionDistance = 0.5f;
        if (Vector3.Distance(transform.position, roamPosition) < reachedPositionDistance)
        {
            roamPosition = GetRoamPosition();
        }
        
        
    }

    public void MoveToRoamPosition(Vector3 roamPosition)
    {
        facingLeft = roamPosition.x - transform.position.x < 0 ? true : false;
        animator.SetBool("moving", true);
        animator.SetBool("facingLeft", facingLeft);
        animator.SetFloat("movX", facingLeft?-1:1);

        transform.position = Vector3.MoveTowards(transform.position, roamPosition, speed * Time.deltaTime);
    }

    public Vector3 GetRoamPosition()
    {
        Vector3 randomDir = GetRandomDir();

        randomDir.x = randomDir.x = Random.Range(-4f, 4f);
        randomDir.y = randomDir.y = Random.Range(-2f, 2f);

        return startingPosition + randomDir;
    }
    
    private IEnumerator EnemyAttack()
    {
        while (true)
        {
            
            //Debug.Log("waiting 5 seconds");
            yield return new WaitForSeconds(5f);
            if(currentState != EnemyState.attack && currentState != EnemyState.stagger)
            {
                //Debug.Log("5 second passed");
                animator.SetBool("moving", false);
                animator.SetBool("attacking", true);
                currentState = EnemyState.attack;
                //Debug.Log("starting attack");
                yield return new WaitForSeconds(0.4f);
                GenerateBullet(new Vector2(0, -1));
                GenerateBullet(new Vector2(1, -2));
                GenerateBullet(new Vector2(-1, -2));
                yield return new WaitForSeconds(0.4f);
                //Debug.Log("Finishing attack");
                animator.SetBool("attacking", false);
                animator.SetBool("moving", true);
                currentState = EnemyState.idle;
            }
            
        }
    }

    private void GenerateBullet(Vector2 dir)
    {
        GameObject bullet1 = Instantiate(enemyAttack, firePoint.position, Quaternion.identity) as GameObject;
        bullet1.SendMessage("ChangeDirection", dir);
    }
}
