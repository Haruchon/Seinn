using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mision_2_1 : Quest
{
    private const string Npc = "Estatua misteriosa";
    private const string extraDialogue = "Conseguire el numero correcto? Veamos...";
    private const string correctDialogue = "Es correcto!";
    private const string incorrectDialogue = "Vaya, me he equivocado... Lo volveré a intentar";
    private int numObjects = 4;
    private List<List<DialogueElement>> dialogues = new List<List<DialogueElement>>();
    private List<CustomNumber> numbers;

    public GameObject[] statueNumberSpawnPoints;
    public GameObject enemyNumberSpawnPoints;
    public GameObject numberPrefab;

    public GameObject statueSpawnPoint;
    public GameObject statueObject;

    public GameObject enemySpawnPoint;
    public GameObject enemyObject;

    public GameObject fire;

    public override void InitializeParameters()
    {
        numbers = new List<CustomNumber>();

        for (int j = 0; j < numObjects; j++)
        {
            numbers.Add(NumberConfig.current.GetNewNumber(questOrder));

            List<DialogueElement> primaryDialoge = new List<DialogueElement>();
            string numberToShow = "Para desactivar esta estatua debes conseguir un número igual a " + numbers[j].ToString() + " y usar el color " + numbers[j].color;
            string rememberNumber = "Recuerda que debes conseguir un número igual a " + numbers[j].ToString() + " y usar el color " + numbers[j].color;
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
        id = 10005;
        questOrder = 5;
        questTitle = "Primer comandante";
        questDescription = "¡Desactiva las estatuas y derrota al enemigo!";
        completed = false;
        acceptDialogue = new DialogueElement("", new string[] { "Debo desactivar las estatuas en las esquinas del cuarto para despertar al enemigo y derrotarlo" });
        declineDialogue = new DialogueElement("", new string[] { "..." });
        InitializeParameters();

        goals.Add(new CollectionGoal(statueObject.GetComponent<CollectableObject>().id, 7, "Sigue las instrucciones de las 4 estatuas para desactivarlas", false, 0, 4));
        goals.Add(new KillGoal(enemyObject.GetComponent<Enemy>().id, 8, "Derrota al enemigo luego de desactivar las estatuas", false, 0, 1));
        goals.ForEach(g => g.Init());

        GameEvents.current.onGoalCompletion += OnFirstGoalCompletion;

    }

    public void OnFirstGoalCompletion(Goal goal)
    {
        if (goal.goalID == 7)
        {
            Destroy(fire);
            Destroy(enemySpawnPoint.transform.GetChild(0).gameObject);
            GameObject enemy = Instantiate(enemyObject, enemySpawnPoint.transform.position, Quaternion.identity, enemySpawnPoint.transform) as GameObject;
            enemy.GetComponent<Enemy>().SetSpawner(enemyNumberSpawnPoints, questOrder);
            GameEvents.current.onGoalCompletion -= OnFirstGoalCompletion;
        }
    }

    public override void InitializeObjects()
    {
        base.InitializeObjects();

        int i = 0;
        foreach (Transform child in statueSpawnPoint.transform)
        {
            GameObject estatua = Instantiate(statueObject, child.transform.position, Quaternion.identity, child.transform) as GameObject;
            estatua.GetComponent<CollectableObject>().SetSpawner(statueNumberSpawnPoints[i], numberPrefab);
            estatua.GetComponent<CollectableObject>().InitializeData(numbers[i].operation, numbers[i].number.ToDouble(), true, dialogues[i], questOrder);
            estatua.GetComponent<CollectableObject>().EnableInteractions();
            i++;
        }
    }

}
