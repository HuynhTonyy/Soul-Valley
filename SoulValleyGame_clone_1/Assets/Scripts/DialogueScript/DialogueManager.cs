using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public Image actorImage;
    public TMPro.TextMeshProUGUI actorName; 
    public TMPro.TextMeshProUGUI MessageText;
    public RectTransform backgroundBox; 

    Message[] currentMessages;
    Actor[] currentActors; 
    int activeMessage = 0; // Index of the currently displayed message
    public static bool isActive = false; // Indicates if the dialogue is currently active

    public void OpenDialogue(Message[] messages, Actor[] actors)
    {
        currentMessages = messages;
        currentActors = actors; 
        activeMessage = 0; 
        isActive = true; 
        Debug.Log("Strarted conversation! Loaded messages: " + messages.Length);
        DisplayMessage(); // Display the first message
        backgroundBox.LeanScale(Vector3.one, 0.5f);
    }

    void DisplayMessage()
    {
        Message messageToDisplay = currentMessages[activeMessage]; // Get the current message from the currentMessages array
        MessageText.text = messageToDisplay.message; // Set the message text to the message from the current message object

        Actor actorToDisplay = currentActors[messageToDisplay.actorId]; // Get the actor associated with the current message
        actorName.text = actorToDisplay.name; // Set the actor's name to the name from the actor object
        actorImage.sprite = actorToDisplay.sprite; // Set the actor's image to the sprite from the actor object

        AnimateTextColor(); // Animate the text color of the message text
    }

    public void NextMessage()
    {
        activeMessage++;
        if (activeMessage < currentMessages.Length) 
        {
            DisplayMessage(); // Display the next message
        }
        else
        {
            Debug.Log("Conversation ended");
            backgroundBox.LeanScale(Vector3.zero, 0.5f).setEaseInOutExpo(); // Animate the background box to scale to zero
            isActive = false; // Set isActive to false, indicating that the dialogue has ended
        }
    }

    void AnimateTextColor()
    {
        LeanTween.textAlpha(MessageText.rectTransform, 0, 0); // Set the initial text alpha to 0
        LeanTween.textAlpha(MessageText.rectTransform, 1, 0.5f); // Animate the text alpha to 1 over 0.5 seconds
    }

    void Start()
    {
        backgroundBox.transform.localScale = Vector3.zero; // Set the initial scale of the background box to zero
    }

    void Update()
    {
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Mouse0)) && isActive == true) // Check if the spacebar or left mouse button is pressed and the dialogue is active
        {
            NextMessage(); // Proceed to the next message
        }
    }
}
