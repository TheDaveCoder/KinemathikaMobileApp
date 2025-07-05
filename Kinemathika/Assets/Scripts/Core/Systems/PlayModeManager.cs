using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;
using System.Collections;
using System.Linq;

public class PlayModeManager : IPlayModeManager
{
    private IProblemManager _currentProblemManager;
    private GameObject _loadingScreen;
    private string UI_SCENE = "UIScene";
    private string AR_SCENE = "ARScene";
    private string currentConcept;
    private string currentWorksheet;
    private string currentProblem;
    public void SetupLoadingScreen(GameObject loadingScreen)
    {
        _loadingScreen = loadingScreen;
    }
    public bool HasLoadingScreen()
    {
        return _loadingScreen != null;
    }

    public async void Load(LevelButtonData levelButtonData)
    {
        currentConcept = levelButtonData.ConceptId;
        currentWorksheet = levelButtonData.WorksheetId;
        currentProblem = levelButtonData.ProblemId;
        _loadingScreen.SetActive(true);
        // Load AR
        var op = SceneManager.LoadSceneAsync(AR_SCENE, LoadSceneMode.Single);
        while (!op.isDone) await Task.Yield();

        // instantiate specific manager based on problemId type
        _currentProblemManager = ProblemManagerFactory.Create(levelButtonData);
        await _currentProblemManager.Initialize(levelButtonData, _loadingScreen);
    }
    public IProblemManager GetCurrentProblem()
    {
        return _currentProblemManager;
    }

    public async void Return(bool newProgress)
    {
        Debug.Log("Return Called!");
        _loadingScreen.SetActive(true);

        if (newProgress)
        {
            Debug.Log("App Context: " + AppContext.Instance);
            Debug.Log("User Manager: " + AppContext.Instance.GetService<IUserSessionManager>());
            Debug.Log(AppContext.Instance.GetService<IUserSessionManager>().GetSession());
            var userData = AppContext.Instance.GetService<IUserSessionManager>().GetSession();
            var concept = userData.progress.Find(c => c.conceptId == currentConcept);
            if (concept != null)
            {
                var worksheet = concept.worksheets.Find(w => w.worksheetId == currentWorksheet);
                if (worksheet != null && !worksheet.completedProblems.Contains(currentProblem))
                {
                    worksheet.completedProblems.Add(currentProblem);
                }
                userData.latestProblemId = currentProblem;
                userData.latestConcept = currentConcept;

                // Save updated userData to disk
                await JsonFileService.SaveAsync("user_session.json", userData);
            }
        }

        _currentProblemManager = null;
        await SceneManager.LoadSceneAsync(UI_SCENE, LoadSceneMode.Single);
        Debug.Log("Finished Unloading");
        _loadingScreen.SetActive(false);
    }


    public async void LoadDummy(LevelButtonData levelButtonData)
    {
        currentConcept = levelButtonData.ConceptId;
        currentWorksheet = levelButtonData.WorksheetId;
        currentProblem = levelButtonData.ProblemId;
        _currentProblemManager = ProblemManagerFactory.Create(levelButtonData);
        await _currentProblemManager.InitializeDummy(levelButtonData);
    }
}