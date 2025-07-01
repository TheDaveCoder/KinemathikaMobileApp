using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UserSetupController : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TMP_InputField nameInputField;
    [SerializeField] private Button submitButton;

    // Invoked when user completes setup with valid input
    public event Action<string> OnComplete;

    private void Awake()
    {
        submitButton.interactable = false;

        nameInputField.onValueChanged.AddListener(OnNameChanged);
        submitButton.onClick.AddListener(OnSubmitClicked);
    }

    private void OnDestroy()
    {
        nameInputField.onValueChanged.RemoveListener(OnNameChanged);
        submitButton.onClick.RemoveListener(OnSubmitClicked);
    }

    private void OnNameChanged(string input)
    {
        submitButton.interactable = !string.IsNullOrWhiteSpace(input);
    }

    private void OnSubmitClicked()
    {
        Debug.Log("Observer Fired!");
        string name = nameInputField.text.Trim();

        if (string.IsNullOrEmpty(name))
        {
            Debug.LogWarning("Name input is empty");
            return;
        }

        OnComplete?.Invoke(name);
    }
}
