using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public Image actorImage; // Reference to the Image component used to display the actor's image
    public TMPro.TextMeshProUGUI actorName; // Reference to the TextMeshProUGUI component used to display the actor's name
    public TMPro.TextMeshProUGUI MessageText; // Reference to the TextMeshProUGUI component used to display the message text
    public RectTransform backgroundBox; // Reference to the RectTransform component used to control the background box appearance

    Message[] currentMessages; // Array to store the current set of messages
    Actor[] currentActors; // Array to store the current set of actors
    int activeMessage = 0; // Index of the currently displayed message
    public static bool isActive = false; // Indicates if the dialogue is currently active

    public void OpenDialogue(Message[] messages, Actor[] actors)
    {
        currentMessages = messages; // Assign the passed-in messages to the currentMessages array
        currentActors = actors; // Assign the passed-in actors to the currentActors array
        activeMessage = 0; // Reset the active message index to 0
        isActive = true; // Set isActive to true, indicating that the dialogue is active
        Debug.Log("Strarted conversation! Loaded messages: " + messages.Length); // Log a debug message
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
        activeMessage++; // Increment the active message index
        if (activeMessage < currentMessages.Length) // Check if there are more messages to display
        {
            DisplayMessage(); // Display the next message
        }
        else
        {
            Debug.Log("Conversation ended"); // Log a debug message indicating the conversation has ended
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
