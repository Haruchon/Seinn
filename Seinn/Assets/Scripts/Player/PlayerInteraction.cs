using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class PlayerInteraction : MonoBehaviour
{

    public bool isInteracting = false;

    private Object currentIntObjData;

    [SerializeField]
    private GameObject currentIntObj;

    //[SerializeField]
    //private string currentInteractableObjName = null;

    [SerializeField]
    public bool isDialogueActive = false;
    [SerializeField]
    public bool isQuestDialogueActive = false;

    private void Start()
    {
        GameEvents.current.onDisableInteractions += DisableInteractions;
        GameEvents.current.onEnableInteractions += EnableInteractions;
    }

    private void DisableInteractions()
    {
        this.enabled = false;
    }

    private void EnableInteractions()
    {
        this.enabled = true;
    }

    private void Update()
    {
        if (Input.GetButtonDown("Interact") && isInteracting)
        {
            //Debug.Log("Presionando boton");
            if (isQuestDialogueActive)
            {
                switch (Input.GetAxisRaw("Interact"))
                {
                    case 1:
                        AcceptQuest();
                        break;
                    case -1:
                        DeclineQuest();
                        break;
                    default:
                        break;
                }
            }
            else
            {
                Interact();
            }   
        }
    }

    public void Interact()
    {
        if (isDialogueActive)
        {
            //Debug.Log("Desactivando dialogo");
            currentIntObjData.DisplayNextSentence();
        }
        else
        {
            //Debug.Log("Activando dialogo");
            currentIntObjData.StartDialogue();
        }
    }

    public void AcceptQuest()
    {
        if (isInteracting)
            currentIntObjData.AcceptDialogue();
    }

    public void DeclineQuest()
    {
        if (isInteracting)
            currentIntObjData.DeclineDialogue();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("InteractableObject"))
        {
            //Debug.Log(collision.name);
            currentIntObj = collision.gameObject;
            currentIntObjData = currentIntObj.GetComponent<Object>();
            isInteracting = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("InteractableObject"))
        {
            if(collision.gameObject == isInteracting)
            {
                isInteracting = false;
                currentIntObj = null;
                currentIntObjData = null;
            }
            
        }
    }
}
