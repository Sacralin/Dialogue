using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueNode
{
    public string dialogueText;
    public List<DialogueChoice> choices;
}

[System.Serializable]
public class DialogueChoice
{
    public string choiceText;
    public DialogueNode nextNode;
    public string flagToSet; //optional flag to set based on choice made
    public bool flagValue;
}

[CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogue")]
public class Dialogue : ScriptableObject
{
    public DialogueNode startNode;
    public List<AlternativeDialogue> alternativeDialogues;
}

[System.Serializable]
public class AlternativeDialogue
{
    public string flagRequired;
    public DialogueNode alternativeStartNode;

}


