using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public TMP_Text dialogueText; //assign in the inspector
    public Button choicePrefab; // assign a prefab with a button in the inspector
    public Transform choicePanel; //the parent panel for choice buttons

    private Queue<DialogueNode> nodesQueue = new Queue<DialogueNode>();

    private DialogueNode currentNode;

    public void StartDialogue(Dialogue dialogue)
    {
        nodesQueue.Clear();

        DialogueNode startNode = dialogue.startNode;

        foreach (var alternative in dialogue.alternativeDialogues)
        {
            if (!string.IsNullOrEmpty(alternative.flagRequired) && GameStateManager.Instance.GetFlag(alternative.flagRequired))
            {
                startNode = alternative.alternativeStartNode;
                break; // break if you want only the first matching alternative to run, otherwise remove the break to let the last matching alternative to take precedence
            }
        }

        nodesQueue.Enqueue(startNode);
        
        DisplayNextNode();
    }

    public void EndDialogue()
    {
        dialogueText.text = "the end of the conversation.";
    }



    private void DisplayNextNode()
    {
        if(nodesQueue.Count == 0)
        {
            EndDialogue();
            return;
        }

        currentNode = nodesQueue.Dequeue();
        dialogueText.text = currentNode.dialogueText;

        foreach (Transform child in choicePanel)
        {
            Destroy(child.gameObject);
        }

        foreach(DialogueChoice choice in currentNode.choices)
        {
            Button choiceButton = Instantiate(choicePrefab, choicePanel);
            choiceButton.GetComponentInChildren<TMP_Text>().text = choice.choiceText;
            choiceButton.onClick.AddListener(delegate { MakeChoice(choice); });
        }
    }



    private void MakeChoice(DialogueChoice choice)
    {
        foreach(Transform child in choicePanel)
        {
            Button button = child.GetComponent<Button>();
            button.onClick.RemoveAllListeners();
            Destroy(child.gameObject);
        }

        if (!string.IsNullOrEmpty(choice.flagToSet))
        {
            GameStateManager.Instance.SetFlag(choice.flagToSet, choice.flagValue);
        }


        if (choice.nextNode != null)
        {

            nodesQueue.Enqueue(choice.nextNode);
        }

        DisplayNextNode();
    }
}
