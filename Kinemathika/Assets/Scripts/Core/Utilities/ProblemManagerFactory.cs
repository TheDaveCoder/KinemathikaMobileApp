using UnityEngine;

public static class ProblemManagerFactory
{
    public static IProblemManager Create(LevelButtonData levelButtonData)
    {
        // switch or mapping
        var metadata = AppContext.Instance.GetService<IConceptStateManager>().GetProblemHeaderMetadata();
        var concept = metadata.concepts.Find(concept => concept.id.Equals(levelButtonData.ConceptId));
        var worksheet = concept.worksheets.Find(worksheet => worksheet.worksheetId.Equals(levelButtonData.WorksheetId));
        var currentProblem = worksheet.problems.Find(problem => problem.problemId.Equals(levelButtonData.ProblemId));

        switch (currentProblem.problemType)
        {
            case 1:
                return new ProblemManagerTypePS();
            case 2:
            // return new ProblemManagerTypeVA();
            default:
                return null;
        }
    }
}