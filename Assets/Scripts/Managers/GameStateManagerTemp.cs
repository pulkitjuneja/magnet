using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public enum GameStates {
  MenuState,
  PlayingState,
  PausedState,
  GameOverState
}

public class GameStateManagerTemp : MonoBehaviour {
  public Signal stateChangeSignal;
  public Signal screenChangeSignal;
  public Signal levelCompleteSignal;
  public Signal TogglePauseSignal;
  public GameObject MagnetPrefab;
  public FloatValue gameSpeed;
  public Text scoreBoard;
  [HideInInspector] public int gameScore;

  SimpleState<GameStateManagerTemp> currentState;
  GameStates currentStateType;
  SimpleState<GameStateManagerTemp> transitionToState;

  IEnumerator Start() {
    stateChangeSignal.addListener(onStateChanged);
    Initialize();
    while (currentState != null) {
      if (transitionToState != null) {
        currentState.transitionOutOf();
        transitionToState.transitionInto();
        currentState = transitionToState;
        transitionToState = null;
      }
      yield return StartCoroutine(currentState.update());
    }
  }

  void Initialize() {
    if (!PlayerPrefs.HasKey(Constants.SCORE_KEY)) {
      PlayerPrefs.SetInt(Constants.SCORE_KEY, 0);
      PlayerPrefs.Save();
    }
    //set this collider to top of camera
    this.transform.position = new Vector3(transform.position.x, (Camera.main.transform.position.y + Camera.main.orthographicSize) - 0.05f);

    // initialize to menu state
    currentState = new SimpleMenuState(this);
    currentState.transitionInto();

    AudioManager.musicSource = Camera.main.GetComponent<AudioSource>();
    GooglePlayServiceHelper.instance.SignIn();
  }

  void onStateChanged(SignalData data) {
    GameStates newStateType = data.get<GameStates>("newStateType");
    if (currentStateType == newStateType) {
      return;
    } else {
      switch (newStateType) {
        case GameStates.MenuState: transitionToState = new SimpleMenuState(this); break;
        case GameStates.PlayingState: transitionToState = new SimpleGameplayState(this); break;
        case GameStates.GameOverState: transitionToState = new SimpleGameoverState(this); break;
      }
      currentStateType = newStateType;
    }
  }

  IEnumerator menuState() {
    yield return null;
  }
}