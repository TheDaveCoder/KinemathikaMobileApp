using System.Collections;
using TMPro;
using UnityEngine;

public class TimerController : MonoBehaviour
{
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private TMP_Text subText;
    [SerializeField] private GameObject arUiController;

    private Coroutine _countdownRoutine;
    private IProblemManager _problemManager;

    public void Initialize(IProblemManager problemManager)
    {
        _problemManager = problemManager;

        _problemManager.OnPlay += HandleOnPlay;
        _problemManager.OnReplay += HandleOnReplay;
        _problemManager.OnReset += HandleOnReset;

        UpdateSubtext();
        ResetTimerDisplay();
    }

    private void UpdateSubtext()
    {
        subText.text = $"{_problemManager.GetCurrentProblem().timeScale}x speed in real time";
    }

    private void HandleOnPlay(float answer, float duration, float timeScale)
    {
        RestartTimer(duration, timeScale, true);
    }

    private void HandleOnReplay(float cachedAnswer, float duration, float timeScale)
    {
        RestartTimer(duration, timeScale, false);
    }

    private void HandleOnReset()
    {
        if (_countdownRoutine != null)
            StopCoroutine(_countdownRoutine);

        ResetTimerDisplay();
    }

    private void RestartTimer(float duration, float timeScale, bool isPlay)
    {
        if (_countdownRoutine != null)
            StopCoroutine(_countdownRoutine);

        _countdownRoutine = StartCoroutine(CountdownTimer(duration, timeScale, isPlay));
    }

    private IEnumerator CountdownTimer(float realDuration, float timeScale, bool isPlay)
    {
        float simulatedDuration = realDuration / timeScale;
        float elapsed = 0f;

        while (elapsed < simulatedDuration)
        {
            elapsed += Time.deltaTime;

            float remainingRatio = Mathf.Clamp01(1f - (elapsed / simulatedDuration));
            float timeLeft = remainingRatio * realDuration;

            timerText.text = timeLeft.ToString("F2") + "s";
            yield return null;
        }

        timerText.text = "0.00s";
        Debug.Log("Countdown Complete!");

        if (isPlay)
        {
            Debug.Log("Firing!");
            if (_problemManager.GetCorrectAnswer() == _problemManager.GetCachedAnswer())
            {
                Debug.Log("Answer Correct!");
                arUiController.GetComponent<ArUiController>().OpenPanel(1);
            }
            else
            {
                Debug.Log("Answer Incorrect!");
                arUiController.GetComponent<ArUiController>().OpenPanel(2);
            }
        }
    }

    private void ResetTimerDisplay()
    {
        timerText.text = "0.00s";
    }

    private void OnDisable()
    {
        if (_problemManager == null) return;

        _problemManager.OnPlay -= HandleOnPlay;
        _problemManager.OnReplay -= HandleOnReplay;
        _problemManager.OnReset -= HandleOnReset;
    }
}
