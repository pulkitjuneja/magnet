using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraJiggle : MonoBehaviour {

    float jiggleAmt = 0.0f;

    void Update() {
        if (jiggleAmt > 0) {
            float quakeAmt = Random.value * jiggleAmt * 2 - jiggleAmt;
            Vector3 pp = transform.position;
            pp.x += quakeAmt;
            transform.position = pp;
        }
    }

    public void jiggleCam(float amt, float duration) {
        jiggleAmt = amt;
        StartCoroutine(jiggleCam2(duration));
    }

    IEnumerator jiggleCam2(float duration) {
        yield return new WaitForSeconds(duration);
        jiggleAmt = 0;
    }
}
