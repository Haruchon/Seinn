using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerInfo : MonoBehaviour
{
    public static PlayerInfo current;
    
    //Health Information
    public Image[] healthIcons;
    public Sprite fullHeart;
    public Sprite emptyHeart;

    //Score Information
    private static int EXP_TO_LVL_UP = 200;
    public  int level;
    public int currentExp;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI currentExpText;
    public Slider expSlider;

    private void Awake()
    {
        current = this;
    }

    private void Start()
    {
        level = 1;
        currentExp = 0;
        UpdateScore(0);

        //Health Information Events
        GameEvents.current.onActivateHearts += ActivateHearts;
        GameEvents.current.onUpdateHealth += UpdateHealth;

        //Score Information Events
        GameEvents.current.onUpdateScore += UpdateScore;
    }

    //Health Information Methods
    private void UpdateHealth(int currentHealth)
    {
        int i = 1;
        foreach(Image healthSlot in healthIcons)
        {
            if (healthSlot.enabled)
            {
                if (i <= currentHealth)
                {
                    healthSlot.sprite = fullHeart;
                }
                else
                {
                    healthSlot.sprite = emptyHeart;
                }
            }
            i++;
        }
    }

    private void ActivateHearts(int maxHealth)
    {
        int i = 1;
        foreach(Image healthSlot in healthIcons)
        {
            if (i <= maxHealth)
            {
                healthSlot.enabled = true;
            }
            i++;
        }
    }


    //Score Information Events
    private void UpdateScore(int obtainedScore)
    {
        if(currentExp + obtainedScore > EXP_TO_LVL_UP)
        {
            int dif = (currentExp + obtainedScore);
            currentExp = dif % EXP_TO_LVL_UP;
            level += dif / EXP_TO_LVL_UP;
        }
        else
        {
            currentExp += obtainedScore;
        }
        levelText.text = "Nv. " + level.ToString();
        currentExpText.text = currentExp.ToString();
        expSlider.value = currentExp;
    } 
    
}
