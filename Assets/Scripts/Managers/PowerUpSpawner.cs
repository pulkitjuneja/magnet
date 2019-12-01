using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpSpawner : MonoBehaviour {
  public float minSpawnTime = 10.0f;
  public float maxSpawnTime = 15.0f;
  public Spawner[] spawners;
  public GameObject[] powerUps;
  public Signal onStateChangedSignal;

  float SpawnTimer = 0;
  float currentInterval;
  bool shouldSpawn;
  void Start() {
    onStateChangedSignal.addListener(onStateChanged);
  }

  void onStateChanged(SignalData data) {
    ClearPowerups();
    GameStates newStateType = data.get<GameStates>("newStateType");
    if (newStateType == GameStates.PlayingState) {
      shouldSpawn = true;
      SpawnTimer = 0;
      currentInterval = UnityEngine.Random.Range(minSpawnTime, maxSpawnTime);
    } else {
      shouldSpawn = false;
    }
  }

  void ClearPowerups() {
    Pickup[] powerUp = GameObject.FindObjectsOfType<Pickup>();
    foreach (var p in powerUp) {
      GameObject.Destroy(p.gameObject);
    }
  }

  void SpawnPickups() {
    var currentSpawner = spawners[UnityEngine.Random.Range(0, spawners.Length)];
    int random = UnityEngine.Random.Range(0, 11);
    int powerUpToSpawn = random < 6 ? 0 : (random < 8 ? 1 : 2);
    currentSpawner.Spawn(powerUps[powerUpToSpawn]);
  }

  void Update() {
    if (shouldSpawn) {
      SpawnTimer += Time.unscaledDeltaTime;
      if (SpawnTimer >= currentInterval) {
        SpawnPickups();
        SpawnTimer = 0;
        currentInterval = UnityEngine.Random.Range(minSpawnTime, maxSpawnTime);
      }
    }
  }
}
