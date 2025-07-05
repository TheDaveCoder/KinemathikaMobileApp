using UnityEngine;
using UnityEngine.EventSystems;
public class ButtonReturnFromAr : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        AppContext.Instance.GetService<IPlayModeManager>().Return(false);
    }
}