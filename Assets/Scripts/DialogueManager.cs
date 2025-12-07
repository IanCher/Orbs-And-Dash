using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class DialogueManager : MonoBehaviour
{
    [SerializeField]
    [TextArea(2, 15)]
    string[] dialogueBoxes;

    [SerializeField] TextMeshProUGUI text;
    [SerializeField] SceneLoader sceneLoader;
    [SerializeField] float typingSpeed = 0.05f;

    int currentDialogueBox = 0;

    InputAction nextDialogue;

    string targetText;
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
            UpdateTextBox();
        }

        if (currentLetterIdx < targetText.Length)
        {
            timeSinceLastLetter += Time.deltaTime;

            if (timeSinceLastLetter >= typingSpeed)
            {
                text.text += targetText[currentLetterIdx];
                timeSinceLastLetter = 0;
                currentLetterIdx++;
            }
        }
    }

    public void UpdateTextBox()
    {
        if (currentDialogueBox == dialogueBoxes.Length){
            sceneLoader.LoadNextScene();
            return;
        }

        targetText = dialogueBoxes[currentDialogueBox];
        text.text = targetText[0].ToString();
    
        currentLetterIdx = 1;
        timeSinceLastLetter = 0;
        currentDialogueBox++;
    }
}
