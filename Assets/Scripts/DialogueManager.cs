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

    int currentDialogueBox = 0;

    InputAction nextDialogue;

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
    }

    public void UpdateTextBox()
    {
        if (currentDialogueBox == dialogueBoxes.Length){
            sceneLoader.LoadNextScene();
            return;
        }

        text.text = dialogueBoxes[currentDialogueBox];
        currentDialogueBox++;
    }
}
