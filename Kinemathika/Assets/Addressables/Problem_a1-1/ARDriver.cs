using UnityEngine;

public class ARDriver : MonoBehaviour
{
    [SerializeField] private GameObject car;
    [SerializeField] private GameObject person;
    [SerializeField] private GameObject distanceTracker;
    // [SerializeField] private GameObject distanceTracker;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Initialize()
    {
        Debug.Log("Initialized!");
        var currentProblem = AppContext.Instance.GetService<IPlayModeManager>().GetCurrentProblem();
        car.GetComponent<CarController>().Initialize(currentProblem);
        person.GetComponent<PersonController>().Initialize(currentProblem);
        distanceTracker.GetComponent<DistanceTrackerController>().Initialize(currentProblem);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
