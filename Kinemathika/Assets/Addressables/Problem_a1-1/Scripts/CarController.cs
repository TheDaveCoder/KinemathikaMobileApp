using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class CarController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject personGO;
    private Vector3 _initialPosition;
    private Quaternion _initialRotation;
    private float _slamPower = 30;
    private Coroutine _playRoutine;
    private IProblemManager _problemManager;

    private float scaleRatio = 26f / 120f;

    public void Initialize(IProblemManager problemManager)
    {
        Debug.Log("Car Initialized!");
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

        animator?.SetBool("IsDriving", true);
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

        animator?.SetBool("IsDriving", true);
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
        animator?.SetBool("IsDriving", false);
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

        Debug.Log("Coroutine Over!");
        animator?.SetBool("IsDriving", false);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (personGO)
        {
            Rigidbody personRb = collision.gameObject.GetComponent<Rigidbody>();
            if (personRb != null)
            {
                Vector3 slamDirection = collision.contacts[0].normal * -1f + Vector3.up * 1f; // Forward + upward
                personRb.AddForce(slamDirection.normalized * _slamPower, ForceMode.Impulse);
                personRb.AddTorque(Random.onUnitSphere * _slamPower, ForceMode.Impulse);
            }
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
