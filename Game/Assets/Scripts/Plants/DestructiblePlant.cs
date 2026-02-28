using System;
using UnityEngine;

public class DestructiblePlant : MonoBehaviour
{
    [SerializeField] private GameObject xpPrefab;
    [SerializeField] private int xpValue = 1;

    public event EventHandler OnDestructibleTakeDamage;

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.GetComponent<Sword>()) {
            OnDestructibleTakeDamage?.Invoke(this, EventArgs.Empty);
            SpawnXP();
            Destroy(gameObject);
            NavMeshSurfaceManagement.Instance.RebakeNavmeshSurface();
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
