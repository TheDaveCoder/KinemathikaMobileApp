using UnityEngine;
using UnityEngine.EventSystems;

public class BtnResetController : MonoBehaviour, IPointerClickHandler
{
    private IProblemManager _problemManager;
    public void Initialize()
    {
        _problemManager = AppContext.Instance.GetService<IPlayModeManager>().GetCurrentProblem();
        Debug.Log(_problemManager);
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        _problemManager.TriggerReset();
    }
}
