using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mision_2_4 : Quest
{
    private const string Npc = "Arbusto";
    private const string goodDialogue = "Parece que este arbusto puede crecer... Listo!";
    private const string badDialogue = "Este arbusto ya está marchito";
    private List<DialogueElement> goodPlantDialogue;
    private List<DialogueElement> badPlantDialogue;
    private bool isEven;

    public GameObject block;
    public GameObject leaveBlock;
    public GameObject leaveWarp;
    public GameObject doorDialogue;

    public GameObject enemyNumberSpawnPoints;

    public GameObject plantSpawnPoints;
    public GameObject plantObject;
    public GameObject grewPlantObject;

    public GameObject enemySpawnPoint;
    public GameObject enemyObject;

    public override void InitializeParameters()
    {
        goodPlantDialogue = new List<DialogueElement>();
        badPlantDialogue = new List<DialogueElement>();

        goodPlantDialogue.Add(new DialogueElement(Npc, new string[] { goodDialogue }));
        goodPlantDialogue.Add(new DialogueElement(Npc, new string[] { goodDialogue }));
        goodPlantDialogue.Add(new DialogueElement(Npc, new string[] { goodDialogue }));
        goodPlantDialogue.Add(new DialogueElement(Npc, new string[] { goodDialogue }));
        badPlantDialogue.Add(new DialogueElement(Npc, new string[] { badDialogue }));
        badPlantDialogue.Add(new DialogueElement(Npc, new string[] { badDialogue }));
        badPlantDialogue.Add(new DialogueElement(Npc, new string[] { badDialogue }));
        badPlantDialogue.Add(new DialogueElement(Npc, new string[] { badDialogue }));
    }

    void Start()
    {
        //InitializeParameters();
        int random = Random.Range(0, 3);
        isEven = (random % 2 == 0) ? true : false;
        id = 10008;
        questOrder = 8;
        questTitle = "Cuarto comandante";
        questDescription = "Haz crecer los arbustos necesarios para luego derrotar al enemigo!";
        completed = false;
        acceptDialogue = new DialogueElement("", new string[] { "¿Cuáles de todos los arbustos podrán crecer?" });
        declineDialogue = new DialogueElement("", new string[] { "..." });
        InitializeParameters();

        goals.Add(new CollectionGoal(plantObject.GetComponent<CollectableObject>().id, 13, "Haz crecer 5 arbustos para despertar al enemigo", false, 0, 5));
        goals.Add(new KillGoal(enemyObject.GetComponent<Enemy>().id, 14, "Derrota al enemigo y sal de la sala", false, 0, 1));
        goals.ForEach(g => g.Init());

        GameEvents.current.onGoalCompletion += OnFirstGoalCompletion;
        GameEvents.current.onCollectedOrDestroyedObject += GrowTree;
        GameEvents.current.onQuestCompleted += QuestCompleted;

    }

    public void QuestCompleted(Quest quest)
    {
        if (quest.id == this.id)
        {
            Destroy(block);
            Destroy(leaveBlock);
            leaveWarp.SetActive(true);
            doorDialogue.SetActive(false);
        }
    }

    public void GrowTree(CollectableObject obj)
    {
        if (obj.id == plantObject.GetComponent<CollectableObject>().id)
        {
            Instantiate(grewPlantObject, obj.transform.position, Quaternion.identity, obj.transform.parent.transform);
        }
    }

    public void OnFirstGoalCompletion(Goal goal)
    {
        if (goal.goalID == 13)
        {
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
        foreach (Transform child in plantSpawnPoints.transform)
        {
            GameObject pot = Instantiate(plantObject, child.transform.position, Quaternion.identity, child.transform) as GameObject;
            if ((isEven && i % 2 == 0) || (!isEven && i % 2 != 0))
            {
                pot.GetComponent<CollectableObject>().InitializeData(-1, -1, false, goodPlantDialogue, questOrder);
                pot.GetComponent<CollectableObject>().EnableInteractions();
            }
            else
            {
                pot.GetComponent<CollectableObject>().InitializeData(-1, -1, false, badPlantDialogue, questOrder);
                pot.GetComponent<CollectableObject>().EnableInteractions();
                pot.GetComponent<CollectableObject>().id = 999;
            }
            i++;
        }
    }
}
