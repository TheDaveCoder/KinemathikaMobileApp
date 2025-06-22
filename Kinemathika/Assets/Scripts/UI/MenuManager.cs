using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private GameObject intro;
    
    void Start()
    {
        ShowLoading();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowLoading()
    {
        CloseAllScreen();
        loadingScreen.SetActive(true);

    }

    public void ShowIntro()
    {
        CloseAllScreen();
        intro.SetActive(true);
    }

    public void CloseAllScreen()
    {
        loadingScreen.SetActive(false);
        intro.SetActive(false);
    }
}
