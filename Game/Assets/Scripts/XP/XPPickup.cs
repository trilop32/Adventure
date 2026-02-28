using UnityEngine;

public class XPPickup : MonoBehaviour {
    [SerializeField] private int xpValue = 1;
    [SerializeField] private float collectionRadius = 3f;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 100f;

    private Player _player;
    private bool _isMovingToPlayer = false;
    private SpriteRenderer _spriteRenderer;

    private void Awake() {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        CircleCollider2D collider = GetComponent<CircleCollider2D>();
        if (collider != null) {
            collider.isTrigger = true;
        }
    }

    private void Start() {
        _player = Player.Instance;
    }

    private void Update() {
        if (_player == null || !_player.IsAlive()) return;
        float distanceToPlayer = Vector2.Distance(transform.position, _player.transform.position);
        if (distanceToPlayer <= collectionRadius) {
            _isMovingToPlayer = true;
        }
        if (_isMovingToPlayer) {
            MoveToPlayer();
        }
        transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
    }

    private void MoveToPlayer() {
        Vector3 direction = (_player.transform.position - transform.position).normalized;
        transform.position += direction * moveSpeed * Time.deltaTime;
        float distanceToPlayer = Vector2.Distance(transform.position, _player.transform.position);
        if (distanceToPlayer <= 0.5f) {
            CollectXP();
        }
    }

    private void CollectXP() {
        XPManager.Instance?.AddXP(xpValue);
        Destroy(gameObject);
    }
    public void SetXPValue(int value) {
        xpValue = value;
        if (value >= 3) {
            _spriteRenderer.color = Color.yellow;
        }
        else {
            _spriteRenderer.color = Color.cyan;
        }
    }
}