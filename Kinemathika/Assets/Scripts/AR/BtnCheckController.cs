using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Linq;

public class BtnCheckController : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private TMP_Text answerField;
    private IProblemManager _problemManager;

    public void Initialize()
    {
        _problemManager = AppContext.Instance.GetService<IPlayModeManager>().GetCurrentProblem();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Clicked!");
        Debug.Log("Before check");
        Debug.Log("First Integer: " + answerField.text[0]);
        Debug.Log("Last Integer: " + answerField.text[answerField.text.Length - 1]);
        Debug.Log("Langth: " + answerField.text.Length);
        string output = new string(answerField.text.Where(c => char.IsLetter(c) || char.IsDigit(c)).ToArray());

        try
        {
            int answer = Convert.ToInt32(output);
            if (!(answer == 0))
            {
                var problem = _problemManager.GetCurrentProblem();
                _problemManager.TriggerPlay(answer, problem.duration, problem.timeScale);
            }
        }
        catch (Exception ex)
        {
            Debug.Log(ex);
        }
    }
}
