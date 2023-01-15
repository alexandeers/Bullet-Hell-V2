using UnityEngine;

public class EnemySpawningSystem : MonoBehaviour {
    public Transform player;
    public new Transform camera;
    public GameObject enemyPrefab;
    public float spawnRadius = 10f;
    public int maxEnemies = 5;
    private int spawnedEnemies = 0;
    private float spawnTimer = 0f;
    public float spawnInterval = 5f;

    void Update() {
        spawnTimer += Time.deltaTime;
        if (spawnedEnemies < maxEnemies && spawnTimer >= spawnInterval) {
            GameObject enemy = null;
            int iterations = 0;
            while (enemy == null) {
                iterations++;
                enemy = TrySpawnEnemy();
                if(iterations == 250) break;
            }
            spawnTimer = 0f;
        }
    }

    GameObject TrySpawnEnemy() {
        Vector2 spawnPos = (Vector2)player.position + (Vector2)Random.insideUnitCircle * spawnRadius;
        print(IsInsideCamera(spawnPos));
        print(IsInsideWall(spawnPos));
        if (!IsInsideCamera(spawnPos) || IsInsideWall(spawnPos)) {
            return null;
        }
        spawnedEnemies++;
        return Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
    }

    bool IsInsideCamera(Vector3 pos) {
        Vector3 screenPos = camera.GetComponent<Camera>().WorldToScreenPoint(pos);
        if (screenPos.x < 0 || screenPos.x > Screen.width || screenPos.y < 0 || screenPos.y > Screen.height) {
            return true;
        }
        return false;
    }

    bool IsInsideWall(Vector3 pos) {
        return Physics.CheckSphere(pos, 0.1f, LayerMask.GetMask("Wall"));
    }
}
