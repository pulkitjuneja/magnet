using System;
using System.Collections;
using UnityEngine;

public class Magnet : MonoBehaviour {

  public Rigidbody2D rigidbody;
  public FloatValue gameSpeed;
  public AudioClip hit;
  public event Action GameOverEvent;
  public harmonicMotion fieldEffect;
  public bool ControlsDisabled = false;
  public bool Toclamp = false;
  public Animator animator;
  public AudioSource audioSource;
  public CircleCollider2D collider;
  public MagnetEnvInteraction fieldController;
  [HideInInspector] public float initialColliderRadius;

  float TouchSeperator;

  void Start() {
    TouchSeperator = Screen.width / 2;
    initialColliderRadius = collider.radius;
  }

  void Awake() {
    GameObject[] magnets = GameObject.FindGameObjectsWithTag("Player");
    if (magnets.Length > 1) {
      Destroy(this.gameObject);
    }
  }

  void FixedUpdate() {
    move();
  }

  void move() {
    if (!ControlsDisabled) {
      // #if UNITY_STANDALONE || UNITY_WEBPLAYER
      if (Input.GetKey(KeyCode.A))
        rigidbody.AddForce(new Vector2(-50, 0));
      if (Input.GetKey(KeyCode.D))
        rigidbody.AddForce(new Vector2(50, 0));
      // #else
      //       if (Input.touchCount > 0) {
      //         Touch touch = Input.GetTouch(0);
      //         if (touch.phase == TouchPhase.Stationary) {
      //           if (touch.position.x < TouchSeperator)
      //             rigidbody.AddForce(new Vector2(-2000 * Time.deltaTime, 0));
      //           else if (touch.position.x >= TouchSeperator)
      //             rigidbody.AddForce(new Vector2(2000 * Time.deltaTime, 0));
      //         }
      //       }
      // #endif
    }
    if (Toclamp)
      transform.position = new Vector3(Mathf.Clamp(transform.position.x, -5.0f, 5.0f), transform.position.y);
  }

  void OnCollisionEnter2D(Collision2D other) {
    rigidbody.velocity = Vector2.zero;
    rigidbody.isKinematic = true;
    transform.parent = other.gameObject.transform;
    AudioManager.play(audioSource, hit);
  }


  void OnDestroy() {
    GameObject[] magnets = GameObject.FindGameObjectsWithTag("Player");
    if (magnets.Length == 0) {
      if (GameOverEvent != null) { //TODO replace with signal
        GameOverEvent();
      }
    }
  }

  public void reset(Camera camera) {
    foreach (Transform transform in this.transform) {
      transform.localScale = new Vector3(1, 1, 1);
    }
    collider.radius = initialColliderRadius;
    fieldEffect.initialLocalScale = 0.8f;
    fieldEffect.finalLocalScale = 1.0f;
    rigidbody.mass = 1;
    rigidbody.velocity = Vector2.zero;
    fieldController.modifyFieldRadius(0);
    transform.position = new Vector3(camera.transform.position.x, camera.transform.position.y + camera.orthographicSize / 2);
  }

}