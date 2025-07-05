using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

[RequireComponent(typeof(RectTransform))]
public class PanelDragController : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private RectTransform panel;
    [SerializeField] private RectTransform handle;
    [SerializeField] private float snapDuration = 0.2f;

    private bool isDragging = false;
    private float panelHeight;
    private Vector2 dragStartPos;
    private float startY;
    private float canvasScaleFactor = 1f;

    private void Awake()
    {
        if (panel == null) panel = GetComponent<RectTransform>();

        Canvas canvas = panel.GetComponentInParent<Canvas>();
        if (canvas != null)
            canvasScaleFactor = canvas.scaleFactor;

        panelHeight = panel.rect.height;
    }


    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!IsPointerOnHandle(eventData)) return;

        isDragging = true;
        dragStartPos = eventData.position;
        startY = panel.anchoredPosition.y;
        StopAllCoroutines();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isDragging) return;

        float deltaY = (eventData.position.y - dragStartPos.y) / canvasScaleFactor;
        float newY = startY + deltaY;

        // Clamp between fully shown (0) and fully hidden (-panelHeight)
        newY = Mathf.Clamp(newY, -panelHeight, 0f);
        panel.anchoredPosition = new Vector2(0f, newY);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!isDragging) return;
        isDragging = false;

        // Snap based on halfway threshold
        float halfway = -panelHeight * 0.25f;
        float currentY = panel.anchoredPosition.y;

        Vector2 target = (currentY > halfway)
            ? new Vector2(0f, 0f)                // Snap open
            : new Vector2(0f, -panelHeight);     // Snap closed

        StartCoroutine(SmoothSnap(target, snapDuration));
    }

    private IEnumerator SmoothSnap(Vector2 target, float duration)
    {
        Vector2 start = panel.anchoredPosition;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            panel.anchoredPosition = Vector2.Lerp(start, target, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        panel.anchoredPosition = target;
    }

    private bool IsPointerOnHandle(PointerEventData eventData)
    {
        if (handle == null) return true;

        return RectTransformUtility.RectangleContainsScreenPoint(
            handle, eventData.position, eventData.enterEventCamera);
    }

    public void ShowPanel()
    {
        StopAllCoroutines();
        StartCoroutine(SmoothSnap(new Vector2(0f, 0f), snapDuration)); // Snap to open
    }

    public void HidePanel()
    {
        StopAllCoroutines();
        StartCoroutine(SmoothSnap(new Vector2(0f, -panelHeight), snapDuration)); // Snap closed
    }

}
