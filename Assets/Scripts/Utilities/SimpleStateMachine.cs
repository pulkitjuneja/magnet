using System.Collections;
using System;
using UnityEngine;
using UnityEngine.UI;

// public class SimpleStateMachine {
//   SimpleState<GameStateManagerTemp> CurrentState;
//   bool Transitioning;
//   GameStateManagerTemp component;

//   public SimpleStateMachine(SimpleState<GameStateManagerTemp> initialState, GameStateManagerTemp component) {
//     CurrentState = initialState;
//     this.component = component;

//   }

//   public IEnumerator Start() {
//     Debug.Log(CurrentState);
//     while (CurrentState != null) {
//       Transitioning = false;
//       yield return Component.StartCoroutine(CurrentState);
//     }
//   }

//   public void SetState(IEnumerator newState) {
//     this.CurrentState = newState;
//     Transitioning = true;
//   }
// }


public abstract class SimpleState<T> where T : MonoBehaviour {

  protected T context;

  public SimpleState(T context) {
    this.context = context;
  }

  public abstract void transitionInto();

  public abstract void transitionOutOf();

  public abstract IEnumerator update();

}






class SimpleMenuState : SimpleState<GameStateManagerTemp> {

  public SimpleMenuState(GameStateManagerTemp context) : base(context) { }
  public override void transitionInto() {
    SignalData screenChangeSignalData = new SignalData();
    Camera camera = Camera.main;
    screenChangeSignalData.set("screenName", ScreenManager.Screens.MenuScreen);
    context.screenChangeSignal.fire(screenChangeSignalData);
    GameObject magnet = GameObject.FindGameObjectWithTag("Player");
    Vector3 magnetResetPosition = new Vector3(camera.transform.position.x, camera.transform.position.y + camera.orthographicSize / 2);
    if (magnet == null) {
      magnet = GameObject.Instantiate(context.MagnetPrefab, magnetResetPosition, Quaternion.identity) as GameObject;
    } else {
      magnet.GetComponent<Magnet>().reset(magnetResetPosition);
      magnet.transform.position = magnetResetPosition;
    }

    magnet.GetComponent<Magnet>().ControlsDisabled = true;
  }

  public override void transitionOutOf() { }

  public override IEnumerator update() {
    yield return null;
  }
}







class SimpleGameplayState : SimpleState<GameStateManagerTemp> {

  bool isPaused;
  float elapsedTime;

  public SimpleGameplayState(GameStateManagerTemp context) : base(context) { }
  public override void transitionInto() {
    context.levelCompleteSignal.addListener(onLevelComplete);
    context.TogglePauseSignal.addListener(onPause);

    SignalData screenChangeSignalData = new SignalData();
    screenChangeSignalData.set("screenName", ScreenManager.Screens.InGameUI);
    context.screenChangeSignal.fire(screenChangeSignalData);

    GameObject magnet = GameObject.FindGameObjectWithTag("Player");
    if (magnet == null) {
      magnet = GameObject.Instantiate(context.MagnetPrefab, new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y + Camera.main.orthographicSize / 2), Quaternion.identity) as GameObject;
    }
    magnet.GetComponent<Magnet>().ControlsDisabled = false;

    context.gameScore = 0;
    context.scoreBoard.text = context.gameScore.ToString();
    context.gameSpeed.value = 5.0f;
  }

  public override void transitionOutOf() {
    context.levelCompleteSignal.removeListener(onLevelComplete);
    context.TogglePauseSignal.removeListener(onPause);
    context.scoreBoard.text = "0";
  }

  public override IEnumerator update() {
    elapsedTime += Time.unscaledDeltaTime;
    if (elapsedTime >= 3.0f && context.gameSpeed.value < 12.0f) {
      context.gameSpeed.value += 0.5f;
      elapsedTime = 0;
    }
    yield return null;
  }

  public void onPause(SignalData data) {
    bool isPaused = data.get<bool>("isPaused");
    this.isPaused = isPaused;
    if (isPaused) {
      SignalData screenChangeSignalData = new SignalData();
      screenChangeSignalData.set("screenName", ScreenManager.Screens.PauseScreen);
      context.screenChangeSignal.fire(screenChangeSignalData);
      Time.timeScale = 0;
    } else {
      Time.timeScale = 1;
      SignalData screenChangeSignalData = new SignalData();
      screenChangeSignalData.set("screenName", ScreenManager.Screens.InGameUI);
      context.screenChangeSignal.fire(screenChangeSignalData);
    }
  }
  void onLevelComplete(SignalData data) {
    context.gameScore++;
    context.scoreBoard.text = context.gameScore.ToString();
  }
}







class SimpleGameoverState : SimpleState<GameStateManagerTemp> {

  public SimpleGameoverState(GameStateManagerTemp context) : base(context) { }
  public override void transitionInto() {
    int currentScore = context.gameScore;
    int personalBestScore = PlayerPrefs.GetInt(Constants.SCORE_KEY);
    if (currentScore > personalBestScore) {
      PlayerPrefs.SetInt(GameStateManager.SCORE_KEY, currentScore);
      GooglePlayServiceHelper.instance.AddScoreToLeaderboard(GPGSIds.leaderboard_magnetic, (long)currentScore);
      PlayerPrefs.Save();
      PrepareScreenForNewPersonalBest();
    } else {
      PrepareScreenForNormalScore(personalBestScore);
    }
  }

  public override void transitionOutOf() { }

  public override IEnumerator update() {
    yield return null;
  }
  void shareScoreAndroid() {
    string score = context.gameScore.ToString();
    AndroidJavaClass intentClass = new AndroidJavaClass("android.content.Intent");
    AndroidJavaObject intentObject = new AndroidJavaObject("android.content.Intent");
    intentObject.Call("setAction", intentClass.GetStatic<String>("ACTION_SEND"));
    intentObject.Call<AndroidJavaObject>("setType", "text/plain");
    intentObject.Call("putExtra", intentClass.GetStatic<String>("EXTRA_SUBJECT"), "Magnetic");
    intentObject.Call("putExtra", intentClass.GetStatic<String>("EXTRA_TITLE"), "New Best Score");
    intentObject.Call("putExtra", intentClass.GetStatic<String>("EXTRA_TEXT"),
        "I just fell " + score + " meters in magnetic, Think you can beat it ? https://play.google.com/apps/testing/com.pipedreams.Magnet");
    AndroidJavaClass unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
    AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject>("currentActivity");
    currentActivity.Call("startActivity", intentObject);
  }

  void PrepareScreenForNewPersonalBest() {
    GameOverUiInfo info = new GameOverUiInfo();
    info.isPersonalBest = true;
    info.personalBestLabelText = "New Personal Best";
    info.personalBestText = "Share";
    info.scoreText = context.gameScore.ToString();
    info.shareButtonCallBack = shareScoreAndroid;
    SignalData data = new SignalData();
    data.set("screenName", ScreenManager.Screens.GameOverScreen);
    data.set("screenStateInfo", info);
    context.screenChangeSignal.fire(data);
  }

  void PrepareScreenForNormalScore(int personalBestScore) {
    GameOverUiInfo info = new GameOverUiInfo();
    info.isPersonalBest = false;
    info.personalBestLabelText = "Personal Best";
    info.scoreText = context.gameScore.ToString();
    info.personalBestText = personalBestScore.ToString() + "m";
    SignalData data = new SignalData();
    data.set("screenName", ScreenManager.Screens.GameOverScreen);
    data.set("screenStateInfo", info);
    context.screenChangeSignal.fire(data);
  }

}


