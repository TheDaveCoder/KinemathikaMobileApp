using TMPro;
using UnityEngine;

public class ConceptListController : MonoBehaviour
{
    [SerializeField] private GameObject conceptCardPrefab;
    void Start()
    {
        var UiTitle = transform.Find("Header/Title");
        var conceptContainer = transform.Find("ConceptList/Content");
        var userData = AppContext.Instance.GetService<IUserSessionManager>().GetSession();
        var metadata = AppContext.Instance.GetService<IConceptStateManager>().GetProblemHeaderMetadata();
        var currentConcept = metadata.concepts.Find(concept => concept.id == userData.latestConcept);

        UiTitle.GetComponent<TMP_Text>().text = currentConcept.title;
        foreach (var concept in metadata.concepts)
        {
            var UiConceptCard = Instantiate(conceptCardPrefab, conceptContainer);
            var UiConceptTitle = UiConceptCard.transform.Find("title_bar/content/concept_title");
            UiConceptTitle.GetComponent<TMP_Text>().text = concept.title;
        }
    }
}
