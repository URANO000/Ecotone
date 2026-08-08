using System.Collections;
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
    [Header("Dialogue")]
    [SerializeField] private string[] dialoguePages;

    [Header("Typewriter Settings")]
    [SerializeField] private float typingSpeed = 0.035f;

    private bool playerInRange;
    private bool dialogueOpen;
    private bool isTyping;

    private int currentPageIndex;
    private Coroutine typingCoroutine;

    private void Start()
    {
        if (interactionPrompt != null)
            interactionPrompt.SetActive(false);

        if (dialoguePanel != null)
            dialoguePanel.SetActive(false);

        if (dialogueText != null)
            dialogueText.text = "";
    }

    private void Update()
    {
        if (!playerInRange || !Input.GetKeyDown(KeyCode.E))
            return;

        if (!dialogueOpen)
        {
            StartDialogue();
            return;
        }

        if (isTyping)
        {
            CompleteCurrentPage();
            return;
        }

        ShowNextPage();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        playerInRange = true;

        if (!dialogueOpen && interactionPrompt != null)
            interactionPrompt.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        playerInRange = false;
        CloseDialogue();

        if (interactionPrompt != null)
            interactionPrompt.SetActive(false);
    }

    private void StartDialogue()
    {
        dialogueOpen = true;
        currentPageIndex = 0;

        if (interactionPrompt != null)
            interactionPrompt.SetActive(false);

        if (dialoguePanel != null)
            dialoguePanel.SetActive(true);

        StartTypingPage();
    }

    private void ShowNextPage()
    {
        currentPageIndex++;

        if (currentPageIndex >= dialoguePages.Length)
        {
            CloseDialogue();
            return;
        }

        StartTypingPage();
    }

    private void StartTypingPage()
    {
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        typingCoroutine = StartCoroutine(TypePage(dialoguePages[currentPageIndex]));
    }

    private IEnumerator TypePage(string pageText)
    {
        isTyping = true;
        dialogueText.text = "";

        foreach (char letter in pageText)
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
    }

    private void CompleteCurrentPage()
    {
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        dialogueText.text = dialoguePages[currentPageIndex];
        isTyping = false;
    }

    private void CloseDialogue()
    {
        dialogueOpen = false;
        isTyping = false;
        currentPageIndex = 0;

        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        if (dialoguePanel != null)
            dialoguePanel.SetActive(false);

        if (dialogueText != null)
            dialogueText.text = "";

        if (playerInRange && interactionPrompt != null)
            interactionPrompt.SetActive(true);
    }
}