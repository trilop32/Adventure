using UnityEngine;
using UnityEngine.UI;

public class XPBar : MonoBehaviour {
    [SerializeField] private Image xpBarImage;
    [SerializeField] private Text xpText;
    [SerializeField] private Text levelText;

    private XPManager _xpManager;

    private void Start() {
        _xpManager = XPManager.Instance;
        if (_xpManager == null) {
            Debug.LogError("XPManager not found!");
            return;
        }
        _xpManager.OnXPChanged += XPManager_OnXPChanged;
        _xpManager.OnLevelUp += XPManager_OnLevelUp;
        UpdateXPBar();
    }

    private void XPManager_OnXPChanged(object sender, System.EventArgs e) {
        UpdateXPBar();
    }

    private void XPManager_OnLevelUp(object sender, System.EventArgs e) {
        UpdateXPBar();
    }

    private void UpdateXPBar() {
        if (xpBarImage == null) return;
        float fillAmount = _xpManager.GetXPPercentage();
        xpBarImage.fillAmount = fillAmount;
        if (xpText != null) {
            xpText.text = $"{_xpManager.GetXPInCurrentLevel()} / {_xpManager.GetXPToNextLevel()}";
        }
        if (levelText != null) {
            levelText.text = $"Lvl {_xpManager.GetCurrentLevel()}";
        }
    }

    private void OnDestroy() {
        if (_xpManager != null) {
            _xpManager.OnXPChanged -= XPManager_OnXPChanged;
            _xpManager.OnLevelUp -= XPManager_OnLevelUp;
        }
    }
}