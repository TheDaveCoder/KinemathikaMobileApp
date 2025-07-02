using TMPro;
using UnityEngine;

public class ConceptOverviewController : MonoBehaviour
{
    [SerializeField] private GameObject worksheetInformationPrefab;
    void Start()
    {
        var worksheetInfoContainer = transform.Find("WorksheetList/Content");
        var conceptTitle = transform.Find("Header/ConceptTitle");
        var userData = AppContext.Instance.GetService<IUserSessionManager>().GetSession();
        var metadata = AppContext.Instance.GetService<IConceptStateManager>().GetProblemHeaderMetadata();
        var currentConcept = metadata.concepts.Find(concept => concept.id == userData.latestConcept);

        conceptTitle.GetComponent<TMP_Text>().text = currentConcept.title;
        foreach (var worksheet in currentConcept.worksheets)
        {
            var UiWorksheetInfo = Instantiate(worksheetInformationPrefab, worksheetInfoContainer);
            var UiWorksheetName = UiWorksheetInfo.transform.Find("WorksheetDivider/WorksheetName");
            var UiWorksheetDescription = UiWorksheetInfo.transform.Find("Description");
            UiWorksheetName.GetComponent<TMP_Text>().text = worksheet.worksheetName;
            UiWorksheetDescription.GetComponent<TMP_Text>().text = worksheet.worksheetDescription;
        }
    }
}
