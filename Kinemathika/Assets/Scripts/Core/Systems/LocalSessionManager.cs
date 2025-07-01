using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using System.Collections.Generic;
using System;

public class LocalSessionManager : IUserSessionManager
{
    private const string FileName = "user_session.json";
    private string _filePath;
    private UserData _user;

    public async Task InitializeAsync()
    {
        _filePath = Path.Combine(Application.persistentDataPath, FileName);

        var dir = Path.GetDirectoryName(_filePath);
        if (!Directory.Exists(dir))
            Directory.CreateDirectory(dir);

        await Task.CompletedTask;
    }
    public async Task<bool> CheckSessionAsync()
    {
        if (!File.Exists(_filePath))
        {
            return false;
        }

        try
        {
            string json = await File.ReadAllTextAsync(_filePath);
            _user = JsonUtility.FromJson<UserData>(json);
            return !string.IsNullOrEmpty(_user?.username);
        }
        catch
        {
            Debug.LogWarning("File exists but parsing failed.");
            return false;
        }
    }
    public async Task CreateSessionAsync(string username, ProblemHeaderMetadata problemMetadata)
    {
        _user = new UserData
        {
            username = username,
            latestConcept = "",
            latestProblemId = "a1-1",
            progress = new List<ConceptProgress>()
        };

        foreach (var concept in problemMetadata.concepts)
        {
            var conceptProgress = new ConceptProgress
            {
                conceptId = concept.id,
                worksheets = new List<WorksheetProgress>()
            };
            Debug.Log("Concept ID: " + concept.title);
            _user.progress.Add(conceptProgress);


            foreach (var worksheet in concept.worksheets)
            {
                var worksheetProgress = new WorksheetProgress
                {
                    worksheetId = worksheet.worksheetId,
                    completedProblems = new List<string>()
                };
                conceptProgress.worksheets.Add(worksheetProgress);
            }
        }

        string json = JsonUtility.ToJson(_user, prettyPrint: true);
        await File.WriteAllTextAsync(_filePath, json);
    }

    public UserData GetSession()
    {
        return _user;
    }
}