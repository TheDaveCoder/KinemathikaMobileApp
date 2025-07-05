using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonSceneLoader : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        var data = GetComponent<LevelButtonData>();
        if (data == null)
        {
            Debug.LogError("IProblemButtonData not found.");
            return;
        }
        AppContext.Instance.GetService<IPlayModeManager>().Load(data);
    }
}

