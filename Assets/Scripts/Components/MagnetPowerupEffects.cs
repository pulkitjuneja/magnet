using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetPowerupEffects : MonoBehaviour {

  public AudioSource audioSource;
  public FloatValue gameSpeed;
  public Magnet magnet;
  public ParticleSystem downScaleParticleEffect;
  public harmonicMotion fieldEffect;
  public Rigidbody2D rigidbody;
  public CircleCollider2D collider;
  public MagnetEnvInteraction fieldController;
  public AudioClip boostSound, timeSlow, absorb;
  public CircleCollider2D magneticField;

  SpriteRenderer boost;

  void Start() {
    boost = GetComponentsInChildren<SpriteRenderer>()[2];
    boost.enabled = false;
  }

  public void beInvincible() {
    StartCoroutine(Invincibility());
  }

  IEnumerator Invincibility() {
    float originalspeed = gameSpeed.value;
    magnet.ControlsDisabled = true;
    rigidbody.velocity = Vector2.zero;
    collider.enabled = false;
    magneticField.enabled = false;
    Camera.main.GetComponent<CameraJiggle>().startJiggle(0.05f, 1f);
    boost.enabled = true;
    gameSpeed.value = 23.0f;
    float time = Time.realtimeSinceStartup + 3.0f;
    while (Time.realtimeSinceStartup < time) {
      yield return null;
    }
    magnet.ControlsDisabled = false;
    gameSpeed.value = originalspeed;
    magnet.Toclamp = true;
    float recovertime = Time.realtimeSinceStartup + 2.0f;
    float flickerTimeDelt = 0.0f, flickerTime = 0.2f; // TODO: do this using animator in boost gameObject
    while (Time.realtimeSinceStartup < recovertime) {
      flickerTimeDelt += Time.deltaTime;
      if (flickerTimeDelt > flickerTime) {
        boost.enabled = !boost.enabled;
        flickerTimeDelt = 0.0f;
      }
      yield return null;
    }
    collider.enabled = true;
    magneticField.enabled = true;
    magnet.Toclamp = false;
    boost.enabled = false;
  }

  public void downScale() {
    StartCoroutine(downScaleAnimation());
  }

  public IEnumerator downScaleAnimation() {
    var initialFieldStartScale = fieldEffect.initialLocalScale;
    var initialFieldEndScale = fieldEffect.finalLocalScale;
    fieldEffect.initialLocalScale = fieldEffect.finalLocalScale = 0.01f;
    yield return new WaitForSeconds(0.25f);
    playDownScaleParticleSystem(transform.GetChild(1).localScale.x);
    foreach (Transform transform in this.transform) {
      var targetScale = Mathf.Clamp(transform.localScale.x - 0.7f, 1, 500);
      transform.localScale = new Vector3(targetScale, targetScale, 1);
    }
    yield return new WaitForSeconds(1);
    collider.radius = magnet.initialColliderRadius * transform.GetChild(1).localScale.x;
    fieldEffect.initialLocalScale = Mathf.Clamp(initialFieldStartScale - 0.7f, 0.8f, 500);
    fieldEffect.finalLocalScale = Mathf.Clamp(initialFieldEndScale - 0.7f, 1, 500);
    rigidbody.mass = Mathf.Clamp(rigidbody.mass - 0.5f, 1, 500);
    fieldController.modifyFieldRadius(1);
  }

  void playDownScaleParticleSystem(float localScale) {
    short particleCount = (short)(Mathf.Round((localScale - 0.9f) * 50));

    ParticleSystem.Burst[] bursts = new ParticleSystem.Burst[1];
    bursts[0] = new ParticleSystem.Burst(0.0f, particleCount, particleCount);
    downScaleParticleEffect.emission.SetBursts(bursts);
    downScaleParticleEffect.Play();
  }

  void OnTriggerEnter2D(Collider2D other) {
    if (other.gameObject.tag == "absorbable") {
      Destroy(other.gameObject);
      upscale();
      AudioManager.play(audioSource, absorb);
    } else if (other.gameObject.tag == "Respawn") { }
  }

  void upscale() {
    if (transform.GetChild(1).localScale.x < 1.7) {
      rigidbody.mass += 0.02f;
      foreach (Transform transform in this.transform) {
        transform.localScale += new Vector3(0.02f, 0.02f, 0f);
      }
      fieldController.modifyFieldRadius(1);
      fieldEffect.initialLocalScale += 0.02f;
      fieldEffect.finalLocalScale += 0.02f;
      collider.radius = magnet.initialColliderRadius * transform.GetChild(1).localScale.x;
    }
  }


}
