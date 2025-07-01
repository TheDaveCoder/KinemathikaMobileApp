using UnityEngine;

public class Bootstrapper : MonoBehaviour
{
    [SerializeField] private AppLoader appLoader;
    void Awake()
    {
        // Dependencies
        IUserSessionManager sessionManager = new LocalSessionManager();
        IConceptStateManager conceptStateManager = new ConceptStateManager();

        // Inject and Launch App
        appLoader.Construct(sessionManager, conceptStateManager);
    }
}

