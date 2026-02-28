using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathScreen : MonoBehaviour {
    [SerializeField] private GameObject deathScreenUI;
    [SerializeField] private string mainMenuSceneName = "Menu";
    [SerializeField] private int currentSceneIndex = 1;
    [SerializeField] private float deathAnimationDelay = 1f;

    private bool _isDead = false;

    private void Start() {
        if (Player.Instance != null) {
            Player.Instance.OnPlayerDeath += Player_OnPlayerDeath;
        }
        if (deathScreenUI != null) {
            deathScreenUI.SetActive(false);
        }
    }

    private void Player_OnPlayerDeath(object sender, System.EventArgs e) {
        if (!_isDead) {
            StartCoroutine(ShowDeathScreenAfterDelay());
        }
    }

    private IEnumerator ShowDeathScreenAfterDelay() {
        _isDead = true;
        yield return new WaitForSeconds(deathAnimationDelay);
        Time.timeScale = 0f;
        if (deathScreenUI != null) {
            deathScreenUI.SetActive(true);
        }
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Debug.Log("Игрок умер!");
    }

    public void ReturnToMenu() {
        if (XPManager.Instance != null) {
            XPManager.Instance.ResetXP();
        }
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenuSceneName);
    }

    public void Retry() {
        if (XPManager.Instance != null) {
            XPManager.Instance.ResetXP();
        }
        Time.timeScale = 1f;
        SceneManager.LoadScene(currentSceneIndex);
    }

    public void QuitGame() {
        Time.timeScale = 1f;
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private void OnDestroy() {
        if (Player.Instance != null) {
            Player.Instance.OnPlayerDeath -= Player_OnPlayerDeath;
        }
    }
}