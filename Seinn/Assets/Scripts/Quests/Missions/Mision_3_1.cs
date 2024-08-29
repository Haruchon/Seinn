using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mision_3_1 : Quest
{
    private const string extraDialogue = "Será el número correcto? Veamos...";
    private const string correctDialogue = "Es correcto! A por el siguiente";
    private const string incorrectDialogue = "Vaya, es incorrecto...";
    private const string Npc = "Estatua Seinn";
    private int numObjects;
    private List<List<DialogueElement>> dialogues = new List<List<DialogueElement>>();
    private List<CustomNumber> numbers;

    public GameObject tombstone;
    public GameObject tombstoneCollider;

    public GameObject spawnPoints;
    public GameObject numberPrefab;
    public GameObject statueSpawnPoint;
    public GameObject statueObject;

    public override void InitializeParameters()
    {
        numbers = new List<CustomNumber>();
        string randomEquation = "";
        for (int j = 0; j < numObjects; j++)
        {
            numbers.Add(NumberConfig.current.GetNewNumber(questOrder));
            randomEquation = Utils.RandomEquation(numbers[j].number.ToDouble(), NumberConfig.current.GetEquationType(questOrder), questOrder);

            List<DialogueElement> primaryDialoge = new List<DialogueElement>();
            string numberToShow = "Halla el valor de x:\n" + randomEquation + "\nDebes tener la respuesta en el color " + numbers[j].color;
            string rememberNumber = "Recuerda que debes hallar el valor de x en:\n" + randomEquation + "\nY tener la respuesta en el color " + numbers[j].color;
            primaryDialoge.Add(new DialogueElement(Npc, new string[] { numberToShow })); //dialogo normal
            primaryDialoge.Add(new DialogueElement(Npc, new string[] { extraDialogue })); //dialogo extra 
            primaryDialoge.Add(new DialogueElement(Npc, new string[] { extraDialogue + "\n" + correctDialogue })); //dialogo correcto
            primaryDialoge.Add(new DialogueElement(Npc, new string[] { extraDialogue, incorrectDialogue, rememberNumber })); //dialogo incorrecto
            dialogues.Add(primaryDialoge);

        }
    }

    void Start()
    {
        id = 10009;
        questOrder = 9;
        questTitle = "Esto ya casi acaba";
        questDescription = "Para continuar a la siguiente y última misión debes responder las preguntas de las estatuas en este cuarto";
        completed = false;
        acceptDialogue = new DialogueElement("", new string[] { "Vamos allá!" });
        declineDialogue = new DialogueElement("", new string[] { "..." });
        numObjects = statueSpawnPoint.transform.childCount;
        InitializeParameters();

        goals.Add(new CollectionGoal(statueObject.GetComponent<CollectableObject>().id, 15, "Responder las preguntas de las 4 estatuas", false, 0, numObjects));
        goals.ForEach(g => g.Init());

        GameEvents.current.onQuestCompleted += QuestCompleted;

    }

    public void QuestCompleted(Quest quest)
    {
        if (quest.id == this.id)
        {
            Destroy(tombstone);
            Destroy(tombstoneCollider);
        }
    }

    public override void InitializeObjects()
    {
        base.InitializeObjects();
        int i = 0;
        foreach (Transform child in statueSpawnPoint.transform)
        {
            GameObject statues = Instantiate(statueObject, child.transform.position, Quaternion.identity, child.transform) as GameObject;
            statues.GetComponent<CollectableObject>().SetSpawner(spawnPoints, numberPrefab);
            statues.GetComponent<CollectableObject>().InitializeData(numbers[i].operation, numbers[i].number.ToDouble(), true, dialogues[i], questOrder);
            statues.GetComponent<CollectableObject>().EnableInteractions();
            i++;
        }
    }

}
