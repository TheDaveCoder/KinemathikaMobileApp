using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System;

public class HomeScreenController : MonoBehaviour
{
    [Header("Scene References")]
    [SerializeField] private Transform worksheetListContent;
    [SerializeField] private GameObject worksheetPackPrefab;
    [SerializeField] private GameObject buttonLockedPrefab;
    [SerializeField] private GameObject buttonInProgressPrefab;
    [SerializeField] private GameObject buttonCompletedPrefab;
    [SerializeField] private GameObject dividerPrefab;

    private AppContext _context;
    private ProblemHeaderMetadata _metadata;
    private UserData _userData;

    public void Initialize()
    {
        _context = AppContext.Instance;
        _userData = _context.GetService<IUserSessionManager>().GetSession();
        _metadata = _context.GetService<IConceptStateManager>().GetProblemHeaderMetadata();
        InsertButtons();
    }

    private void InsertButtons()
    {
        foreach (var concept in _metadata.concepts)
        {
            try
            {
                var conceptProgress = _userData.progress.Find(cp => cp.conceptId.Equals(concept.id));
                if (conceptProgress is null)
                    continue;

                foreach (var worksheet in concept.worksheets)
                {
                    var worksheetProgress = conceptProgress.worksheets
                        .Find(wp => wp.worksheetId == worksheet.worksheetId);

                    // Instantiate worksheet pack
                    var worksheetGO = Instantiate(worksheetPackPrefab, worksheetListContent);

                    // Set worksheet name
                    var nameText = worksheetGO
                        .transform
                        .Find("WorksheetHeader/WorksheetName")
                        .GetComponent<TMP_Text>();
                    nameText.text = "Worksheet#" + worksheet.worksheetId;

                    // Get level list container
                    var levelListContainer = worksheetGO.transform.Find("level_list");

                    // Spawn buttons for each level
                    int worksheetIndex = 0;
                    foreach (var level in worksheet.problems)
                    {
                        string status = "locked";
                        if (worksheetProgress != null)
                        {
                            if (level.problemId.Equals(_userData.latestProblemId))
                            {
                                status = "in_progress";
                            }
                            else if (worksheetProgress.completedProblems.Contains(level.problemId))
                                status = "completed";
                        }

                        if (worksheetIndex != 0) Instantiate(dividerPrefab, levelListContainer);
                        GameObject prefab = GetButtonPrefabByStatus(status);
                        var buttonGO = Instantiate(prefab, levelListContainer);

                        var buttonData = buttonGO.GetComponent<LevelButtonData>();
                        buttonData.ProblemId = level.problemId;
                        buttonData.WorksheetId = worksheet.worksheetId;
                        buttonData.ConceptId = concept.id;
                        buttonData.ProblemType = level.problemType;
                        worksheetIndex++;
                    }
                }
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }

        }
    }


    private GameObject GetButtonPrefabByStatus(string status)
    {
        switch (status)
        {
            case "locked":
                return buttonLockedPrefab;
            case "in_progress":
                return buttonInProgressPrefab;
            case "completed":
                return buttonCompletedPrefab;
            default:
                return buttonLockedPrefab;
        }
    }
}
