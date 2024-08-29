using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Helper : MonoBehaviour
{
    private int nShowing;
    private int nObtained;
    private int nInPossesion;
    private List<string> helpTitles;
    private List<string> helpDescriptions;

    private TextMeshProUGUI helpTitleText;
    private TextMeshProUGUI helpDescriptionText;
    private GameObject previousButton;
    private GameObject nextButton;

    private void Awake()
    {
        helpTitles = new List<string>();
        helpDescriptions = new List<string>();
        nShowing = 0;
        nObtained = 0;
        nInPossesion = 0;
        helpTitleText = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        helpDescriptionText = transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        previousButton = transform.GetChild(3).gameObject;
        nextButton = transform.GetChild(4).gameObject;
    }

    private void Start()
    {
        GameEvents.current.onTriggeredDialogue += UpdateHelpingStatements;
    }

    private void CheckButtons()
    {
        if(nShowing == 0)
        {
            previousButton.SetActive(false);
        }
        else if(nShowing == nInPossesion - 1)
        {
            nextButton.SetActive(false);
        }
        else
        {
            previousButton.SetActive(true);
            nextButton.SetActive(true);
        }
    }

    public void ShowNextHelp()
    {
        nShowing = Mathf.Abs((++nShowing) % nInPossesion);
        CheckButtons();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(helpTitleText, helpTitles[nShowing]));
        StartCoroutine(TypeSentence(helpDescriptionText, helpDescriptions[nShowing]));
    }

    public void ShowPreviousHelp()
    {
        nShowing = nShowing == 0 ? (nInPossesion - 1) : Mathf.Abs((--nShowing) % nInPossesion);
        CheckButtons();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(helpTitleText, helpTitles[nShowing]));
        StartCoroutine(TypeSentence(helpDescriptionText, helpDescriptions[nShowing]));
    }

    private void UpdateHelpingStatements(int id)
    {
        if (id == 100 && nObtained == 0)
        {
            //Interactuar y moverte --- 100
            helpTitles.Add("Ayuda #1");
            helpDescriptions.Add("Presiona E para interactuar con algunos objetos o personas.\nPuedes usar las flechas para moverte libremente.");
            nInPossesion += 1;
            nObtained++;
            ShowNextHelp();
        }
        else if (id == 101 && nObtained == 1)
        {
            //seguir flecha de mision --- 101
            helpTitles.Add("Ayuda #2");
            helpDescriptions.Add("Cuando aceptas una mision, una flecha aparecerá en la parte superior de la pantalla.\nSi la flecha desaparece significa que estas cerca!");
            helpTitles.Add("Ayuda #3");
            helpDescriptions.Add("Si olvidas cual es la mision que debes completar puedes revisar la misión activa entrando al menú.\nPresiona ESC para abrir el menú y da clic en el boton \"Ver mision actual\" para ver los objetivos de tu mision actual.");
            nInPossesion += 2;
            nObtained++;
        }
        else if (id == 102 && nObtained == 2)
        {
            //dañar a los enemigos y colores --- 102
            helpTitles.Add("Ayuda #4");
            helpDescriptions.Add("Para dañar al enemigo debes igualar el número que tiene encima suyo con el número que tienes en tus proyectiles.\nEl color del número del enemigo indica qué número debes tener en el proyectil, tambien del mismo color, para dañarlo!\nPara disparar un proyectil presiona Q. Inténtalo!");
            helpTitles.Add("Ayuda #5");
            helpDescriptions.Add("Cuando dispares a un número que veas en el suelo, será como operar dos números!\nEl número que tengas en tu proyectil se operará con el número en el suelo dependiendo del color que uses. Recuerda: Azul es Sumar, Rojo es Restar, Verde es Multiplicar y Gris es Dividir.");
            nInPossesion += 2;
            nObtained++;
        }
        else if (id == 103 && nObtained == 3)
        {
            //cambiar de numeros y proyectiles --- 103
            helpTitles.Add("Ayuda #6");
            helpDescriptions.Add("Puedes presionar las teclas R o F para cambiar el proyectil que estas usando. El proyectil activo es aquel que esta girando!");
            helpTitles.Add("Ayuda #7");
            helpDescriptions.Add("Tambien puedes presionar las teclas W o S para cambiar los números que tienen los proyectiles.\nLa tecla W mueve los números hacia la derecha.\nLa tecla S mueve los números hacia la izquierda!");
            nInPossesion += 2;
            nObtained++;

        }
        else if (id == 104 && nObtained == 4)
        {
            //los puntos caen si te equivocas --- 104
            helpTitles.Add("Ayuda #8");
            helpDescriptions.Add("Piensa bien las respuestas que vas a dar o los proyectiles que usarás.\nPuedes perder puntos si te equivocas!!\nPero no bajarás de nivel :)");
            nInPossesion += 1;
            nObtained++;
        }
        else
            return;
    }

    private IEnumerator TypeSentence(TextMeshProUGUI textArea, string sentence)
    {
        textArea.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            textArea.text += letter;
            yield return null;
        }
    }
}
