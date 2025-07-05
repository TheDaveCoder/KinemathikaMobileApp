using UnityEngine;

public class Billboad : MonoBehaviour
{
    [SerializeField] GameObject car;
    void Update()
    {
        if (Camera.main != null)
        {
            Vector3 cameraForward = Camera.main.transform.forward;
            transform.rotation = Quaternion.LookRotation(cameraForward);
            // Vector3 offset = new Vector3(1f, 2.75f, -1.75f);
            // transform.position = car.transform.position + offset;
        }
    }
}
