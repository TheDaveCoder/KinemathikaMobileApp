using System.Threading.Tasks;
using UnityEngine;
using System.Collections.Generic;

public class LocalSessionManager : IUserSessionManager
{
    private const string FileName = "user_session.json";
    private const string standardStartProblemID = "a1-1";
    private const string starndardStartConceptId = "a";
    private UserData _user;

    public async Task<bool> CheckSessionAsync()
    {
        if (!JsonFileService.FileExists(FileName))
            return false;

        _user = await JsonFileService.LoadAsync<UserData>(FileName);

        if (_user == null || string.IsNullOrEmpty(_user.username))
        {
            Debug.LogWarning("Failed to parse session or username missing.");
            return false;
        }

        return true;
    }

    public async Task CreateSessionAsync(string username, ProblemHeaderMetadata problemMetadata)
    {
        _user = new UserData
        {
            username = username,
            latestConcept = starndardStartConceptId,
            latestProblemId = standardStartProblemID,
            progress = new List<ConceptProgress>()
        };

        foreach (var concept in problemMetadata.concepts)
        {
            var conceptProgress = new ConceptProgress
            {
                conceptId = concept.id,
                worksheets = new List<WorksheetProgress>()
            };

            foreach (var worksheet in concept.worksheets)
            {
                var worksheetProgress = new WorksheetProgress
                {
                    worksheetId = worksheet.worksheetId,
                    completedProblems = new List<string>()
                };

                conceptProgress.worksheets.Add(worksheetProgress);
            }

            _user.progress.Add(conceptProgress);
        }

        await JsonFileService.SaveAsync(FileName, _user);
    }

    public UserData GetSession()
    {
        return _user;
    }
}
