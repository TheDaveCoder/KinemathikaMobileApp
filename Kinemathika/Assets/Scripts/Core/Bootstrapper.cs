using UnityEngine;

public class Bootstrapper : MonoBehaviour
{
    [SerializeField] private AppBuilder appBuilder;

    private void Awake()
    {
        if (!AppContext.isInitialized)
        {
            // Register dependencies
            AppContext.Instance.RegisterService<IUserSessionManager>(new LocalSessionManager());
            AppContext.Instance.RegisterService<IConceptStateManager>(new ConceptStateManager());
            AppContext.Instance.RegisterService<IPlayModeManager>(new PlayModeManager());
        }
        appBuilder.Build();
        Destroy(gameObject);
    }
}
