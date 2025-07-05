using UnityEngine;

public class DummyInit : MonoBehaviour
{
    [SerializeField] private string ConceptId;
    [SerializeField] private string WorksheetId;
    [SerializeField] private string ProblemId;

    private LevelButtonData _levelButtonData;

    void Start()
    {
        _levelButtonData = GetComponent<LevelButtonData>();
        _levelButtonData.ConceptId = ConceptId;
        _levelButtonData.WorksheetId = WorksheetId;
        _levelButtonData.ProblemId = ProblemId;
    }

    public LevelButtonData GetDummyValues()
    {
        return _levelButtonData;
    }
}
