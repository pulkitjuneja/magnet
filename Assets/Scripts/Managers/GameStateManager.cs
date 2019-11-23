using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class GameStateManager : MonoBehaviour {
  public GameStateMachine fsm;

  public FloatValue gameSpeed;

  [HideInInspector]
  public static string SCORE_KEY = "BestScore";
  public GameObject MagnetPrefab;
  public Spawner[] spawners;
  public GameObject[] powerups;
  public GameObject[] levels;

  void Awake() {
    fsm = GameStateMachine.GetNewInstance(null, this);
  }

  void Start() {
    Camera camera = Camera.main;
    AudioManager.musicSource = camera.GetComponent<AudioSource>();
    this.transform.position = new Vector3(transform.position.x, (camera.transform.position.y + camera.orthographicSize) - 0.05f);
    StartCoroutine(fsm.run());
    GooglePlayServiceHelper.instance.SignIn();
  }

  void OnTriggerExit2D(Collider2D other) {
    fsm.TriggerExit2D(other);
  }

  void OnGUI() {
    fsm.OnGUI();
  }
}