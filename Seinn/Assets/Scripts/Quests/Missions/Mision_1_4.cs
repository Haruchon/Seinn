using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mision_1_4 : Quest
{
    private const string Npc = "Instrucciones";
    private const string extraDialogue = "Conseguire el numero correcto? Veamos...";
    private const string correctDialogue = "Es correcto! Habran mas intrucciones?";
    private const string incorrectDialogue = "Vaya, me he equivocado... Lo volveré a intentar";
    private int numObjects = 4;
    private int iBox;
    private int iBarrel;
    private List<List<DialogueElement>> dialogues = new List<List<DialogueElement>>();
    private List<CustomNumber> numbers;

    public GameObject numberSpawnPoints;
    public GameObject numberPrefab;

    public GameObject boxSpawnPoint;
    public GameObject boxObject;

    public GameObject barrelSpawnPoint;
    public GameObject barrelObject;

    public GameObject statue;
    
    public override void InitializeParameters()
    {
        numbers = new List<CustomNumber>();

        for(int j = 0; j < numObjects; j++)
        {
            numbers.Add(NumberConfig.current.GetNewNumber(questOrder));

            List<DialogueElement> primaryDialoge = new List<DialogueElement>();
            string numberToShow = "Dividir este objeto en " + numbers[j].ToString() + " partes y usar el color " + numbers[j].color ;
            string rememberNumber = "Recuerda que debes dividir este objeto en " + numbers[j].ToString() + " partes y usar el color " + numbers[j].color;
            primaryDialoge.Add(new DialogueElement(Npc, new string[] { numberToShow })); //dialogo normal
            primaryDialoge.Add(new DialogueElement(Npc, new string[] { extraDialogue })); //dialogo extra 
            primaryDialoge.Add(new DialogueElement(Npc, new string[] { extraDialogue + "\n" + correctDialogue })); //dialogo correcto
            primaryDialoge.Add(new DialogueElement(Npc, new string[] { extraDialogue, incorrectDialogue, rememberNumber })); //dialogo incorrecto
            dialogues.Add(primaryDialoge);
        }
    }

    void Start()
    {
        //InitializeParameters();
        iBox = 0;
        iBarrel = 1;
        id = 10004;
        questOrder = 4;
        questTitle = "Ayudando a tus vecinos";
        questDescription = "Ayuda al señor Hidelburg a organizar su mercancia";
        completed = false;
        acceptDialogue = new DialogueElement("Señor Hidelburg", new string[] { "Interactua con la caja y el barril para ver las instrucciones" });
        declineDialogue = new DialogueElement("Señor Hidelburg", new string[] { "No hay problema... \nVuelve cuando tengas tiempo!" });
        InitializeParameters();

        goals.Add(new CollectionGoal(boxObject.GetComponent<CollectableObject>().id, 5, "Sigue las instrucciones que muestra la caja", false, 0, 2));
        goals.Add(new CollectionGoal(barrelObject.GetComponent<CollectableObject>().id, 6, "Sigue las instrucciones que muestra el barril", false, 0, 2));
        goals.ForEach(g => g.Init());

        GameEvents.current.onCollectedOrDestroyedObject += ReinstantiateBoxOrBarrel;
        GameEvents.current.onQuestCompleted += QuestCompleted;

    }

    public void QuestCompleted(Quest quest)
    {
        if (quest.id == this.id)
        {
            Destroy(statue);
        }
    }

    public void ReinstantiateBoxOrBarrel(CollectableObject obj)
    {
        if (obj.id == boxObject.GetComponent<CollectableObject>().id)
        {
            if (!this.goals[0].completed)
            {
                InitializeBox();
            }
        }else if(obj.id == barrelObject.GetComponent<CollectableObject>().id)
        {
            if (!this.goals[1].completed)
            {
                InitializeBarrel(); 
            }
        }
    }

    private void InitializeBox()
    {
        foreach (Transform child in boxSpawnPoint.transform)
            GameObject.Destroy(child.gameObject);
        GameObject estatua = Instantiate(boxObject, boxSpawnPoint.transform.position, Quaternion.identity, boxSpawnPoint.transform) as GameObject;
        estatua.GetComponent<CollectableObject>().SetSpawner(numberSpawnPoints, numberPrefab);
        estatua.GetComponent<CollectableObject>().InitializeData(numbers[iBox].operation, numbers[iBox].number.ToDouble(), true, dialogues[iBox], questOrder);
        estatua.GetComponent<CollectableObject>().EnableInteractions();
        iBox += 2;
    }

    private void InitializeBarrel()
    {
        foreach (Transform child in barrelSpawnPoint.transform)
            GameObject.Destroy(child.gameObject);
        GameObject estatua = Instantiate(barrelObject, barrelSpawnPoint.transform.position, Quaternion.identity, barrelSpawnPoint.transform) as GameObject;
        estatua.GetComponent<CollectableObject>().SetSpawner(numberSpawnPoints, numberPrefab);
        estatua.GetComponent<CollectableObject>().InitializeData(numbers[iBarrel].operation, numbers[iBarrel].number.ToDouble(), true, dialogues[iBarrel], questOrder);
        estatua.GetComponent<CollectableObject>().EnableInteractions();
        iBarrel += 2;
    }

    public override void InitializeObjects()
    {
        base.InitializeObjects();

        InitializeBarrel();
        InitializeBox();
    }

}
