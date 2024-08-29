using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mision_1_2 : Quest
{
    private const string extraDialogue = "¿Lo lograste? Veamos...";
    private const string correctDialogue = "¡Oh no! ¡Es correcto! Noooo... ";
    private const string incorrectDialogue = "Vaya, te has equivocado...\nVuelve a intentarlo...\nMuajajaja";
    private const string thanksDialogue = "Gracias por tu ayuda, con gusto iré contigo";
    private const string NpcBad = "Siempreverde Malvado";
    private const string NpcGood = "Siempreverde Azul";
    private int numObjects;
    private List<List<DialogueElement>> dialogues = new List<List<DialogueElement>>();
    private List<DialogueElement> goodPLantDialogues;
    private List<CustomNumber> numbers;

    public GameObject spawnPoints;
    public GameObject numberPrefab;
    public GameObject badPlantSpawnPoints;
    public GameObject badPlants;
    public GameObject goodPlantSpawnPoints;    
    public GameObject goodPlant;

    public GameObject goodPlantSpawn;

    public override void InitializeParameters()
    {
        numbers = new List<CustomNumber>();
        goodPLantDialogues = new List<DialogueElement>();
        for (int j = 0; j < numObjects; j++)
        {
            numbers.Add(NumberConfig.current.GetNewNumber(questOrder));

            List<DialogueElement> primaryDialoge = new List<DialogueElement>();
            string numberToShow = "Muajaja\nSolo me podrás cortar si consigues el número " + numbers[j].ToString() + " con el color " + numbers[j].color;
            string rememberNumber = "Recuerda que debes conseguir el número " + numbers[j].ToString() + " con el color " + numbers[j].color;
            primaryDialoge.Add(new DialogueElement(NpcBad, new string[] { numberToShow })); //dialogo normal
            primaryDialoge.Add(new DialogueElement(NpcBad, new string[] { extraDialogue })); //dialogo extra 
            primaryDialoge.Add(new DialogueElement(NpcBad, new string[] { extraDialogue + "\n"  + correctDialogue })); //dialogo correcto
            primaryDialoge.Add(new DialogueElement(NpcBad, new string[] { extraDialogue, incorrectDialogue, rememberNumber })); //dialogo incorrecto
            dialogues.Add(primaryDialoge);

            goodPLantDialogues.Add(new DialogueElement(NpcGood, new string[] { thanksDialogue }));
        }
    }

    void Start()
    {
        id = 10002;
        questOrder = 2;
        questTitle = NpcGood;
        questDescription = "Tu madre siempre quiso un Siempreverde azul para su pequeño jardín";
        completed = false;
        acceptDialogue = new DialogueElement("Papa", new string[] { "No demores mucho!" });
        declineDialogue = new DialogueElement("Papa", new string[] { "Vaya, estas ocupado?", "Vuelve cuando tengas tiempo" });
        numObjects = badPlantSpawnPoints.transform.childCount;
        InitializeParameters();

        goals.Add(new CollectionGoal(badPlants.GetComponent<CollectableObject>().id,2, "Poda los 4 Siempreverdes oscuros que rodean al Siempreverde azul", false, 0, numObjects));
        goals.Add(new CollectionGoal(goodPlant.GetComponent<CollectableObject>().id,3, "Obten el Siempreverde azul y llévaselo a tu madre!", false, 0, 1));
        goals.ForEach(g => g.Init());

        GameEvents.current.onGoalCompletion += OnFirstGoalCompletion;
    }

    public void OnFirstGoalCompletion(Goal goal)
    {
        if(goal.goalID == 2)
        {
            goodPlantSpawn.GetComponent<CollectableObject>().EnableInteractions();
            GameEvents.current.onGoalCompletion -= OnFirstGoalCompletion;
        }
    }

    public override void InitializeObjects()
    {
        base.InitializeObjects();
        int i = 0;
        foreach (Transform child in badPlantSpawnPoints.transform)
        {
            GameObject bPlant = Instantiate(badPlants, child.transform.position, Quaternion.identity, child.transform) as GameObject;
            bPlant.GetComponent<CollectableObject>().SetSpawner(spawnPoints,numberPrefab);
            bPlant.GetComponent<CollectableObject>().InitializeData(numbers[i].operation, numbers[i].number.ToDouble(), true, dialogues[i],questOrder);
            bPlant.GetComponent<CollectableObject>().EnableInteractions();
            i++;
        }

        goodPlantSpawn = Instantiate(goodPlant, goodPlantSpawnPoints.transform.position, Quaternion.identity, goodPlantSpawnPoints.transform) as GameObject;
        goodPlantSpawn.GetComponent<CollectableObject>().InitializeData(-1,-1,false, goodPLantDialogues,questOrder);
        goodPlantSpawn.GetComponent<CollectableObject>().DisableInteractions();
    }

}
