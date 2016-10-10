﻿using UnityEngine;
using System.Collections;

public class AccuseDoppelganger : MonoBehaviour {

    // Linear text iterates through the dialogue array.
    // An empty string causes the dialogue to end.
    // The elements of the first four arrays with the same index correspond to each other.
    // The number of choices is 0 for linear text.
    // If the number of choices is greater than 0 then the start index in the choices array will be
    // a valid value that indicates which index to start reading from in the choices array.
    // Dialogue effect indicates which block of code to execute in the the npc effects script.
    // If dialogue effect is 0, then there is no effect.

    [Header("Next Four Must Be Equal Size")]
    string[] dialogue;
    int[] numberOfChoices;
    int[] startIndexInChoices;
    int[] dialogueEffect;

    // The elements of the final two arrays with the same index correspond to each other.
    // Each choice in the choices array will have a corresponding index to which to skip to in the 
    // dialogue array.
    [Header("Next Two Must Be Equal Size")]
    string[] choices;
    int[] nextIndexInDialogue;

    void Start()
    {
        dialogue = new string[] {
            "Do you want to accuse this person of being a doppelganger and attack them?",
            "You attacked an innocent townsperson! As a result, you've been kicked out of town and failed your mission.",
            "",
            "You decided not to attack.",
            ""
        };

        numberOfChoices = new int[] { 2, 0, 0, 0, 0};
        startIndexInChoices = new int[] { 0, 0, 0, 0, 0};
        dialogueEffect = new int[] { 0, 1, 0, 0, 0};

        choices = new string[]{
            "Yes.",
            "No."
        };
        nextIndexInDialogue = new int[] { 1, 3 };
    }

    public IEnumerator Accuse(string npcName)
    {        
        switch (npcName)
        {
            case "Dwight":
            case "LaMarcus":
            case "Darko":
                dialogue[1] = "You correctly identified a shapeshifter!";
                dialogueEffect[1] = 2;
                break;
            default:
                break;
        }
        
        DialogueController.DialogueScript ds = new DialogueController.DialogueScript();
        ds.dialogue = dialogue;
        ds.numberOfChoices = numberOfChoices;
        ds.startIndexInChoices = startIndexInChoices;
        ds.dialogueEffect = dialogueEffect;
        ds.choices = choices;
        ds.nextIndexInDialogue = nextIndexInDialogue;

        StartCoroutine(this.gameObject.GetComponent<DialogueController>().StartDialogue(ds));

        yield break;
    }
}
