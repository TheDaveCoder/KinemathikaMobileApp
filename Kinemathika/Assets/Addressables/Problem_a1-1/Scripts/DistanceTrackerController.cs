using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class DistanceTrackerController : MonoBehaviour
{
    [SerializeField] private TMP_Text textGO;
    private Coroutine _playRoutine;
    private IProblemManager _problemManager;
    private float scaleRatio = 26f / 120f;
    private float _trackedDistanceMeters;
    private Vector3 _initialPosition;
    private Quaternion _initialRotation;


    public void Initialize(IProblemManager problemManager)
    {
        Debug.Log("Distance Initialized!");
        _problemManager = problemManager;
        _problemManager.OnPlay += HandleOnPlay;
        _problemManager.OnReplay += HandleOnReplay;
        _problemManager.OnReset += HandleOnReset;
        _initialPosition = transform.position;
        _initialRotation = transform.rotation;
        _trackedDistanceMeters = 0f;
    }

    private void HandleOnPlay(float answer, float duration, float multiplier)
    {
        Debug.Log("Car Triggered!");
        float finalModifier = 1 / multiplier;
        float realDistance = answer * duration;
        float simulatedDistance = realDistance * scaleRatio;
        float simulatedDuration = duration * finalModifier;

        if (_playRoutine != null)
        {
            StopCoroutine(_playRoutine);
            HandleOnReset();
        }


        _playRoutine = StartCoroutine(MoveForDuration(simulatedDuration, simulatedDistance));
    }

    private void HandleOnReplay(float cachedAnswer, float duration, float timeScale)
    {
        float finalModifier = 1 / timeScale;
        float realDistance = cachedAnswer * duration;
        float simulatedDistance = realDistance * scaleRatio;
        float simulatedDuration = duration * finalModifier;

        if (_playRoutine != null)
        {
            StopCoroutine(_playRoutine);
            HandleOnReset();
        }

        _playRoutine = StartCoroutine(MoveForDuration(simulatedDuration, simulatedDistance));
    }

    private void HandleOnReset()
    {
        if (_playRoutine != null)
            StopCoroutine(_playRoutine);
        _trackedDistanceMeters = 0f;
        transform.position = _initialPosition;
        transform.rotation = _initialRotation;
        textGO.text = "0.00 m"; // Reset label visually
    }

    private IEnumerator MoveForDuration(float simulatedDuration, float simulatedDistance)
    {
        float elapsed = 0f;
        float speed = simulatedDistance / simulatedDuration;
        float realDistance = simulatedDistance / scaleRatio;

        while (elapsed < simulatedDuration)
        {
            // transform.Translate(Vector3.forward * speed * Time.deltaTime);
            elapsed += Time.deltaTime;

            float progressRatio = Mathf.Clamp01(elapsed / simulatedDuration);
            float currentRealDistance = progressRatio * realDistance;
            textGO.text = currentRealDistance.ToString("F2") + " m";

            yield return null;
        }
    }


    private void OnDisable()
    {
        if (_problemManager != null)
        {
            _problemManager.OnPlay -= HandleOnPlay;
            _problemManager.OnReplay -= HandleOnReplay;
            _problemManager.OnReset -= HandleOnReset;
        }
    }
}
