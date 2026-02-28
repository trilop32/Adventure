using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelComplete : MonoBehaviour {
    [SerializeField] private GameObject levelCompleteUI;
    [SerializeField] private string mainMenuSceneName = "Menu";
    [SerializeField] private string nextLevelSceneName = "Level2";

    private bool _isLevelComplete = false;

    private void Start() {
        if (XPManager.Instance != null) {
            XPManager.Instance.OnLevelUp += XPManager_OnLevelUp;
        }
        if (levelCompleteUI != null) {
            levelCompleteUI.SetActive(false);
        }
    }

    private void XPManager_OnLevelUp(object sender, System.EventArgs e) {
        if (XPManager.Instance.GetCurrentLevel() >= 2 && !_isLevelComplete) {
            ShowLevelComplete();
        }
    }

    private void ShowLevelComplete() {
        _isLevelComplete = true;
        Time.timeScale = 0f;
        if (Player.Instance != null) {
            Player.Instance.SetLevelComplete(true);
        }
        if (levelCompleteUI != null) {
            levelCompleteUI.SetActive(true);
        }
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Debug.Log("Уровень завершён!");
    }

    public void ReturnToMenu() {
        if (XPManager.Instance != null) {
            XPManager.Instance.ResetXP();
        }
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenuSceneName);
    }

    public void NextLevel() {
        Debug.Log("Следующий уровень ещё не готов!");
        //UIManager.Instance.ShowMessage("Следующий уровень в разработке!");
    }

    private void OnDestroy() {
        if (XPManager.Instance != null) {
            XPManager.Instance.OnLevelUp -= XPManager_OnLevelUp;
        }
    }
}