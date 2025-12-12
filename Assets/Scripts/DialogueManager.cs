using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class DialogueManager : MonoBehaviour
{
    [SerializeField]
    [TextArea(2, 15)]
    string[] dialogueBoxes;

    [SerializeField] TextMeshProUGUI dialogueText;
    [SerializeField] TextMeshProUGUI nextText;
    [SerializeField] SceneLoader sceneLoader;
    [SerializeField] float typingSpeed = 0.05f;

    int currentDialogueBox = 0;

    InputAction nextDialogue;
    int currentLetterIdx;
    float timeSinceLastLetter;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        nextDialogue = InputSystem.actions.FindAction("NextDialogue");

        currentDialogueBox = 0;
        UpdateTextBox();
    }

    void Update()
    {
        if (nextDialogue.WasPressedThisFrame())
        {
            if (currentLetterIdx <= dialogueText.text.Length)
            {
               currentLetterIdx = dialogueText.text.Length;
               ShowNextCharacter();
            }
            else
            { 
                UpdateTextBox();
            }
        }

        if (currentLetterIdx <= dialogueText.text.Length)
        {
            timeSinceLastLetter += Time.deltaTime;

            if (timeSinceLastLetter >= typingSpeed)
            {
                ShowNextCharacter();
            }
        }
    }

    private void ShowNextCharacter()
    {
        dialogueText.maxVisibleCharacters = currentLetterIdx;
        timeSinceLastLetter = 0;
        currentLetterIdx++;

        if (currentLetterIdx > dialogueText.text.Length)
        {
            nextText.maxVisibleCharacters = nextText.text.Length;
        }
    }

    public void UpdateTextBox()
    {
        if (currentDialogueBox == dialogueBoxes.Length){
            if(SceneManager.GetActiveScene().buildIndex == 5)
            PlayerData.IsOutroComplete = 1;
            if (SceneManager.GetActiveScene().buildIndex == 1)
                PlayerData.IsIntroComplete = 1;
            sceneLoader.LoadNextScene();
            return;
        }

        dialogueText.text = dialogueBoxes[currentDialogueBox];
        dialogueText.maxVisibleCharacters = 1;

        nextText.maxVisibleCharacters = 0;
    
        currentLetterIdx = 1;
        timeSinceLastLetter = 0;
        currentDialogueBox++;
    }
}
