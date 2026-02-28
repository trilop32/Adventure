using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[SelectionBase]
public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }
    public event EventHandler OnPlayerDeath;
    public event EventHandler OnFlashBlink;
    public Image LineBar;
    public int GetCurrentHealth() => _currentHealth;
    public int GetMaxHealth() => maxHealth;
    [SerializeField] private float movingSpeed = 10f;
    [SerializeField] private int maxHealth = 10;
    [SerializeField] private float damageRecoveryTime = 0.5f;
    [Space(20)]
    [SerializeField] private int dashSpeed = 4;
    [SerializeField] private float dashTime = 0.2f;
    [SerializeField] private TrailRenderer trailRenderer;
    [SerializeField] private float dashCoolDownTime = 0.25f;

    private Vector2 _inputVector;

    private Rigidbody2D _rb;
    private KnockBack _knockBack;

    private readonly float _minMovingSpeed = 0.1f;
    private bool _isRunning = false;

    private int _currentHealth;
    private bool _canTakeDamage;
    private bool _isAlive;
    private bool _isDashing;
    private float _initialMovingSpeed;
    
    private Camera _mainCamera;

    private bool _isPaused = false;
    private bool _isLevelComplete = false;
    private bool _isDead = false;

    private void Awake()
    {
        Instance = this;
        _rb = GetComponent<Rigidbody2D>();
        _knockBack = GetComponent<KnockBack>();
        _mainCamera = Camera.main;        
        _initialMovingSpeed  = movingSpeed;
    }

    private void Start()
    {
        _currentHealth = maxHealth;
        _canTakeDamage = true;
        _isAlive = true;        
        GameInput.Instance.OnPlayerAttack += GameInput_OnPlayerAttack;
        GameInput.Instance.OnPlayerDash += GameInput_OnPlayerDash;
    }
    
    private void Update()
    {
        if (_isPaused || _isLevelComplete || _isDead) {
            _inputVector = Vector2.zero;
            return;
        }
        _inputVector = GameInput.Instance.GetMovementVector();
    }

    private void FixedUpdate()
    {
        if (_isPaused || _isLevelComplete || _isDead) return;
        if (_knockBack.IsGettingKnockedBack)
            return;
        HandleMovement();
    }
    public void SetPaused(bool paused) {
        _isPaused = paused;
    }
    public void SetLevelComplete(bool complete) {
        _isLevelComplete = complete;
    }
    public void SetDead(bool dead) {
        _isDead = dead;
    }

    public bool IsAlive() => _isAlive;

    public void TakeDamage(Transform damageSource, int damage)
    {
        if (_canTakeDamage && _isAlive)
        {
            _canTakeDamage = false;
            _currentHealth = Mathf.Max(0, _currentHealth -= damage);
            Debug.Log(_currentHealth);
            //LineBar.fillAmount = _currentHealth;
            _knockBack.GetKnockedBack(damageSource);

            OnFlashBlink?.Invoke(this, EventArgs.Empty);

            StartCoroutine(DamageRecoveryRoutine());
        }

        DetectDeath();
    }

    private void DetectDeath()
    {
        if (_currentHealth == 0 && _isAlive)
        {
            _isAlive = false;
            _isDead = true;
            _knockBack.StopKnockBackMovement();
            GameInput.Instance.DisableMovement();
            OnPlayerDeath?.Invoke(this, EventArgs.Empty);
        }
    }
    
    private void GameInput_OnPlayerDash(object sender, System.EventArgs e) {
        if (_isPaused || _isLevelComplete || _isDead) return;
        Dash();
    }

    private void Dash()
    {
        if (!_isDashing)
            StartCoroutine(DashRoutine());
    }

    private IEnumerator DashRoutine()
    {
        _isDashing  = true;
        movingSpeed *= dashSpeed;
        trailRenderer.emitting = true;
        yield return new WaitForSeconds(dashTime);
        
        trailRenderer.emitting = false;
        movingSpeed = _initialMovingSpeed;
        
        yield return new WaitForSeconds(dashCoolDownTime);
        _isDashing  = false;
    }

    private IEnumerator DamageRecoveryRoutine()
    {
        yield return new WaitForSeconds(damageRecoveryTime);
        _canTakeDamage = true;
    }

    public bool IsRunning()
    {
        return _isRunning;
    }

    private void GameInput_OnPlayerAttack(object sender, System.EventArgs e)
    {
        if (_isPaused || _isLevelComplete || _isDead) return;
        ActiveWeapon.Instance.GetActiveWeapon().Attack();
    }

    private void HandleMovement()
    {
        _rb.MovePosition(_rb.position + _inputVector * (movingSpeed * Time.fixedDeltaTime));
        if (Mathf.Abs(_inputVector.x) > _minMovingSpeed || Mathf.Abs(_inputVector.y) > _minMovingSpeed)
        {
            _isRunning = true;
        } else
        {
            _isRunning = false;
        }
    }

    public Vector3 GetPlayerScreenPosition()
    {
        Vector3 playerScreenPosition = _mainCamera.WorldToScreenPoint(transform.position);
        return playerScreenPosition;
    }

    private void OnDestroy()
    {
        GameInput.Instance.OnPlayerAttack -= GameInput_OnPlayerAttack;
    }
}