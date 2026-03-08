using System;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(PolygonCollider2D))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(EnemyAI))]
public class EnemyEntity : MonoBehaviour
{
    [SerializeField] private EnemySO enemySO;
    [SerializeField] private GameObject xpPrefab;
    [SerializeField] private int xpValue = 3;

    public event EventHandler OnTakeHit;
    public event EventHandler OnDeath;
    
    private int _currentHealth;

    private PolygonCollider2D _polygonCollider2D;
    private BoxCollider2D _boxCollider2D;
    private EnemyAI _enemyAI;

    private void Awake()
    {
        _polygonCollider2D = GetComponent<PolygonCollider2D>();
        _boxCollider2D = GetComponent<BoxCollider2D>();
        _enemyAI = GetComponent<EnemyAI>();        
    }

    private void Start()
    {
        _currentHealth = enemySO.enemyHealth;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.transform.TryGetComponent(out Player player))
        {
            player.TakeDamage(transform, enemySO.enemyDamageAmount);
        }
    }

    public void TakeDamage(int damage)
    {
        _currentHealth -= damage;
        OnTakeHit?.Invoke(this, EventArgs.Empty);
        DetectDeath();
    }

    public void PolygonColliderTurnOff()
    {
        _polygonCollider2D.enabled = false;
    }

    public void PolygonColliderTurnOn()
    {
        _polygonCollider2D.enabled = true;
    }

    private void DetectDeath()
    {
        if (_currentHealth <= 0) {
            _boxCollider2D.enabled = false;
            _polygonCollider2D.enabled = false;
            _enemyAI.SetDeathState();
            var navMeshAgent = GetComponent<NavMeshAgent>();
            if (navMeshAgent != null) {
                navMeshAgent.isStopped = true;
                navMeshAgent.ResetPath();
                navMeshAgent.enabled = false;
            }
            var rb = GetComponent<Rigidbody2D>();
            if (rb != null) {
                rb.linearVelocity = Vector2.zero;
                rb.simulated = false;
            }
            var knockBack = GetComponent<KnockBack>();
            if (knockBack != null) {
                knockBack.StopKnockBackMovement();
                knockBack.enabled = false;
            }
            OnDeath?.Invoke(this, EventArgs.Empty);
            SpawnXP();
        }
    }

    private void SpawnXP() {
        if (xpPrefab != null) {
            XPDropSpawner.SpawnXP(transform.position, xpValue, xpPrefab);
        }
        else {
            XPDropSpawner.SpawnXP(transform.position, xpValue);
        }
    }
}
