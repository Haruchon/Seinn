using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mision_1_1 : Quest
{
    public GameObject spawnPoints;
    public GameObject enemySpawnPoint;

    public GameObject numberPrefab;
    public GameObject enemyObject;

    void Start()
    {
        id = 10001;
        questOrder = 1;
        questTitle = "Arbolito malvado";
        questDescription = "Derrota al arbolito malvado al noreste del pueblo";
        completed = false;
        acceptDialogue = new DialogueElement("Mama", new string[] {"Vuelve cuando derrotes al arbol malvado."});
        declineDialogue = new DialogueElement("Mama", new string[] { "Vaya, estas ocupado?", "Vuelve cuando tengas tiempo" });

        goals.Add(new KillGoal(enemyObject.GetComponent<Enemy>().id,1, "Derrota al arbolito malvado!", false, 0, 1));
        goals.ForEach(g => g.Init());

    }

    public override void InitializeObjects()
    {
        base.InitializeObjects();

        foreach (Transform child in enemySpawnPoint.transform)
            GameObject.Destroy(child.gameObject);
        GameObject enemy = Instantiate(enemyObject, enemySpawnPoint.transform.position, Quaternion.identity, enemySpawnPoint.transform) as GameObject;
        enemy.GetComponent<Enemy>().SetSpawner(spawnPoints, questOrder);

    }


}
    