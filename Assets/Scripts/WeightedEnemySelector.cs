using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WeightedEnemySelector : MonoBehaviour
{
    public GameObject[] enemies; // array of enemy GameObjects, ordered by difficulty

    public void SetEnemies(GameObject[] _enemies) => enemies = _enemies;

    public GameObject SelectEnemy(float normalizedValue, float width)
    {
        // Scale mean based on length of enemies array and normalizedValue
        float mean = Mathf.Lerp(0, enemies.Length - 1, Mathf.Clamp01(normalizedValue));
        // Calculate weights based on normal distribution curve
        float[] weights = new float[enemies.Length];
        for (int i = 0; i < enemies.Length; i++)
        {
            float x = i;
            float stdDev = width;
            weights[i] = (1.0f / (stdDev * Mathf.Sqrt(2 * Mathf.PI))) * Mathf.Exp(-0.5f * Mathf.Pow((x - mean) / stdDev, 2));
        }

        // Normalize weights
        float totalWeight = weights.Sum();
        for (int i = 0; i < weights.Length; i++)
        {
            weights[i] /= totalWeight;
        }

        // Select enemy based on weights
        float randomValue = Random.value;
        for (int i = 0; i < weights.Length; i++)
        {
            if (randomValue < weights[i])
            {
                return enemies[i];
            }
            randomValue -= weights[i];
        }

        // Default to last enemy in array
        return enemies[enemies.Length - 1];
    }
}