using UnityEngine;

public class SequencePuzzle : MonoBehaviour
{
    [Header("Orden correcto")]
    [SerializeField] private int[] correctSequence = { 1, 2, 3 };

    [Header("Objetos a controlar")]
    [SerializeField] private MovingPlatform movingPlatform;
    [SerializeField] private GameObject blockingWall;

    private int currentStep = 0;
    private bool puzzleCompleted = false;

    private void Start()
    {
        if (movingPlatform != null)
        {
            movingPlatform.enabled = false;
        }
    }

    public void PressButton(int buttonId)
    {
        if (puzzleCompleted)
        {
            return;
        }

        if (buttonId == correctSequence[currentStep])
        {
            currentStep++;

            if (currentStep >= correctSequence.Length)
            {
                CompletePuzzle();
            }
        }
        else
        {
            ResetPuzzle();
        }
    }

    private void CompletePuzzle()
    {
        puzzleCompleted = true;

        if (movingPlatform != null)
        {
            movingPlatform.enabled = true;
        }

        if (blockingWall != null)
        {
            blockingWall.SetActive(false);
        }

        Debug.Log("Puzzle completado.");
    }

    private void ResetPuzzle()
    {
        currentStep = 0;
        Debug.Log("Secuencia incorrecta. Puzzle reiniciado.");
    }
}