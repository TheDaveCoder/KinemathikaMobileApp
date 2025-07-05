using System.Collections.Generic;

// User Data
[System.Serializable]
public class UserData
{
    public string username;
    public string latestConcept;
    public string latestProblemId;
    public List<ConceptProgress> progress;
}

[System.Serializable]
public class ConceptProgress
{
    public string conceptId;
    public List<WorksheetProgress> worksheets;
}

[System.Serializable]
public class WorksheetProgress
{
    public string worksheetId;
    public List<string> completedProblems;
}

// Problem Header Metadata
[System.Serializable]
public class ProblemHeaderMetadata
{
    public List<Concept> concepts = new List<Concept>();
}
[System.Serializable]
public class Concept
{
    public string id;
    public string title;
    public List<WorksheetHeader> worksheets = new List<WorksheetHeader>();
}
[System.Serializable]
public class WorksheetHeader
{
    public string worksheetId;
    public string worksheetName;
    public string worksheetDescription;
    public List<ProblemHeader> problems = new List<ProblemHeader>();
}
[System.Serializable]
public class ProblemHeader
{
    public string problemId;
    public int problemType;
    public string title;
    public string description;
}

// Question List Wrapper
[System.Serializable]
public class QuestionListWrapper { public List<QuestionMetadata> questions; }

// Full Question Metadata
[System.Serializable]
public class QuestionMetadata
{
    public string description;
    public int correctAnswer;
    public int duration;
    public int timeScale;
}