using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{

    [SerializeField] GameObject prefabToSpawn;
    [SerializeField] float delay;
    [SerializeField] int maxAmount;

    List<GameObject> amountOfPrefabs = new List<GameObject>();

    void Start() {
        InvokeRepeating("SpawnPrefab", delay, delay);
    }

    void Update() {
        for(int i = amountOfPrefabs.Count - 1; i >= 0; i--) {
            if(amountOfPrefabs[i] == null) {
                amountOfPrefabs.RemoveAt(i);
            }
        }
    }

    void SpawnPrefab() {
        if(amountOfPrefabs.Count >= maxAmount) return;

        GameObject spawnedPrefab = Instantiate(prefabToSpawn);
        amountOfPrefabs.Add(spawnedPrefab);
    }

}
