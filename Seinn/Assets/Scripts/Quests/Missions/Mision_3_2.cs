using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mision_3_2 : Quest
{
    private float nextActionTime = 0.0f;
    private float period = 60f;
    private const string Npc = "Estatua misteriosa";
    private const string extraDialogue = "Conseguire el numero correcto? Veamos...";
    private const string correctDialogue = "Es correcto! Habran mas intrucciones?";
    private const string incorrectDialogue = "Vaya, me he equivocado... Lo volveré a intentar";
    private int numObjects = 2;
    private List<List<DialogueElement>> dialogues = new List<List<DialogueElement>>();
    private List<CustomNumber> numbers;
    private int i = 0;

    public GameObject statueNumberSpawnPoints;
    public GameObject enemyNumberSpawnPoints;
    public GameObject numberPrefab;

    public GameObject statueSpawnPoint;
    public GameObject statueObject;

    public GameObject enemySpawnPoint;
    public GameObject enemyObject;

    private GameObject enemy;
    private bool questGiven;

    public override void InitializeParameters()
    {
        numbers = new List<CustomNumber>();
        string randomEquation;

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

    private void Start()
    {
        //InitializeParameters();
        id = 10010;
        questOrder = 10;
        questTitle = "La última misión";
        questDescription = "Desactivar las estatuas para dañar al enemigo y derrotarlo!";
        completed = false;
        acceptDialogue = new DialogueElement("", new string[] { "Estoy listo..." });
        declineDialogue = new DialogueElement("", new string[] { "..." });
        InitializeParameters();

        goals.Add(new CollectionGoal(statueObject.GetComponent<CollectableObject>().id, 17, "Desactiva las 2 estatuas para ", false, 0, 2));
        goals.Add(new KillGoal(enemyObject.GetComponent<Enemy>().id, 16, "Derrota al enemigo", false, 0, 1));
        goals.ForEach(g => g.Init());

        GameEvents.current.onGoalCompletion += OnFirstGoalCompletion;
    }

    private void Update() {
        if (Time.time > nextActionTime)
        {
            if (questGiven)
            {
                nextActionTime = Time.time + period;
                InstantiateStatue(true);
                enemy.GetComponent<Enemy>().DisableHealth();
                goals[0].completed = false;
            }
        }
    }

    public void OnFirstGoalCompletion(Goal goal)
    {
        if (goal.goalID == 17)
        {
            enemy.GetComponent<Enemy>().EnableHealth();
        }
    }

    public override void InitializeObjects()
    {
        base.InitializeObjects();
        InstantiateStatue(false);
        Destroy(enemySpawnPoint.transform.GetChild(0).gameObject);
        enemy = Instantiate(enemyObject, enemySpawnPoint.transform.position, Quaternion.identity, enemySpawnPoint.transform) as GameObject;
        enemy.GetComponent<Enemy>().SetSpawner(enemyNumberSpawnPoints, questOrder);
        questGiven = true;
    }

    private void InstantiateStatue(bool reInstantiate)
    {
        if (reInstantiate)
        {
            numbers[i] = NumberConfig.current.GetNewNumber(questOrder);
            string randomEquation = Utils.RandomEquation(numbers[i].number.ToDouble(), NumberConfig.current.GetEquationType(questOrder), questOrder);

            List<DialogueElement> primaryDialoge = new List<DialogueElement>();
            string numberToShow = "Halla el valor de x:\n" + randomEquation + "\nDebes tener la respuesta en el color " + numbers[i].color;
            string rememberNumber = "Recuerda que debes hallar el valor de x en:\n" + randomEquation + "\nY tener la respuesta en el color " + numbers[i].color;
            primaryDialoge.Add(new DialogueElement(Npc, new string[] { numberToShow })); //dialogo normal
            primaryDialoge.Add(new DialogueElement(Npc, new string[] { extraDialogue })); //dialogo extra 
            primaryDialoge.Add(new DialogueElement(Npc, new string[] { extraDialogue + "\n" + correctDialogue })); //dialogo correcto
            primaryDialoge.Add(new DialogueElement(Npc, new string[] { extraDialogue, incorrectDialogue, rememberNumber })); //dialogo incorrecto
            dialogues[i] = primaryDialoge;
        }

        foreach (Transform child in statueSpawnPoint.transform)
        {
            if(child.transform.childCount > 0)
                Destroy(child.transform.GetChild(0).gameObject);
            GameObject estatua = Instantiate(statueObject, child.transform.position, Quaternion.identity, child.transform) as GameObject;
            estatua.GetComponent<CollectableObject>().SetSpawner(statueNumberSpawnPoints, numberPrefab);
            estatua.GetComponent<CollectableObject>().InitializeData(numbers[i].operation, numbers[i].number.ToDouble(), true, dialogues[i], questOrder);
            estatua.GetComponent<CollectableObject>().EnableInteractions();
            i = (++i) % 2;
        }
    }
}
