using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class PlayModeCanvasController : MonoBehaviour
{
    [SerializeField] private TMP_Text questionText;
    [SerializeField] private Button resetButton;
    [SerializeField] private Button playButton;

    void Awake()
    {
        resetButton.onClick.AddListener(OnReset);
        playButton.onClick.AddListener(OnPlay);
    }

    public void SetQuestion(string desc)
    {
        questionText.text = desc;
    }

    private void OnReset()
    {
        // reset
    }

    private void OnPlay()
    {
        // OnPlay
    }
}
