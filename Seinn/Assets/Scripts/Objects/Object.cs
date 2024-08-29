using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object : MonoBehaviour{

    public virtual void AcceptDialogue() { }
    public virtual void DeclineDialogue() { }
    public virtual void DisplayNextSentence() { }
    public virtual void StartDialogue() { }
}
