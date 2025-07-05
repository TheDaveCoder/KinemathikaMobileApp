using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class AppBuilder : MonoBehaviour
{
    [SerializeField] private GameObject splashScreen;
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private GameObject userSetupScreenPrefab;
    [SerializeField] private GameObject homeGO;
    private AppContext _appContext;
    private ProblemHeaderMetadata _metadata;
    private UserData _userData;

    public async void Build()
    {
        await Task.Delay(500); // Lil delay
        if (!AppContext.isInitialized)
        {
            _appContext = AppContext.Instance;
            // Setup app loading screen
            var playModeManager = _appContext.GetService<IPlayModeManager>();
            DontDestroyOnLoad(loadingScreen);
            playModeManager.SetupLoadingScreen(loadingScreen);

            var sessionManager = _appContext.GetService<IUserSessionManager>();
            Debug.Log("Kachow: " + sessionManager);
            var conceptManager = _appContext.GetService<IConceptStateManager>();

            var _metadata = await JsonFileService.LoadAsync<ProblemHeaderMetadata>("problem_header_metadata.json");
            conceptManager.LoadProblemHeaderMetadata(_metadata);

            bool hasUser = await sessionManager.CheckSessionAsync();

            if (hasUser)
            {
                _userData = sessionManager.GetSession();
                AppContext.isInitialized = true;
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
                    AppContext.isInitialized = true;
                    LaunchMainScreen();
                };
            }
        }
        else
        {
            _userData = AppContext.Instance.GetService<IUserSessionManager>().GetSession();
            Destroy(splashScreen);
            LaunchMainScreen();
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

