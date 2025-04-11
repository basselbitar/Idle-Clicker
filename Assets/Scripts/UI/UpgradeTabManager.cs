using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UpgradeTabManager : MonoBehaviour {
    [Header("Tab Buttons")]
    public Button mazeButton;
    public Button idleButton;
    public Button mouseButton;
    public Button achievementButton;

    [Header("Tab Panels")]
    public GameObject mazeTab;
    public GameObject idleTab;
    public GameObject mouseTab;
    public GameObject achievementTab;

    private void Start() {
        mazeButton.onClick.AddListener(() => ShowTab(mazeTab));
        idleButton.onClick.AddListener(() => ShowTab(idleTab));
        mouseButton.onClick.AddListener(() => ShowTab(mouseTab));
        achievementButton.onClick.AddListener(() => ShowTab(achievementTab));

        ShowTab(mazeTab);
    }

    private void ShowTab(GameObject tabToShow) {
        mazeTab.SetActive(tabToShow == mazeTab);
        idleTab.SetActive(tabToShow == idleTab);
        mouseTab.SetActive(tabToShow == mouseTab);
        achievementTab.SetActive(tabToShow == achievementTab);
    }
}
