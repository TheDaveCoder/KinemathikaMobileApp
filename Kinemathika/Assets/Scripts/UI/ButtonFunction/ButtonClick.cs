using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ButtonClick : MonoBehaviour
{
    [Header("Destination UI Canvas")]
    [SerializeField] private GameObject targetScreen;
    [Header("Additional Functionality")]
    [SerializeField] private UnityEvent specialProcedure;
    private Button _button;
    private void Awake()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(HandleClick);
    }

    private void HandleClick()
    {
        specialProcedure?.Invoke();

        if (targetScreen == null)
        {
            Debug.LogWarning($"No target screen set on {gameObject.name}");
            return;
        }

        Canvas rootCanvas = GetComponentInParent<Canvas>().rootCanvas;
        rootCanvas.gameObject.SetActive(false);

        // Show target screen
        targetScreen.SetActive(true);
    }
}
