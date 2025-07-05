using UnityEngine;
using UnityEngine.EventSystems;

public class BtnReplayController : MonoBehaviour, IPointerClickHandler
{
    private IProblemManager _problemManager;
    public void Initialize()
    {
        _problemManager = AppContext.Instance.GetService<IPlayModeManager>().GetCurrentProblem();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        var duration = _problemManager.GetCurrentProblem().duration;
        var timeScale = _problemManager.GetCurrentProblem().timeScale;
        _problemManager.TriggerReplay(duration, timeScale);
    }
}
