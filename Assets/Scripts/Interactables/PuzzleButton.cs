using UnityEngine;

public class PuzzleButton : MonoBehaviour
{
    [SerializeField] private int buttonId;
    [SerializeField] private SequencePuzzle puzzleManager;

    [Header("Sprites")]
    [SerializeField] private Sprite normalSprite;
    [SerializeField] private Sprite pressedSprite;

    private SpriteRenderer spriteRenderer;
    private bool playerInside = false;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (normalSprite != null)
        {
            spriteRenderer.sprite = normalSprite;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }

        if (playerInside)
        {
            return;
        }

        playerInside = true;

        if (pressedSprite != null)
        {
            spriteRenderer.sprite = pressedSprite;
        }

        if (puzzleManager != null)
        {
            puzzleManager.PressButton(buttonId);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }

        playerInside = false;

        if (normalSprite != null)
        {
            spriteRenderer.sprite = normalSprite;
        }
    }
}