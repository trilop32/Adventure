using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {
    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private string mainMenuSceneName = "Menu";
    [SerializeField] private Player player;

    private bool isPaused = false;

    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    public void PauseGame() {
        Time.timeScale = 0f;
        pauseMenuUI.SetActive(true);
        isPaused = true;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        if (player != null)
            player.SetPaused(true);
    }

    public void ResumeGame() {
        Time.timeScale = 1f;
        pauseMenuUI.SetActive(false);
        isPaused = false;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        if (player != null)
            player.SetPaused(false);
    }

    public void ExitToMenu() {
        Time.timeScale = 1f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        if (player != null)
            player.SetPaused(false);

        SceneManager.LoadScene(mainMenuSceneName);
    }

    public void QuitGame() {
        Time.timeScale = 1f;
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}