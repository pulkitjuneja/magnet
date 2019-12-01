using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

class UiManager : MonoBehaviour {
  public Sprite sfxOn, sfxOff, musicOn, musicOff;
  public Signal stateChangeSignal;
  public Signal screenChangeSignal;
  public Signal TogglePauseSignal;

  bool isSfxToggled = true;
  bool isMusicToggled = true;
  bool firstTimeStarted = false;

  List<GameObject> musicButtons;
  List<GameObject> SfxButtons;


  void Awake() {
    musicButtons = new List<GameObject>();
    SfxButtons = new List<GameObject>();
    if (!PlayerPrefs.HasKey(Constants.SCORE_KEY)) {
      firstTimeStarted = true;
    }
    Button[] buttons = GetComponentsInChildren<Button>(true);
    foreach (Button butt in buttons) {
      switch (butt.gameObject.name) {
        case "NewGame":
          butt.onClick.AddListener(() => NewGameListener());
          break;
        case "Exit":
          butt.onClick.AddListener(() => ExitListener());
          break;
        case "Pause":
          butt.onClick.AddListener(() => PauseListener());
          break;
        case "Resume":
          butt.onClick.AddListener(() => ResumeListener());
          break;
        case "ExitToMenu":
          butt.onClick.AddListener(() => ExitToMenu());
          break;
        case "toggleMusic":
          butt.onClick.AddListener(() => toggleMusic(butt));
          musicButtons.Add(butt.gameObject);
          break;
        case "toggleSfx":
          butt.onClick.AddListener(() => toggleSfx(butt));
          SfxButtons.Add(butt.gameObject);
          break;
        case "ExitHowToMenu":
          butt.onClick.AddListener(() => exitHowToMenu());
          break;
        case "toggleHowTo":
          butt.onClick.AddListener(() => showHowToMenu());
          break;
        case "ShowLeaderboard":
          butt.onClick.AddListener(() => ShowLeaderboard());
          break;
        case "RateUs":
          butt.onClick.AddListener(() => rateUs());
          break;
      }
    }
  }

  public void NewGameListener() {
    if (firstTimeStarted) {
      SignalData screenChangeData = new SignalData();
      screenChangeData.set("screenName", ScreenManager.Screens.HowToScreen);
      screenChangeData.set("firstTime", firstTimeStarted);
      screenChangeSignal.fire(screenChangeData);
      firstTimeStarted = false;
    } else {
      SignalData data = new SignalData();
      data.set("newStateType", GameStates.PlayingState);
      stateChangeSignal.fire(data);
    }
  }

  void ExitListener() {
    Application.Quit();
  }

  void PauseListener() {
    SignalData togglePauseData = new SignalData();
    togglePauseData.set("isPaused", true);
    TogglePauseSignal.fire(togglePauseData);
  }

  void ResumeListener() {
    SignalData togglePauseData = new SignalData();
    togglePauseData.set("isPaused", false);
    TogglePauseSignal.fire(togglePauseData);
  }

  void ExitToMenu() {
    Time.timeScale = 1.0f;
    SignalData data = new SignalData();
    data.set("newStateType", GameStates.MenuState);
    stateChangeSignal.fire(data);
  }

  void toggleMusic(Button butt) {
    isMusicToggled = !isMusicToggled;
    AudioManager.toggleMusic(isMusicToggled);
    musicButtons.ForEach((GameObject gameObject) => {
      if (isMusicToggled) {
        gameObject.GetComponent<Image>().overrideSprite = musicOn;
      } else {
        gameObject.GetComponent<Image>().overrideSprite = musicOff;
      }
    });
  }

  void exitHowToMenu() {
    SignalData screenChangeData = new SignalData();
    screenChangeData.set("screenName", ScreenManager.Screens.MenuScreen);
    screenChangeSignal.fire(screenChangeData);
  }

  void showHowToMenu() {
    SignalData screenChangeData = new SignalData();
    screenChangeData.set("screenName", ScreenManager.Screens.HowToScreen);
    screenChangeSignal.fire(screenChangeData);
  }

  void toggleSfx(Button butt) {
    isSfxToggled = !isSfxToggled;
    AudioManager.toggleSFX(isSfxToggled);
    SfxButtons.ForEach((GameObject gameObject) => {
      if (isSfxToggled) {
        gameObject.GetComponent<Image>().overrideSprite = sfxOn;
      } else {
        gameObject.GetComponent<Image>().overrideSprite = sfxOff;
      }
    });
  }

  void ShowLeaderboard() {
    GooglePlayServiceHelper.instance.ShowLeaderboard();
  }

  void rateUs() {
#if UNITY_ANDROID && !UNITY_EDITOR
        Application.OpenURL ("market://details?id=com.pipedreams.Magnetic");
#endif
  }

}