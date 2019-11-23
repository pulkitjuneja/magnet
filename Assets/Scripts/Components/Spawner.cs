using System.Collections;
using UnityEngine;

public class Spawner : MonoBehaviour {

  public int collisionCount = 0;
  public GameObject ToSpawn = null;

  void OnTriggerEnter2D(Collider2D other) {
    if (other.gameObject.tag != "level" && other.gameObject.tag != "absorbable") {
      collisionCount++;
    }
  }
  void OnTriggerExit2D(Collider2D other) {
    if (other.gameObject.tag != "level" && other.gameObject.tag != "absorbable") {
      collisionCount--;
      if (collisionCount == 0 && ToSpawn != null) {
        Spawn(ToSpawn);
        ToSpawn = null;
      }
    }
  }

  public void Spawn(GameObject spawn) {
    if (collisionCount == 0) {
      var pos = this.transform.position;
      GameObject pickup = Instantiate(spawn, pos, Quaternion.identity) as GameObject;
    } else {
      ToSpawn = spawn;
    }
  }
}