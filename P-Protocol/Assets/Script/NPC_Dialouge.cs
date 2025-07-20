using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class NPC_Dialouge : MonoBehaviour
{
    public string[] dialogueLines;
    public GameObject interactionUI; 
    public TextMeshProUGUI dialogueText;

    private bool playerInRange = false;
    private bool isTalking = false;
    private bool hasTalked = false;
    private int currentLine = 0;

    private PlayerControls controls;
    private InputAction interactAction;


    private void Awake()
    {
        controls = new PlayerControls(); // 🔹 FIX: Create input actions properly
        interactAction = controls.Player.Interact;
        interactAction.performed += OnInteract;
    }

    private void OnEnable()
    {
        interactAction.Enable();
    }

    private void OnDisable()
    {
        interactAction.Disable();
    }
    private void Start()
    {
        if (interactionUI != null)
            interactionUI.SetActive(false);

        if (dialogueText != null)
            dialogueText.text = "";
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            if (!isTalking && interactionUI != null&& !hasTalked)
                interactionUI.SetActive(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            isTalking = false;
            currentLine = 0;

            if (interactionUI != null)
                interactionUI.SetActive(false);

            EndDialogue();
        }
    }
    private void OnInteract(InputAction.CallbackContext context)
    {
        if (!context.performed || !playerInRange || hasTalked) return;

        if (!isTalking)
        {
            isTalking = true;
            currentLine = 0;
            if (interactionUI != null)
                interactionUI.SetActive(false); // Hide "Press E" when talking starts
            ShowDialogue();
        }
        else
        {
            currentLine++;
            if (currentLine >= dialogueLines.Length)
            {
                isTalking = false;
                hasTalked = true;
                EndDialogue();
                
            }
            else
            {
                ShowDialogue();
            }
        }
    }

    private void ShowDialogue()
    {
        if (dialogueText != null)
            dialogueText.text = dialogueLines[currentLine];
    }

    private void EndDialogue()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        PlayerRageMeter plRM = player.GetComponent<PlayerRageMeter>();
        PlayerHealth plH = player.GetComponent<PlayerHealth>();
        plRM.DecreaseRage(50);
        plH.TakeHeal(50);
        if (dialogueText != null)
            dialogueText.text = "";

        GameObject NPC = GameObject.FindGameObjectWithTag("Npc");
        Destroy(NPC);

        //GetComponent<Collider>().enabled = false;
        //interactionUI.SetActive(false);
    }


    
}
