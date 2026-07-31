using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    [SerializeField] private GameObject promptUI;
    [SerializeField] private GameObject puzzleCanvas;
    [SerializeField] private CameraFocusController cameraController;

    private bool playerInRange = false;
    private bool puzzleOpened = false; 

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        playerInRange = true;
        promptUI.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        playerInRange = false;
        promptUI.SetActive(false);

        if (!puzzleOpened)
        {
            cameraController.ReturnToPlayer();
        }
    }

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            OpenPuzzle();
        }
    }

    private void OpenPuzzle()
    {
        promptUI.SetActive(false);
        puzzleCanvas.SetActive(true);
        puzzleOpened = true; 
        Time.timeScale = 0f;
    }
}