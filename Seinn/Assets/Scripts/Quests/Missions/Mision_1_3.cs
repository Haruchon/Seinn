using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mision_1_3 : Quest
{
    private const string Npc = "Estatua Misteriosa";
    private const string extraDialogue = "Conseguiste el numero que te dije? Veamos...";
    private const string correctDialogue = "Es correcto! Ahora al siguiente acertijo...";
    private const string incorrectDialogue = "Vaya, te has equivocado... Vuelve a intentarlo";
    private int numObjects = 3;
    private int i;
    private List<List<DialogueElement>> dialogues = new List<List<DialogueElement>>();
    private List<CustomNumber> numbers;

    public GameObject numberSpawnPoints;
    public GameObject numberPrefab;
    public GameObject statueSpawnPoint;
    public GameObject statueObject;
    

    public override void InitializeParameters()
    {
        numbers = new List<CustomNumber>();
        for (int j = 0; j < numObjects; j++)
        {
            numbers.Add(NumberConfig.current.GetNewNumber(questOrder));

            List<DialogueElement> primaryDialoge = new List<DialogueElement>();
            string numberToShow = "¿Podrás conseguir un número igual a " + numbers[j].ToString() + "? Debe estar en el color " + numbers[j].color;
            string rememberNumber = "Recuerda que debes conseguir un número igual a " + numbers[j].ToString() + ".\nDebe estar en el color " + numbers[j].color;
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
        i = 0;
        id = 10003;
        questOrder = 3;
        questTitle = "Resolviendo acertijos";
        questDescription = "Debes cumplir acertijos para pasar por este camino.";
        completed = false;
        acceptDialogue = new DialogueElement("Hombre misterioso", new string[] { "Adelante... \nPuedes hablar con las estatuas para conocer los acertijos" });
        declineDialogue = new DialogueElement("Hombre misterioso", new string[] { "Entiendo... \nDeberas cumplir con los acertijos si decides pasar por aqui" });
        InitializeParameters();

        goals.Add(new CollectionGoal(statueObject.GetComponent<CollectableObject>().id, 4, "Cumple con los 3 acertijos que la estatua tiene para ti", false, 0, 3));
        goals.ForEach(g => g.Init());

        GameEvents.current.onCollectedOrDestroyedObject += ReinstantiateStatues;

    }

    public void ReinstantiateStatues(CollectableObject obj)
    {
        if(obj.id == statueObject.GetComponent<CollectableObject>().id)
        {
            if (!this.goals[0].completed) 
            {
                InstantiateStatue();
            }
        }
    }

    private void InstantiateStatue()
    {
        foreach (Transform child in statueSpawnPoint.transform)
            GameObject.Destroy(child.gameObject);
        GameObject estatua = Instantiate(statueObject, statueSpawnPoint.transform.position, Quaternion.identity, statueSpawnPoint.transform) as GameObject;
        estatua.GetComponent<CollectableObject>().SetSpawner(numberSpawnPoints, numberPrefab);
        estatua.GetComponent<CollectableObject>().InitializeData(numbers[i].operation, numbers[i].number.ToDouble(), true, dialogues[i],questOrder);
        estatua.GetComponent<CollectableObject>().EnableInteractions();
        i++;
    }

    public override void InitializeObjects()
    {
        base.InitializeObjects();   

        InstantiateStatue();
        
        //bPlant.GetComponent<CollectableObject>().InitializeData(operations[i], values[i], true, dialogues[i]);
    }


}
