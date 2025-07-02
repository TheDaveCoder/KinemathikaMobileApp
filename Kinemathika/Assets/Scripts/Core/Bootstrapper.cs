using UnityEngine;

public class Bootstrapper : MonoBehaviour
{
    [SerializeField] private AppBuilder appBuilder;

    private void Awake()
    {

        // Dependencies
        var sessionManager = new LocalSessionManager();
        var conceptStateManager = new ConceptStateManager();

        // Register dependencies
        AppContext.Instance.RegisterService<IUserSessionManager>(sessionManager);
        AppContext.Instance.RegisterService<IConceptStateManager>(conceptStateManager);

        appBuilder.Build();
        Destroy(gameObject);
    }
}
