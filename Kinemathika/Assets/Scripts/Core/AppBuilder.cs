using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class AppBuilder : MonoBehaviour
{
    [SerializeField] private GameObject splashScreen;
    [SerializeField] private GameObject userSetupScreenPrefab;
    [SerializeField] private GameObject homeGO;
    private AppContext _appContext;
    private ProblemHeaderMetadata _metadata;
    private UserData _userData;

    public async void Build()
    {
        await Task.Delay(500); // Lil delay
        _appContext = AppContext.Instance;
        var sessionManager = _appContext.GetService<IUserSessionManager>();
        var conceptManager = _appContext.GetService<IConceptStateManager>();

        var metadata = await JsonFileService.LoadAsync<ProblemHeaderMetadata>("problem_header_metadata.json");
        conceptManager.LoadProblemHeaderMetadata(metadata);

        bool hasUser = await sessionManager.CheckSessionAsync();

        if (hasUser)
        {
            _userData = sessionManager.GetSession();
            LaunchMainScreen();
        }
        else
        {
            var setupGO = Instantiate(userSetupScreenPrefab);
            StartCoroutine(UIUtility.Rebuild(setupGO));
            Destroy(splashScreen);

            var setupController = setupGO.GetComponent<UserSetupController>();
            setupController.OnComplete += async name =>
            {
                await sessionManager.CreateSessionAsync(name, _metadata);
                _userData = sessionManager.GetSession();
                LaunchMainScreen();
            };
        }
    }

    private void LaunchMainScreen()
    {
        homeGO.transform
            .Find("UpperSection/Header/HeaderText/AccountName")
            .GetComponent<TextMeshProUGUI>()
            .text = "Hi, " + _userData.username;

        homeGO.SetActive(true);
        homeGO.GetComponent<HomeScreenController>().Initialize();
        StartCoroutine(UIUtility.Rebuild(homeGO));
        Destroy(splashScreen);
        Destroy(gameObject);
    }
}

