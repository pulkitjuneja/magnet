using UnityEngine;
using System;
using System.Collections;


public class Magnet : MonoBehaviour {

  public Rigidbody2D rigidbody;
  public AudioClip absorb;
  bool Toclamp = false;
  public AudioClip hit, boostSound, timeSlow;
  CircleCollider2D collider;
  CircleCollider2D magField;
  SpriteRenderer boost;
  bool ControlsDisabled = false;
  AudioSource audio;
  public event Action GameOverEvent;
  public harmonicMotion fieldEffect;
  Animator animator;

  MagEnvInteraction fieldController;
  float TouchSeperator;
  float initialColliderRadius ;

  void Start() {

    animator = GetComponent<Animator>();
    audio = GetComponent<AudioSource>();
    fieldEffect= GetComponentInChildren<harmonicMotion>();
    collider = GetComponent<CircleCollider2D>();
    TouchSeperator = Screen.width / 2;
    fieldController = GetComponentInChildren<MagEnvInteraction>();
    magField = GetComponentsInChildren<CircleCollider2D>()[1];
    boost = GetComponentsInChildren<SpriteRenderer>()[2];
    rigidbody = GetComponent<Rigidbody2D>();
    boost.enabled = false;
    initialColliderRadius = collider.radius;
  }

  void Awake() {
    GameObject[] magnets = GameObject.FindGameObjectsWithTag("Player");
    if (magnets.Length > 1) {
      Destroy(this.gameObject);
    }
  }

  void Update() {
    if (!ControlsDisabled) {
#if UNITY_STANDALONE || UNITY_WEBPLAYER
            if (Input.GetKey(KeyCode.A))
                rigidbody.AddForce(new Vector2(-50, 0));
            if (Input.GetKey(KeyCode.D))
                rigidbody.AddForce(new Vector2(50, 0));
#else
      if (Input.touchCount > 0) {
        Touch touch = Input.touches[0];
        if (touch.phase == TouchPhase.Stationary) {
          if (touch.position.x < TouchSeperator)
            rigidbody.AddForce(new Vector2(-2000 * Time.deltaTime, 0));
          else if (touch.position.x >= TouchSeperator)
            rigidbody.AddForce(new Vector2(2000 * Time.deltaTime, 0));
        }
      }
#endif
    }
    if (Toclamp)
      transform.position = new Vector3(Mathf.Clamp(transform.position.x, -5.0f, 5.0f), transform.position.y);
  }

  void OnTriggerEnter2D(Collider2D other) {
    if (other.gameObject.tag == "absorbable") {
      Destroy(other.gameObject);
      upscale();
      AudioManager.play(audio, absorb);
    } else if (other.gameObject.tag == "Respawn") {
      Debug.Log("error");
    }
  }
  void OnCollisionEnter2D(Collision2D other) {
    Debug.Log("collided");
    rigidbody.velocity = Vector2.zero;
    rigidbody.isKinematic = true;
    transform.parent = other.gameObject.transform;
    AudioManager.play(audio, hit);
  }

  public void beInvincible() {
    StartCoroutine(Invincibility());
  }


  IEnumerator Invincibility() {
    float originalspeed = GamePlay.gamespeed;
    GamePlay.gamespeed = 0.5f;
    AudioManager.play(audio, timeSlow);
    ControlsDisabled = true;
    rigidbody.velocity = Vector2.zero;
    float time1 = Time.realtimeSinceStartup + 1.0f;
    while (Time.realtimeSinceStartup < time1) {
      yield return null;
    }
    rigidbody.velocity = Vector2.zero;
    collider.enabled = false;
    magField.enabled = false;
    Camera.main.GetComponent<CameraJiggle>().jiggleCam(0.05f, 1f);
    boost.enabled = true;
    GamePlay.gamespeed = 23.0f;
    AudioManager.play(audio, boostSound);
    float time = Time.realtimeSinceStartup + 3.0f;
    while (Time.realtimeSinceStartup < time) {
      yield return null;
    }
    ControlsDisabled = false;
    GamePlay.gamespeed = originalspeed;
    Toclamp = true;
    float recovertime = Time.realtimeSinceStartup + 2.0f;
    float flickerTimeDelt = 0.0f, flickerTime = 0.2f; // TODO: do this using animator in boost gameObject
    while (Time.realtimeSinceStartup < recovertime) {
      flickerTimeDelt += Time.deltaTime;
      Debug.Log("here");
      if (flickerTimeDelt > flickerTime) {
        boost.enabled = !boost.enabled;
        flickerTimeDelt = 0.0f;
      }
      yield return null;
    }
    collider.enabled = true;
    magField.enabled = true;
    Toclamp = false;
    boost.enabled = false;
  }

  void OnDestroy() {
    GameObject[] magnets = GameObject.FindGameObjectsWithTag("Player");
    if (magnets.Length == 0) {
      Debug.Log("magnet destroyed");
      if (GameOverEvent != null)
        GameOverEvent();
    }
  }

  void upscale() {
    rigidbody.mass += 0.05f;
    foreach (Transform transform in this.transform) {
      transform.localScale += new Vector3(0.05f, 0.05f, 0f);
    }
    fieldController.modifyFieldRadius(1);
    fieldEffect.initialLocalScale += 0.05f;
    fieldEffect.finalLocalScale += 0.05f;
    collider.radius = initialColliderRadius*transform.GetChild(0).localScale.x;
  }
public void downScale () {
    foreach (Transform transform in this.transform) {
       var targetScale = Mathf.Clamp(transform.localScale.x - 0.5f,1,500); 
       transform.localScale = new Vector3(targetScale, targetScale, 1);
    }
    collider.radius = initialColliderRadius*transform.GetChild(0).localScale.x;
    fieldEffect.initialLocalScale = Mathf.Clamp(fieldEffect.initialLocalScale - 0.5f, 1, 500);
    fieldEffect.finalLocalScale = Mathf.Clamp(fieldEffect.finalLocalScale - 0.5f, 1, 500);
    rigidbody.mass = Mathf.Clamp(rigidbody.mass - 0.5f, 1, 500);   
fieldController.modifyFieldRadius(1); 
  }

  public void reset() {
      foreach (Transform transform in this.transform) {
       transform.localScale = new Vector3(1, 1, 1);
    }
    collider.radius = initialColliderRadius;
    fieldEffect.initialLocalScale = 0.8f ; 
    fieldEffect.finalLocalScale = 1.0f;
    rigidbody.mass = 1;  //TODO get values from pre stored variables for all these
    fieldController.modifyFieldRadius(0);
  }

}

