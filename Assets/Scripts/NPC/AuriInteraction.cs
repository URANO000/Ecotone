using TMPro;
using UnityEngine;

public class AuriInteraction : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject interactionPrompt;
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TMP_Text dialogueText;

    [Header("Dialogue")]
    [TextArea(3, 6)]
    [SerializeField]
    private string dialogueMessage =
        "Auri:\nBienvenido al bosque, Gusgus.\n\nExplora con cuidado, salta entre plataformas y observa el entorno.\nAlgunos caminos no se resuelven corriendo, sino pensando.";

    private bool playerInRange;
    private bool dialogueOpen;

    private void Start()
    {
        if (interactionPrompt != null)
            interactionPrompt.SetActive(false);

        if (dialoguePanel != null)
            dialoguePanel.SetActive(false);
    }

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (dialogueOpen)
                CloseDialogue();
            else
                ShowDialogue();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;

            if (!dialogueOpen && interactionPrompt != null)
                interactionPrompt.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            CloseDialogue();

            if (interactionPrompt != null)
                interactionPrompt.SetActive(false);
        }
    }

    private void ShowDialogue()
    {
        dialogueOpen = true;

        if (interactionPrompt != null)
            interactionPrompt.SetActive(false);

        if (dialoguePanel != null)
            dialoguePanel.SetActive(true);

        if (dialogueText != null)
            dialogueText.text = dialogueMessage;
    }

    private void CloseDialogue()
    {
        dialogueOpen = false;

        if (dialoguePanel != null)
            dialoguePanel.SetActive(false);

        if (playerInRange && interactionPrompt != null)
            interactionPrompt.SetActive(true);
    }
}