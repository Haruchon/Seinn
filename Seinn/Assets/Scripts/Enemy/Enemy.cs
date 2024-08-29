using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using TMPro;

public class Enemy : MonoBehaviour
{
    private const float RANGE_MIN_NUMBER = -100f;
    private const float RANGE_MAX_NUMBER = 100f;
    public int id;
    public int questOrder;

    [SerializeField]
    public float health;

    [SerializeField]
    public double value;

    [SerializeField]
    public int operation = 0;
    private System.Random seed;

    public GameObject numberSpawnPoints;

    public SpriteRenderer sr;
    public BoxCollider2D bc1; // hit box
    public BoxCollider2D bc2; // movement box
    public GameObject deathEffect;

    public TextMeshPro valueText;
    public TextMeshPro solutionMsg;
    private GameObject healthTextUI;

    public GameObject randomNumberPrefab;

    private Weapon weapon;

    private Vector3 startingPosition;

    public bool canTakeDmg;

    Animator animator;
    public EnemyAI enemyAI;

    public void EnableInteractions()
    {
        enemyAI.enabled = true;
        this.enabled = true;
    }

    public void DisableInteractions()
    {
        enemyAI.enabled = false;
        this.enabled = false;
    }

    public void Start()
    {
        startingPosition = transform.position;
        animator = GetComponent<Animator>();
        enemyAI = GetComponent<EnemyAI>();
        //valueText.SetText(value.ToString());
        //valueText.color = new Color32(91, 110, 255, 255);
        weapon = FindObjectOfType<Weapon>();
        Assert.IsNotNull(numberSpawnPoints);
        UpdateValue();
        RespawnNumbers();
        canTakeDmg = true;
    }

    public void DisableHealth()
    {
        canTakeDmg = false;
    }

    public void EnableHealth()
    {
        canTakeDmg = true;
    }

    public void UpdateValue()
    {
        valueText.SetText("");
        //value = Mathf.Floor(Random.Range(RANGE_MIN_NUMBER, RANGE_MAX_NUMBER));
        CustomNumber val = NumberConfig.current.GetNewNumber(questOrder);
        value = val.number.ToDouble();
        operation = CustomNumber.GetOperation(questOrder);
        switch (operation)
        {
            case 0:
                valueText.color = new Color32(91, 110, 255, 255);
                break;

            case 1:
                valueText.color = new Color32(217, 87, 99, 255);
                break;

            case 2:
                valueText.color = new Color32(106, 190, 48, 255);
                break;

            case 3:
                valueText.color = new Color32(89, 86, 82, 255);
                break;

            default:
                Debug.Log("Algo malo pasó al cambiar de número en el enemigo");
                break;
        }
        valueText.SetText(val.ToString());
    }


    public void TakeDamage(float damage)
    {
        StartCoroutine(TakeDamageRoutine(damage));
    }

    public void messageEffect(Color32 color, string msg)
    {
        StartCoroutine(messageEffectRoutine(color, msg));
    }


    private IEnumerator messageEffectRoutine(Color32 color, string msg)
    {
        solutionMsg.color = color;
        solutionMsg.SetText(msg);
        yield return new WaitForSeconds(1f);
        solutionMsg.SetText("");
    }

    private IEnumerator TakeDamageRoutine (float damage)
    {
        enemyAI.currentState = EnemyState.stagger;
        animator.SetBool("moving", false);
        health -= damage;
        if(health <= 0)
        {
            StartCoroutine(Die()) ;
        }
        else
        {
            yield return new WaitForSeconds(1f);
            enemyAI.currentState = EnemyState.idle;
        }

        UpdateValue();
        RespawnNumbers();
    }

    IEnumerator Die()
    {
        GameEvents.current.EnemyDeath(this);
        GameObject dEffect = Instantiate(deathEffect, transform.position, Quaternion.identity);
        bc1.enabled = false;
        bc2.enabled = false;
        sr.enabled = false;
        valueText.enabled = false;
        solutionMsg.enabled = false;
        yield return new WaitForSeconds(2f);
        Destroy(dEffect);
        yield return new WaitForSeconds(0.3f);
        DestroySpawnedNumbers();
        Destroy(gameObject);
    }

    public void SetSpawner(GameObject spawnPoints,int questOrder)
    {
        this.questOrder = questOrder;
        numberSpawnPoints = spawnPoints;
    }

    public void RespawnNumbers()
    {
        DestroySpawnedNumbers();

        SpawnNumber();
    }

    private void DestroySpawnedNumbers()
    {

        for (int i = 0; i < numberSpawnPoints.transform.childCount; i++)
        {
            Transform spawn = numberSpawnPoints.transform.GetChild(i);
            foreach (Transform child in spawn)
                GameObject.Destroy(child.gameObject);
        }
    }

    private void SpawnNumber()
    {
        bool isSpawnedSolution = false;
        CustomNumber numberToSpawn;
        int operationToUse = 0;
        for (int i = 0; i < numberSpawnPoints.transform.childCount; i++)
        {
            Transform spawn = numberSpawnPoints.transform.GetChild(i);
            numberToSpawn = NumberConfig.current.GetNewNumber(questOrder);
            if (!isSpawnedSolution)
            {
                operationToUse = CustomNumber.GetOperation(questOrder);
                switch (operationToUse)
                {

                    case 0:
                        numberToSpawn = new CustomNumber(operationToUse, value - weapon.bulletDmg[operationToUse], NumberConfig.current.GetNumberType(questOrder));
                        break;

                    case 1:
                        numberToSpawn = new CustomNumber(operationToUse, value + weapon.bulletDmg[operationToUse], NumberConfig.current.GetNumberType(questOrder));
                        break;

                    case 2:
                        numberToSpawn = new CustomNumber(operationToUse, value / weapon.bulletDmg[operationToUse], NumberConfig.current.GetNumberType(questOrder));
                        break;

                    case 3:
                        numberToSpawn = new CustomNumber(operationToUse, value * weapon.bulletDmg[operationToUse], NumberConfig.current.GetNumberType(questOrder));
                        break;

                    default:
                        Debug.Log("Algo malo pasó en la creacion de un numero blanco");
                        break;
                }
                isSpawnedSolution = true;
            }
            GameObject randomNumber = Instantiate(randomNumberPrefab, spawn.position, Quaternion.identity, spawn) as GameObject;
            randomNumber.GetComponent<Number>().value = numberToSpawn;
        }
    }
}
