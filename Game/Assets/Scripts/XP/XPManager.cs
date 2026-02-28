using System;
using UnityEngine;

public class XPManager : MonoBehaviour {
    public static XPManager Instance { get; private set; }

    public event EventHandler OnXPChanged;
    public event EventHandler OnLevelUp;

    [SerializeField] private int xpToNextLevel = 10;
    [SerializeField] private float xpMultiplier = 1.5f;

    private int _currentXP = 0;
    private int _currentLevel = 1;
    private int _xpInCurrentLevel = 0;
    private int _initialXpToNextLevel;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else {
            Destroy(gameObject);
        }
        _initialXpToNextLevel = xpToNextLevel;
    }

    public void AddXP(int amount) {
        _currentXP += amount;
        _xpInCurrentLevel += amount;
        OnXPChanged?.Invoke(this, EventArgs.Empty);

        CheckLevelUp();
    }

    private void CheckLevelUp() {
        if (_xpInCurrentLevel >= xpToNextLevel) {
            LevelUp();
        }
    }

    private void LevelUp() {
        _currentLevel++;
        _xpInCurrentLevel -= xpToNextLevel;
        xpToNextLevel = Mathf.FloorToInt(xpToNextLevel * xpMultiplier);
        OnLevelUp?.Invoke(this, EventArgs.Empty);
        Debug.Log($"LEVEL UP! Current Level: {_currentLevel}");
    }
    public void ResetXP() {
        _currentXP = 0;
        _currentLevel = 1;
        _xpInCurrentLevel = 0;
        xpToNextLevel = _initialXpToNextLevel;
        OnXPChanged?.Invoke(this, EventArgs.Empty);
        Debug.Log("XP סבנמרום!");
    }

    public int GetCurrentXP() => _currentXP;
    public int GetCurrentLevel() => _currentLevel;
    public int GetXPInCurrentLevel() => _xpInCurrentLevel;
    public int GetXPToNextLevel() => xpToNextLevel;

    public float GetXPPercentage() {
        return (float)_xpInCurrentLevel / xpToNextLevel;
    }
}