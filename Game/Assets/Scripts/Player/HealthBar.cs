using UnityEngine;
using UnityEngine.UI;
public class HealthBar : MonoBehaviour {
    [SerializeField] private Image healthBarImage;
    private Player player;
    private int maxHealth;
    private void Start() {
        player = Player.Instance;
        if (player == null) {
            Debug.LogError("Player instance not found!");
            return;
        }
        if (healthBarImage == null) {
            healthBarImage = GetComponent<Image>();
        }
        maxHealth = player.GetMaxHealth();
        UpdateHealthBar();
    }
    private void Update() {
        UpdateHealthBar();
    }
    private void UpdateHealthBar() {
        if (player == null || healthBarImage == null) return;
        int currentHealth = player.GetCurrentHealth();
        float fillAmount = (float)currentHealth / maxHealth;
        healthBarImage.fillAmount = fillAmount;
    }
}