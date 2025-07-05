using UnityEngine;
using UnityEngine.EventSystems;
public class BtnOSController : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private GameObject slidingPanel;
    [SerializeField] private bool snapToHide;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void OnPointerClick(PointerEventData eventData)
    {
        if (slidingPanel != null)
        {
            var panelController = slidingPanel.GetComponent<PanelDragController>();
            if (snapToHide)
            {
                panelController.HidePanel();
            }
            else
            {
                panelController.ShowPanel();
            }
        }

    }
}