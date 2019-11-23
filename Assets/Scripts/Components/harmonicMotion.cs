using UnityEngine;
using System.Collections;

public class harmonicMotion : MonoBehaviour {

    public static float temp = 0;
    public float fadeDuration;
    public float initialLocalScale, finalLocalScale, initialOpacity, finalOpacity;
    float scale, opacity, startTime;
    private SpriteRenderer spriteRenderer;

    void Start() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        transform.localScale = new Vector3(initialLocalScale, initialLocalScale, 1);
        spriteRenderer.color = new Color(1f, 1f, 1f, initialOpacity);
        startTime = Time.time;

    }

    void Update() {
        if (!spriteRenderer.enabled)
            spriteRenderer.enabled = true;
        float t = (Time.time - startTime) / fadeDuration;
        scale = Mathf.SmoothStep(initialLocalScale, finalLocalScale, t);
        opacity = Mathf.SmoothStep(initialOpacity, finalOpacity, t);
        transform.localScale = new Vector3(scale, scale, 1);
        spriteRenderer.color = new Color(1f, 1f, 1f, opacity);
        if (Time.time - startTime > fadeDuration) {
            startTime = Time.time;
        }
    }
}