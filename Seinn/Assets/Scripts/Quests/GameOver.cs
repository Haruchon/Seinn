using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameOver : MonoBehaviour
{
    private const int LAST_LEVEL_ID = 10010;
    private int currentLevel;
    private int currentExp;
    private string playerData;
    
    private TextMeshProUGUI playerDataText;

    private void Awake()
    {
        playerDataText = transform.GetChild(2).GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        GameEvents.current.onQuestCompleted += ShowPlayerData;
        playerData = "";
    }

    public void ShowPlayerData(Quest quest)
    {
        if(quest.id == LAST_LEVEL_ID)
        {
            currentLevel = PlayerInfo.current.level;
            currentExp = PlayerInfo.current.currentExp;
            playerData = "Felicitaciones! Has completado el juego!!!\nNivel: "
                        + currentLevel.ToString() + " \nPuntuacion: " + currentExp.ToString() 
                        + "\nEspero que te hayas divertido.\nGracias por jugar este juego :D";
            playerDataText.text = "";
            StopAllCoroutines();
            ShowGameOverWindow();
            StartCoroutine(TypeSentence(playerData));
        }
    }

    private void ShowGameOverWindow()
    {
        LeanTween.moveLocalY(this.gameObject, 0f, 1f);
    }

    private IEnumerator TypeSentence(string sentence)
    {
        playerDataText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            playerDataText.text += letter;
            yield return null;
        }
    }
}
