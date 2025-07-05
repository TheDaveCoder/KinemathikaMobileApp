using UnityEngine;

public interface IPlayModeManager
{
    void SetupLoadingScreen(GameObject loadingScreen);
    bool HasLoadingScreen();
    void Load(LevelButtonData levelButtonData);
    public IProblemManager GetCurrentProblem();
    void Return(bool newProgresse);
    void LoadDummy(LevelButtonData levelButtonData);
}