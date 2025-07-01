using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public static class UIUtility
{
    public static IEnumerator Rebuild(GameObject go)
    {
        yield return null;
        Canvas.ForceUpdateCanvases();
        LayoutRebuilder.ForceRebuildLayoutImmediate(go.GetComponent<RectTransform>());
    }
}
