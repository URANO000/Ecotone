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

    [SerializeField] private Image[] symbolGlows;
    [SerializeField] private Image[] buttonIcons;
    [SerializeField] private RectTransform panelRoot;
    [SerializeField] private Color correctFlashColor = new Color(0.4f, 1f, 0.5f);
    [SerializeField] private Color wrongFlashColor = new Color(1f, 0.35f, 0.35f);

    [SerializeField] private UnityEngine.Events.UnityEvent<int> onIconLit;
    [SerializeField] private UnityEngine.Events.UnityEvent onButtonClicked;
    [SerializeField] private UnityEngine.Events.UnityEvent onCorrectPress;
    [SerializeField] private UnityEngine.Events.UnityEvent onWrongPress;
    [SerializeField] private UnityEngine.Events.UnityEvent onSuccess;
    [SerializeField] private UnityEngine.Events.UnityEvent onFail;

    [Header("Eventos")]
    [SerializeField] private GameObject successPanel;
    [SerializeField] private GameObject failPanel;
    [SerializeField] private ObstacleController targetObstacle;
    [SerializeField] private CameraFocusController cameraController;
    [SerializeField] private GameObject puzzleCanvas;

    [Header("Idle pulse (símbolos de arriba en reposo)")]
    [SerializeField] private bool useIdlePulse = true;
    [SerializeField] private float idlePulseMinAlpha = 0.12f;
    [SerializeField] private float idlePulseMaxAlpha = 0.28f;
    [SerializeField] private float idlePulseDuration = 2f;

    private List<int> sequence = new List<int>();
    private int playerStep = 0;
    private bool acceptingInput = false;
    private bool isShowingSequence = false;
    private Coroutine[] idlePulseRoutines;

    private void OnEnable()
    {
        GenerateSequence();
        StartCoroutine(OpenThenShowRoutine());

        if (useIdlePulse && symbolGlows != null)
        {
            idlePulseRoutines = new Coroutine[symbolGlows.Length];
            for (int i = 0; i < symbolGlows.Length; i++)
                idlePulseRoutines[i] = StartCoroutine(IdlePulseRoutine(i));
        }
    }

    private void OnDisable()
    {
        if (idlePulseRoutines != null)
        {
            foreach (var r in idlePulseRoutines)
                if (r != null) StopCoroutine(r);
        }
    }
    private IEnumerator IdlePulseRoutine(int index)
    {
        if (symbolGlows == null || symbolGlows.Length <= index || symbolGlows[index] == null)
            yield break;

        Image glow = symbolGlows[index];

        while (true)
        {
            if (isShowingSequence)
            {
                yield return null;
                continue;
            }

            float t = 0f;
            while (t < idlePulseDuration && !isShowingSequence)
            {
                t += Time.unscaledDeltaTime;
                float p = Mathf.PingPong(t / (idlePulseDuration * 0.5f), 1f);
                float a = Mathf.Lerp(idlePulseMinAlpha, idlePulseMaxAlpha, p);
                Color c = glow.color;
                c.a = a;
                glow.color = c;
                yield return null;
            }
        }
    }

    private void GenerateSequence()
    {
        sequence.Clear();
        playerStep = 0;
        for (int i = 0; i < sequenceLength; i++)
            sequence.Add(Random.Range(0, symbolIcons.Length));
    }

    private IEnumerator OpenThenShowRoutine()
    {
        if (panelRoot != null)
        {
            panelRoot.localScale = Vector3.zero;
            float t = 0f;
            float openDuration = 0.25f;
            while (t < openDuration)
            {
                t += Time.unscaledDeltaTime;
                float p = t / openDuration;
                panelRoot.localScale = Vector3.one * Mathf.SmoothStep(0f, 1.05f, p);
                yield return null;
            }
            panelRoot.localScale = Vector3.one;
        }

        yield return StartCoroutine(ShowSequenceRoutine());
    }

    private IEnumerator ShowSequenceRoutine()
    {
        acceptingInput = false;
        isShowingSequence = true;
        yield return new WaitForSecondsRealtime(0.5f);

        foreach (int index in sequence)
        {
            onIconLit?.Invoke(index);
            yield return StartCoroutine(LightUpSymbol(index));
            yield return new WaitForSecondsRealtime(0.2f);
        }

        isShowingSequence = false;
        acceptingInput = true;
    }

    private IEnumerator LightUpSymbol(int index)
    {
        Image icon = symbolIcons[index];
        Image glow = (symbolGlows != null && symbolGlows.Length > index) ? symbolGlows[index] : null;
        RectTransform rt = icon.rectTransform;

        float half = showDelay * 0.5f;
        float t = 0f;

        while (t < half)
        {
            t += Time.unscaledDeltaTime;
            float p = t / half;
            icon.color = Color.Lerp(idleColor, highlightColor, p);
            rt.localScale = Vector3.one * Mathf.Lerp(1f, 1.15f, p);
            if (glow != null)
            {
                Color c = glow.color;
                c.a = Mathf.Lerp(0f, 0.8f, p);
                glow.color = c;
            }
            yield return null;
        }
        t = 0f;
        while (t < half)
        {
            t += Time.unscaledDeltaTime;
            float p = t / half;
            icon.color = Color.Lerp(highlightColor, idleColor, p);
            rt.localScale = Vector3.one * Mathf.Lerp(1.15f, 1f, p);
            if (glow != null)
            {
                Color c = glow.color;
                c.a = Mathf.Lerp(0.8f, 0f, p);
                glow.color = c;
            }
            yield return null;
        }
    }

    public void OnSymbolPressed(int index)
    {
        if (!acceptingInput) return;
        onButtonClicked?.Invoke();

        if (index == sequence[playerStep])
        {
            onCorrectPress?.Invoke();
            StartCoroutine(FlashButton(index, correctFlashColor));
            playerStep++;

            if (playerStep >= sequence.Count)
            {
                acceptingInput = false;
                StartCoroutine(SuccessRoutine());
            }
        }
        else
        {
            onWrongPress?.Invoke();
            acceptingInput = false;
            StartCoroutine(FlashButton(index, wrongFlashColor));
            StartCoroutine(FailRoutine());
        }
    }
    private IEnumerator FlashButton(int index, Color flashColor)
    {
        if (buttonIcons == null || buttonIcons.Length <= index) yield break;

        Image btnIcon = buttonIcons[index];
        RectTransform rt = btnIcon.rectTransform;
        Color original = btnIcon.color;
        btnIcon.color = flashColor;

        float t = 0f;
        float duration = 0.18f;
        while (t < duration)
        {
            t += Time.unscaledDeltaTime;
            float p = t / duration;
            rt.localScale = Vector3.one * (1f + Mathf.Sin(p * Mathf.PI) * 0.2f);
            yield return null;
        }
        rt.localScale = Vector3.one;
        btnIcon.color = original;
    }
    private IEnumerator ShakePanel()
    {
        if (panelRoot == null) yield break;

        Vector3 originalPos = panelRoot.localPosition;
        float duration = 0.3f;
        float strength = 8f;
        float t = 0f;
        while (t < duration)
        {
            t += Time.unscaledDeltaTime;
            float offsetX = Random.Range(-1f, 1f) * strength * (1f - t / duration);
            panelRoot.localPosition = originalPos + new Vector3(offsetX, 0f, 0f);
            yield return null;
        }
        panelRoot.localPosition = originalPos;
    }

    private IEnumerator SuccessRoutine()
    {
        onSuccess?.Invoke();
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
        onFail?.Invoke();
        yield return StartCoroutine(ShakePanel());
        failPanel.SetActive(true);
        yield return new WaitForSecondsRealtime(1f);
        failPanel.SetActive(false);
        GenerateSequence();
        StartCoroutine(ShowSequenceRoutine());
    }
}