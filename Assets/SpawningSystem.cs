using UnityEngine;

public class SpawningSystem : MonoBehaviour {
    [SerializeField] GameObject[] enemyPrefabs;
    [SerializeField] float[] enemyWeights;
    [SerializeField] float spawnInterval = 5.0f;
    [SerializeField] float timeToNextDifficulty = 60.0f;

    private float lastSpawnTime;
    private int currentDifficultyIndex;
    private float timeToNextDifficultyCountdown;

    void Start() {
        lastSpawnTime = Time.time;
        currentDifficultyIndex = 0;
        timeToNextDifficultyCountdown = timeToNextDifficulty;
    }

    void Update() {
        if (Time.time - lastSpawnTime > spawnInterval) {
            lastSpawnTime = Time.time;
            SpawnEnemy();
            IncreaseDifficulty();
        }
    }

    void SpawnEnemy() {
        float totalWeight = 0.0f;
        for(int i = currentDifficultyIndex; i < enemyWeights.Length; i++) {
            totalWeight += enemyWeights[i];
        }
        float randomValue = Random.value * totalWeight;
        for (int i = currentDifficultyIndex; i < enemyWeights.Length; i++) {
            if (randomValue < enemyWeights[i]) {
                Instantiate(enemyPrefabs[i], transform.position, Quaternion.identity);
                break;
            }
            randomValue -= enemyWeights[i];
        }
    }

    void IncreaseDifficulty() {
        timeToNextDifficultyCountdown -= Time.deltaTime;
        if (timeToNextDifficultyCountdown <= 0) {
            currentDifficultyIndex++;
            timeToNextDifficultyCountdown = timeToNextDifficulty;
        }
    }
}
