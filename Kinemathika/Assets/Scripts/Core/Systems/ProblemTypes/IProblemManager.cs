using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public interface IProblemManager
{
    event UnityAction<float, float, float> OnPlay;
    event UnityAction<float, float, float> OnReplay;
    event UnityAction OnReset;
    Task Initialize(LevelButtonData levelButtonData, GameObject loadingScreen);
    void TriggerPlay(float answer, float duration, float timeScale);
    void TriggerReplay(float duration, float timeScale);
    void TriggerReset();
    void QuestionNodeForward();
    QuestionMetadata GetCurrentProblem();
    // DEVELOPMENT ONLY - REMOVE IN PROD
    Task InitializeDummy(LevelButtonData levelButtonData);

    public float GetCachedAnswer();
    public float GetCorrectAnswer();
}