using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
//system.collections.generic and system.collections needed for us to use Queue type of data

public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;
    //so that it shows up on our textmesh/canvas/in-game

    private Queue<string> sentences;
    // what happens if this is public?
    //Queue is a FIFO collection type = First in First out (it might not be adequate for more evolved dialogue)

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sentences = new Queue<string>();
    }

    public void StartDialogue(Dialogue dialogue)

    {
        Debug.Log("probably someone talking" + dialogue.name);

        nameText.text = dialogue.name;
        //so that machine knows where we want the text in name (added from inspector) to be placed

        sentences.Clear();
        //removes all sentences that might already be there
        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }
        //to play the next sentence in the array (from the Dialogue script
        //dialogue script is a separate script for our dialogue arrays, the text of which is added from the inspector
        //(though I'm not entirely sure why seperation is necessary)

        DisplayNextSentence();
    }


    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        //if there's more to say
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
        //so that machine knows where we want to text in sentences (added from inspector) to be placed
    }
    IEnumerator TypeSentence(string sentence)

    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(0.1f);

            //this calls out characters in the sentence string array and asks the machine to wait a little before showing the next character
            //also known as type-writer effect
        }
    }
    void EndDialogue()
    {
            Debug.Log("wheoever was talking has stopped");
            //triggers dialogue box out animation   
    }


}
