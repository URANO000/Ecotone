using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ForestSealPuzzle : MonoBehaviour
{
    [Header("Configuración del ritual")]
    [SerializeField] private int sequenceLength = 4;
    [SerializeField] private Image[] symbolIcons;
    [SerializeField] private Button[] symbolButtons;
    [SerializeField] private float showDelay = 0.7f;
    [SerializeField] private Color highlightColor = Color.white;
    [SerializeField] private Color idleColor = new Color(1, 1, 1, 0.4f);

    [Header("Eventos")]
    [SerializeField] private GameObject successPanel;
    [SerializeField] private GameObject failPanel;
    [SerializeField] private ObstacleController targetObstacle;
    [SerializeField] private CameraFocusController cameraController;
    [SerializeField] private GameObject puzzleCanvas;

    private List<int> sequence = new List<int>();
    private int playerStep = 0;
    private bool acceptingInput = false;

    private void OnEnable()
    {
        GenerateSequence();
        StartCoroutine(ShowSequenceRoutine());
    }

    private void GenerateSequence()
    {
        sequence.Clear();
        playerStep = 0;
        for (int i = 0; i < sequenceLength; i++)
            sequence.Add(Random.Range(0, symbolIcons.Length));
    }

    private IEnumerator ShowSequenceRoutine()
    {
        acceptingInput = false;
        yield return new WaitForSecondsRealtime(0.5f);

        foreach (int index in sequence)
        {
            symbolIcons[index].color = highlightColor;
            yield return new WaitForSecondsRealtime(showDelay);
            symbolIcons[index].color = idleColor;
            yield return new WaitForSecondsRealtime(0.2f);
        }

        acceptingInput = true;
    }

    public void OnSymbolPressed(int index)
    {
        if (!acceptingInput) return;

        if (index == sequence[playerStep])
        {
            playerStep++;
            if (playerStep >= sequence.Count)
            {
                acceptingInput = false;
                StartCoroutine(SuccessRoutine());
            }
        }
        else
        {
            acceptingInput = false;
            StartCoroutine(FailRoutine());
        }
    }

    private IEnumerator SuccessRoutine()
    {
        successPanel.SetActive(true);
        yield return new WaitForSecondsRealtime(1.2f);
        successPanel.SetActive(false);

        puzzleCanvas.SetActive(false);
        Time.timeScale = 1f;

        cameraController.FocusOnTarget(targetObstacle.transform, onComplete: () =>
        {
            targetObstacle.Unlock();
            cameraController.ReturnToPlayer();
        });
    }

    private IEnumerator FailRoutine()
    {
        failPanel.SetActive(true);
        yield return new WaitForSecondsRealtime(1f);
        failPanel.SetActive(false);
        GenerateSequence();
        StartCoroutine(ShowSequenceRoutine());
    }
}