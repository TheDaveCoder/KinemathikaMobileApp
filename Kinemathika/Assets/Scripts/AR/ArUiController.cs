using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class ArUiController : MonoBehaviour
{
    // Unity serializables
    [Header("Editable References")]
    [SerializeField] private TMP_Text problemDescription;
    [SerializeField] private TMP_Text questionDescription;
    [Header("Unity Modals")]
    [SerializeField] private GameObject successModal;
    [SerializeField] private GameObject errorModal;
    [Header("Button References")]
    [SerializeField] private GameObject buttonCheckMain;
    [SerializeField] private GameObject buttonCheckSub;
    [SerializeField] private GameObject buttonReset;
    [SerializeField] private GameObject timerControl;
    private GameObject _scenePack;

    // public void Initialize(LevelButtonData levelButtonData, GameObject prefab, string description)
    // {
    //     // Instantiate scene pack
    //     _scenePack = Instantiate(prefab);
    //     problemDescription.text = description;

    //     buttonCheckMain.GetComponent<BtnCheckController>().Initialize();
    //     buttonCheckSub.GetComponent<BtnReplayController>().Initialize();
    //     buttonReset.GetComponent<BtnResetController>().Initialize();
    //     var temp = AppContext.Instance.GetService<IPlayModeManager>().GetCurrentProblem();
    //     timerControl.GetComponent<TimerController>().Initialize(temp);
    //     _scenePack.GetComponent<ARDriver>().Initialize();
    // }

    public void Initialize(LevelButtonData levelButtonData, GameObject prefab, string description)
    {
        // Instantiate scene pack
        _scenePack = Instantiate(prefab);

        // Position it in front of the AR camera
        Transform cam = Camera.main.transform;
        _scenePack.transform.position = cam.position + cam.forward * 1.0f;
        _scenePack.transform.rotation = Quaternion.LookRotation(-cam.forward);

        problemDescription.text = description;

        buttonCheckMain.GetComponent<BtnCheckController>().Initialize();
        buttonCheckSub.GetComponent<BtnReplayController>().Initialize();
        buttonReset.GetComponent<BtnResetController>().Initialize();

        var temp = AppContext.Instance.GetService<IPlayModeManager>().GetCurrentProblem();
        timerControl.GetComponent<TimerController>().Initialize(temp);
        _scenePack.GetComponent<ARDriver>().Initialize();
    }


    public void SetQuestion(QuestionMetadata questionMetadata)
    {
        questionDescription.text = questionMetadata.description;
    }

    public void OpenPanel(int id)
    {
        if (id == 1)
        {
            successModal.SetActive(true);
        }
        else
        {
            errorModal.SetActive(true);
        }
    }
}
