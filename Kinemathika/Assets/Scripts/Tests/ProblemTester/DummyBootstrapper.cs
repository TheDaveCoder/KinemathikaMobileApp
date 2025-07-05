using UnityEngine;

public class DummyBootstrapper : MonoBehaviour
{
    [SerializeField] private GameObject dummyLevelButtonData;
    private async void Awake()
    {
        // Instantiate App Context
        AppContext.Instance.RegisterService<IConceptStateManager>(new ConceptStateManager());
        AppContext.Instance.RegisterService<IPlayModeManager>(new PlayModeManager());

        // Initialize dummy values
        var metadata = await JsonFileService.LoadAsync<ProblemHeaderMetadata>("problem_header_metadata.json");
        AppContext.Instance.GetService<IConceptStateManager>().LoadProblemHeaderMetadata(metadata);
        var dummyLevelData = dummyLevelButtonData.GetComponent<DummyInit>().GetDummyValues();
        AppContext.Instance.GetService<IPlayModeManager>().LoadDummy(dummyLevelData);
        Destroy(gameObject);
    }
}
