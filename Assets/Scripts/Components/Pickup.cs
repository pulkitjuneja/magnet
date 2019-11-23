using System.Collections;
using UnityEngine;

public class Pickup : MonoBehaviour {

  public FloatValue gameSpeed;
  string name;
  Animator anim;
  AudioSource audio;
  void Start() {
    anim = GetComponent<Animator>();
    audio = GetComponent<AudioSource>();
  }

  void Update() {
    transform.Translate(Vector3.up * gameSpeed.value * Time.deltaTime, Space.World);
  }

  void OnTriggerEnter2D(Collider2D other) {
    if (other.gameObject.tag == "Player") {
      switch (gameObject.tag) {
        case "invincibility":
          other.gameObject.GetComponent<MagnetPowerupEffects>().beInvincible();
          break;
        case "DownSize":
          DownSize(other.gameObject);
          break;
        case "ReduceTime":
          ReduceTime();
          break;
      }
      anim.SetBool("Fade", true);
      if (audio.clip != null)
        AudioManager.play(audio, audio.clip);
    }
  }

  void OnTriggerExit2D(Collider2D other) {
    if (other.gameObject.tag == "Respawn") {
      StartCoroutine(DestroyAfterAudio());
    }
  }

  IEnumerator DestroyAfterAudio() {
    while (audio.isPlaying) {
      yield return null;
    }
    Destroy(this.gameObject);
  }

  void DownSize(GameObject player) {
    player.GetComponent<MagnetPowerupEffects>().downScale();
  }

  void ReduceTime() {
    gameSpeed.value = Mathf.Clamp(gameSpeed.value - 7.0f, 5, 18.0f);
  }
}