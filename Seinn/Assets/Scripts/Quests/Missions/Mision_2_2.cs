using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mision_2_2 : Quest
{
    private const string Npc = "Cofre";
    private const string keyDialogue = "Encontraste la llave!";
    private const string nothingDialogue = "El cofre esta vacío...";
    private List<DialogueElement> keyBoxDialogue;
    private List<DialogueElement> nothingBoxDialogue;

    public GameObject enemyNumberSpawnPoints;

    public GameObject boxSpawnPoints;
    public GameObject boxObject;

    public GameObject enemySpawnPoint;
    public GameObject enemyObject;

    public override void InitializeParameters()
    {
        keyBoxDialogue = new List<DialogueElement>();
        nothingBoxDialogue = new List<DialogueElement>();

        keyBoxDialogue.Add(new DialogueElement(Npc, new string[] { keyDialogue }));
        keyBoxDialogue.Add(new DialogueElement(Npc, new string[] { keyDialogue }));
        keyBoxDialogue.Add(new DialogueElement(Npc, new string[] { keyDialogue }));
        keyBoxDialogue.Add(new DialogueElement(Npc, new string[] { keyDialogue }));
        nothingBoxDialogue.Add(new DialogueElement(Npc, new string[] { nothingDialogue }));
        nothingBoxDialogue.Add(new DialogueElement(Npc, new string[] { nothingDialogue }));
        nothingBoxDialogue.Add(new DialogueElement(Npc, new string[] { nothingDialogue }));
        nothingBoxDialogue.Add(new DialogueElement(Npc, new string[] { nothingDialogue }));
    }

    void Start()
    {
        //InitializeParameters();
        id = 10006;
        questOrder = 6;
        questTitle = "Segundo comandante";
        questDescription = "Consigue la llave y derrota al enemigo!";
        completed = false;
        acceptDialogue = new DialogueElement("", new string[] { "No debo olvidar el encontrar la llave, debe estar en algún cofre" });
        declineDialogue = new DialogueElement("", new string[] { "..." });
        InitializeParameters();

        goals.Add(new CollectionGoal(boxObject.GetComponent<CollectableObject>().id, 9, "Consigue la llave escondida en el cofre", false, 0, 1));
        goals.Add(new KillGoal(enemyObject.GetComponent<Enemy>().id, 10, "Derrota al enemigo y sal de la sala", false, 0, 1));
        goals.ForEach(g => g.Init());

    }

    public override void InitializeObjects()
    {
        base.InitializeObjects();

        int i = 0;
        foreach (Transform child in boxSpawnPoints.transform)
        {
            if (i == 0)
            {
                GameObject box = Instantiate(boxObject, child.transform.position, Quaternion.identity, child.transform) as GameObject;
                box.GetComponent<CollectableObject>().InitializeData(-1, -1, false, keyBoxDialogue, questOrder);
                box.GetComponent<CollectableObject>().EnableInteractions();
            }
            else
            {
                GameObject box = Instantiate(boxObject, child.transform.position, Quaternion.identity, child.transform) as GameObject;
                box.GetComponent<CollectableObject>().InitializeData(-1, -1, false, nothingBoxDialogue, questOrder);
                box.GetComponent<CollectableObject>().EnableInteractions();
                box.GetComponent<CollectableObject>().id = 999;
            }
            i++;
        }
        Destroy(enemySpawnPoint.transform.GetChild(0).gameObject);
        GameObject enemy = Instantiate(enemyObject, enemySpawnPoint.transform.position, Quaternion.identity, enemySpawnPoint.transform) as GameObject;
        enemy.GetComponent<Enemy>().SetSpawner(enemyNumberSpawnPoints, questOrder);
    }

}
