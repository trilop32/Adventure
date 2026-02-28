using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public void ToGame() {
        if (XPManager.Instance != null) {
            XPManager.Instance.ResetXP();
        }
        SceneManager.LoadScene(1);
    }
    public void QuitGame() {
        Application.Quit();
    }
}
