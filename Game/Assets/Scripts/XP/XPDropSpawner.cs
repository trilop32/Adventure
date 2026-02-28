using UnityEngine;

public static class XPDropSpawner {
    [SerializeField] private static GameObject xpPrefab;

    public static void SpawnXP(Vector3 position, int xpValue, GameObject prefab = null) {
        GameObject xpObject = prefab != null ? Object.Instantiate(prefab, position, Quaternion.identity): CreateDefaultXP(position);

        XPPickup xpPickup = xpObject.GetComponent<XPPickup>();
        if (xpPickup != null) {
            xpPickup.SetXPValue(xpValue);
        }
    }

    private static GameObject CreateDefaultXP(Vector3 position) {
        GameObject xpObject = new GameObject("XP");
        xpObject.transform.position = position;
        SpriteRenderer sr = xpObject.AddComponent<SpriteRenderer>();
        sr.color = Color.yellow;
        CircleCollider2D collider = xpObject.AddComponent<CircleCollider2D>();
        collider.radius = 0.3f;
        xpObject.AddComponent<XPPickup>();
        return xpObject;
    }
}