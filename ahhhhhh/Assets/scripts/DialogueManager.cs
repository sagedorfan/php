using System.Collections; //This defines the specific elements within the UI
using System.Collections.Generic; //This is used to categorise the elements
using UnityEngine;
using UnityEngine.UI; //This will be used to update the UI (to displaythe dialogue...or not)
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI nameText; //This is the name of the character (Fine Shyt) that will be displayed in the UI (instead of the consoul? console... yeah)
    public TextMeshProUGUI dialogueText;
    private Queue<string> sentences; //The is an array of dialogue lines

    void Start()
    {
        sentences = new Queue<string>(); //This creates a new queue to store the dialogue line
    }
    public void StartDialogue(Dialogue dialogue)
    {
        //Debug.Log("Starting conversation with " + dialogue.name); //Displays character (Fine Shyt's) name (console purposes bc i like testing :3)

        nameText.text = dialogue.name;
        sentences.Clear(); //This clears the queue of any previous dialogue lines

        foreach (string sentence in dialogue.sentences) //This loops through each sentence in the list
        {
            sentences.Enqueue(sentence);
        }
        DisplayNextSentence(); //Shows the next sentence or smth i dont fucking know

    }
    public void DisplayNextSentence()
    {
        if (sentences.Count == 0) //I think it prevents sentences from being looped???? idk tho that's just me 
        {
            EndDialogue(); //This ends the dialogue
            return;
        }
        string sentence = sentences.Dequeue(); //Diplays the line in the UI, I guess
                                               //Debug.Log(sentence); //This is for testing purposes again)
        dialogueText.text = sentence; //This updates the dialogue text in the UI (there was a debug here. The debug was for shits n gigs)
    }
    void EndDialogue()
    {
        Debug.Log("End of conversation"); //The end <3 (Hi I lost my mind while coding this, just ignore the loss of formalities)
    }
}