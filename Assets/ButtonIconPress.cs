using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(RectTransform))]
public class ButtonIconPress : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private RectTransform icon;           
    [SerializeField] private Vector2 pressOffset = new Vector2(2f, -3f);

    private Vector2 originalPos;

    private void Awake()
    {
        if (icon != null)
            originalPos = icon.anchoredPosition;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (icon == null) return;
        icon.anchoredPosition = originalPos + pressOffset;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (icon == null) return;
        icon.anchoredPosition = originalPos;
    }
}