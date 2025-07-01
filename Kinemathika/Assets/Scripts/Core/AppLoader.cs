using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AppLoader : MonoBehaviour
{
    [SerializeField] private GameObject splashScreen;
    [SerializeField] private GameObject userSetupScreenPrefab; // For account creation
    [SerializeField] private GameObject homeGO;

    private IUserSessionManager _sessionManager;
    private IConceptStateManager _conceptStateManager;
    private ProblemHeaderMetadata _problemMetadata;

    // Manager Objects
    public void Construct(IUserSessionManager sessionManager, IConceptStateManager conceptStateManager)
    {
        _sessionManager = sessionManager;
        _conceptStateManager = conceptStateManager;
    }

    async void Start()
    {
        // Delay splash screen
        await Task.Delay(500);

        await _sessionManager.InitializeAsync();

        _problemMetadata = await JsonFileService.LoadAsync<ProblemHeaderMetadata>("problem_header_metadata.json");

        bool hasUser = await _sessionManager.CheckSessionAsync();

        if (hasUser)
            LaunchMainScreen();
        else
        {
            var setupGO = Instantiate(userSetupScreenPrefab);
            StartCoroutine(UIUtility.Rebuild(setupGO));
            Destroy(splashScreen);
            var setupController = setupGO.GetComponent<UserSetupController>();
            setupController.OnComplete += name =>
            {
                _sessionManager.CreateSessionAsync(name, _problemMetadata);
                LaunchMainScreen();
            };
        }
    }

    private void LaunchMainScreen()
    {
        var userData = _sessionManager.GetSession();
        _conceptStateManager.LoadProblemHeaderMetadata(_problemMetadata);
        homeGO.transform.Find("UpperSection/Header/HeaderText/AccountName").GetComponent<TextMeshProUGUI>().text = "Hi," + userData.username;
        homeGO.SetActive(true);
        homeGO.GetComponent<HomeScreenController>().Initialize(_conceptStateManager, userData);
        StartCoroutine(UIUtility.Rebuild(homeGO));
        Destroy(splashScreen);
    }
}
