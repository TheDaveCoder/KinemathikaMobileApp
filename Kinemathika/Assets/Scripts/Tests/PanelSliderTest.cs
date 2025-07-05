using UnityEngine;
using UnityEngine.EventSystems;

public class SlidePanelTest : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private GameObject panelContainer;
    private PanelController panel;

    private bool isShown = true;

    public void Awake()
    {
        panel = panelContainer.GetComponent<PanelController>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (panel == null) return;

        if (isShown)
            panel.Hide();
        else
            panel.Show();

        isShown = !isShown;
    }
}
