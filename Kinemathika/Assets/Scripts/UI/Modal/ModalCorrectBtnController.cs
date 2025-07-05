using UnityEngine;
using UnityEngine.EventSystems;
public class ModalCorrectBtnController : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] GameObject modal;
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Success Clicked!");
        modal.SetActive(false);
        AppContext.Instance.GetService<IPlayModeManager>().Return(true);
    }
}