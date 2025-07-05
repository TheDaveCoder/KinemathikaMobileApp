using UnityEngine;

[CreateAssetMenu(fileName = "ProblemBundle", menuName = "Scriptable Objects/ProblemBundle")]
public class ProblemBundle : ScriptableObject
{
    [Tooltip("AR Scene Pack prefab")]
    public GameObject scenePackPrefab;

    [Tooltip("Question metadata JSON")]
    public TextAsset metadataJson;
}
