using UnityEngine;
using UnityEngine.EventSystems;
public class ModalWrongBtnController : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] GameObject modal;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void OnPointerClick(PointerEventData eventData)
    {
        modal.SetActive(false);
    }
}
