using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class PersonController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    private Vector3 _initialPosition;
    private Quaternion _initialRotation;
    private Coroutine _playRoutine;
    private IProblemManager _problemManager;

    private float scaleRatio = 26f / 120f;

    public void Initialize(IProblemManager problemManager)
    {
        _problemManager = problemManager;
        _problemManager.OnPlay += HandleOnPlay;
        _problemManager.OnReplay += HandleOnReplay;
        _problemManager.OnReset += HandleOnReset;
        _initialPosition = transform.position;
        _initialRotation = transform.rotation;

        if (animator == null)
            animator = GetComponentInChildren<Animator>();
    }

    private void HandleOnPlay(float answer, float duration, float multiplier)
    {
        float simulatedDistance = 10.24f;
        float simulatedDuration = 3f;

        if (_playRoutine != null)
        {
            StopCoroutine(_playRoutine);
            HandleOnReset();
        }

        animator?.SetBool("IsWalking", true);
        _playRoutine = StartCoroutine(MoveForDuration(simulatedDuration, simulatedDistance));
    }

    private void HandleOnReplay(float cachedAnswer, float duration, float timeScale)
    {
        float simulatedDistance = 10.24f;
        float simulatedDuration = 3f;

        if (_playRoutine != null)
        {
            StopCoroutine(_playRoutine);
            HandleOnReset();
        }

        animator?.SetBool("IsWalking", true);
        _playRoutine = StartCoroutine(MoveForDuration(simulatedDuration, simulatedDistance));
    }

    private void HandleOnReset()
    {
        if (_playRoutine != null)
            StopCoroutine(_playRoutine);
        var thisRB = GetComponent<Rigidbody>();
        thisRB.linearVelocity = Vector3.zero;
        thisRB.angularVelocity = Vector3.zero;
        transform.position = _initialPosition;
        transform.rotation = _initialRotation;
        animator?.SetBool("IsWalking", false);
        animator?.SetBool("IsShocked", false);
    }

    private IEnumerator MoveForDuration(float simulatedDuration, float simulatedDistance)
    {
        float elapsed = 0f;
        float speed = simulatedDistance / simulatedDuration;
        Debug.Log("Duration" + simulatedDuration);

        while (elapsed < simulatedDuration)
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
            elapsed += Time.deltaTime;
            Debug.Log("Elapsed" + elapsed);
            yield return null;
        }
        animator?.SetBool("IsWalking", false);
        animator?.SetBool("IsShocked", true);
        yield return new WaitForSeconds(0.1f); // Short delay
        animator?.SetBool("IsShocked", false);
        Debug.Log("Coroutine Over!");
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
