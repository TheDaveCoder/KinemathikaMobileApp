using UnityEngine;
using System.Collections;

public class PanelController : MonoBehaviour
{
    public RectTransform panel;
    public float duration = 0.3f;

    private Vector2 originalOffsetMin;
    private Vector2 originalOffsetMax;
    private float panelHeight;

    private void Awake()
    {
        if (panel == null)
            panel = GetComponent<RectTransform>();

        // Cache starting offsets and height
        originalOffsetMin = panel.offsetMin;
        originalOffsetMax = panel.offsetMax;
        panelHeight = panel.rect.height;
    }

    public void Show()
    {
        StopAllCoroutines();
        StartCoroutine(SlideTo(originalOffsetMin, originalOffsetMax));
    }

    public void Hide()
    {
        StopAllCoroutines();

        // Slide the panel down by its full height
        Vector2 targetMin = originalOffsetMin + new Vector2(0, -panelHeight);
        Vector2 targetMax = originalOffsetMax + new Vector2(0, -panelHeight);
        StartCoroutine(SlideTo(targetMin, targetMax));
    }

    private IEnumerator SlideTo(Vector2 targetMin, Vector2 targetMax)
    {
        Vector2 startMin = panel.offsetMin;
        Vector2 startMax = panel.offsetMax;

        float elapsed = 0f;
        while (elapsed < duration)
        {
            float t = elapsed / duration;
            panel.offsetMin = Vector2.Lerp(startMin, targetMin, t);
            panel.offsetMax = Vector2.Lerp(startMax, targetMax, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        panel.offsetMin = targetMin;
        panel.offsetMax = targetMax;
    }
}
