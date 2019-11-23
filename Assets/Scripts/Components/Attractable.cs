using UnityEngine;
using System.Collections;

public class Attractable : MonoBehaviour {
  public float weight;
  public Rigidbody2D rigidbody;

  string fieldObjectName = "magnetic field";
  bool inMagnetRange = false;
  Transform parent;
  Transform magnetLocation;
  float destroyTimer = 0.0f;


  void Start() {
    rigidbody = GetComponent<Rigidbody2D>();
    parent = transform.parent;
  }
  void FixedUpdate() {
    if (inMagnetRange && magnetLocation) {
      Vector3 direction = magnetLocation.position - transform.position;
      float magnetDistance = Vector3.Distance(magnetLocation.position, transform.position);
      float magnetstr = (MagnetEnvInteraction.CurrentFieldRadius / magnetDistance) * 500;
      rigidbody.AddForce(direction * magnetstr, ForceMode2D.Force);
    }
    if (destroyTimer > 0) {
      destroyTimer -= Time.deltaTime;
      if (destroyTimer <= 0) {
        Destroy(this.gameObject);
      }
    }
  }

  void OnTriggerEnter2D(Collider2D other) {
    if (other.gameObject.name == fieldObjectName) {
      transform.parent = null;
      rigidbody.velocity = new Vector2(0, 0);
      rigidbody.isKinematic = false;
      magnetLocation = other.transform;
      inMagnetRange = true;
      destroyTimer = 3.0f;
    }
  }
  void OnTriggerExit2D(Collider2D other) {
    if (other.gameObject.name == fieldObjectName) {
      rigidbody.isKinematic = true;
      Destroy(this.gameObject);
    }
  }
}