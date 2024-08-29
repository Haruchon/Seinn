using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemyUI : MonoBehaviour
{

    private const float TWEENING_TIME = 0.3f;

    public GameObject enemyImg;

    public TextMeshProUGUI healthText;

    private Enemy closestEnemy;


    // Start is called before the first frame update
    void Start()
    {
        //
    }

    public void changeEnemy(Enemy newEnemy)
    {
        if(newEnemy != null)
        {
            //Debug.Log("changing enemy");
            closestEnemy = newEnemy;
            //change image
            enemyImg.GetComponent<Image>().sprite = closestEnemy.GetComponent<SpriteRenderer>().sprite;
            //change text
            healthText.SetText(closestEnemy.health.ToString());
            LeanTween.moveLocalX(gameObject, 350f, TWEENING_TIME);
        }
        else
        {
            //Debug.Log("null enemy");
            LeanTween.moveLocalX(gameObject, 450f, TWEENING_TIME);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(closestEnemy != null)
        {
            healthText.SetText(closestEnemy.health.ToString());
        }
    }
}
