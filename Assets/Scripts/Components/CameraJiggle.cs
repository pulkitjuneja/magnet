using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraJiggle : MonoBehaviour {

  float jiggleAmount = 0.0f;
  float jiggleTimer;

  Vector3 basePosition;

  void Start() {
    basePosition = transform.position;
  }

  void Update() {
    if (jiggleTimer > 0) {
      float quakeAmt = Random.value * jiggleAmount * 2 - jiggleAmount;
      Vector3 framePosition = transform.position;
      framePosition.x += quakeAmt;
      transform.position = framePosition;
      jiggleTimer -= Time.deltaTime;
    } else if (jiggleAmount > 0) {
      transform.position = basePosition;
      jiggleAmount = 0;
    }
  }

  public void startJiggle(float amt, float duration) {
    jiggleAmount = amt;
    jiggleTimer = duration;
  }
}