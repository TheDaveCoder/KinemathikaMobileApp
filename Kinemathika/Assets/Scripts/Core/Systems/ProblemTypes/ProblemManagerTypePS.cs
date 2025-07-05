using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;
using System.Threading.Tasks;
using UnityEngine.Events;
using Unity.Multiplayer.Center.Common;
using UnityEngine.PlayerLoop;

public class ProblemManagerTypePS : IProblemManager
{
    private readonly string addressablePrefix = "ProblemBundles/";
    private LevelButtonData _levelButtonData;
    private LinkedList<QuestionMetadata> _questions;
    private LinkedListNode<QuestionMetadata> _currentNode;
    private ArUiController _arUiController;
    public event UnityAction<float, float, float> OnPlay;
    public event UnityAction<float, float, float> OnReplay;
    public event UnityAction OnReset;
    private float cachedAnswer;


    public async Task Initialize(LevelButtonData levelButtonData, GameObject loadingScreen)
    {
        _arUiController = GameObject.Find("Canvas").GetComponent<ArUiController>();
        _levelButtonData = levelButtonData;
        // Addressable address
        string addressableAddress = addressablePrefix + _levelButtonData.ProblemId;
        // load addressable SO
        var bundleSO = await Addressables.LoadAssetAsync<ProblemBundle>(addressableAddress).Task;
        // Load the metadata JSON
        var jsonRaw = bundleSO.metadataJson;
        var questionListWrapper = JsonUtility.FromJson<QuestionListWrapper>(jsonRaw.text);
        _questions = new LinkedList<QuestionMetadata>(questionListWrapper.questions);
        _currentNode = _questions.First;

        // Get Problem Description
        string description = AppContext
            .Instance
            .GetService<IConceptStateManager>()
            .GetProblemHeaderMetadata()
            .concepts.Find(concept => concept.id.Equals(_levelButtonData.ConceptId))
            .worksheets.Find(worksheet => worksheet.worksheetId.Equals(_levelButtonData.WorksheetId))
            .problems.Find(problem => problem.problemId == _levelButtonData.ProblemId)
            .description;

        // Initial values
        _arUiController.Initialize(_levelButtonData, bundleSO.scenePackPrefab, description);
        Addressables.Release(bundleSO);

        // Set question
        SetQuestionUi(_currentNode.Value);
        loadingScreen.SetActive(false);
    }

    public void QuestionNodeForward()
    {
        if (_currentNode?.Next != null)
        {
            _currentNode = _currentNode.Next;
            SetQuestionUi(_currentNode.Value);
        }
        else
        {
            // trigger completion
        }
    }
    public QuestionMetadata GetCurrentProblem()
    {
        return _currentNode.Value;
    }

    public void TriggerPlay(float answer, float duration, float timeScale)
    {
        cachedAnswer = answer;
        OnPlay?.Invoke(answer, duration, timeScale);
    }
    public void TriggerReplay(float duration, float timeScale)
    {
        OnReplay?.Invoke(cachedAnswer, duration, timeScale);
    }
    public void TriggerReset()
    {
        OnReset?.Invoke();
    }

    private void SetQuestionUi(QuestionMetadata questionMetadata)
    {
        _arUiController.SetQuestion(questionMetadata);
    }

    // DEVELOPMENT ONLY - DELETE WHEN DONE
    public async Task InitializeDummy(LevelButtonData levelButtonData)
    {
        _arUiController = GameObject.Find("Canvas").GetComponent<ArUiController>();
        _levelButtonData = levelButtonData;
        // Addressable address
        string addressableAddress = addressablePrefix + _levelButtonData.ProblemId;
        // load addressable SO
        var bundleSO = await Addressables.LoadAssetAsync<ProblemBundle>(addressableAddress).Task;
        // Load the metadata JSON
        var jsonRaw = bundleSO.metadataJson;
        var questionListWrapper = JsonUtility.FromJson<QuestionListWrapper>(jsonRaw.text);
        _questions = new LinkedList<QuestionMetadata>(questionListWrapper.questions);
        _currentNode = _questions.First;

        // Get Problem Description
        string description = AppContext
            .Instance
            .GetService<IConceptStateManager>()
            .GetProblemHeaderMetadata()
            .concepts.Find(concept => concept.id.Equals(_levelButtonData.ConceptId))
            .worksheets.Find(worksheet => worksheet.worksheetId.Equals(_levelButtonData.WorksheetId))
            .problems.Find(problem => problem.problemId == _levelButtonData.ProblemId)
            .description;

        // Initial values
        _arUiController.Initialize(_levelButtonData, bundleSO.scenePackPrefab, description);
        Addressables.Release(bundleSO);

        // Set question
        SetQuestionUi(_currentNode.Value);
    }

    public float GetCorrectAnswer()
    {
        return _currentNode.Value.correctAnswer;
    }
    public float GetCachedAnswer()
    {
        return cachedAnswer;
    }
}
