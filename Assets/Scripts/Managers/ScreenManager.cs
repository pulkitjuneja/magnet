using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenManager : MonoBehaviour {
  public enum Screens {
    MenuScreen,
    InGameUI,
    PauseScreen,
    HowToScreen,
    GameOverScreen
  }

  Screens currentScreenName;
  public Dictionary<Screens, Animator> screens;
  public Signal screenChangeSignal;

  void Awake() {
    screenChangeSignal.addListener(onScreenChange);
    screens = new Dictionary<Screens, Animator>();
    screens.Add(Screens.MenuScreen, GameObject.Find("Start Menu").GetComponent<Animator>());
    screens.Add(Screens.InGameUI, GameObject.Find("InGameUi").GetComponent<Animator>());
    screens.Add(Screens.PauseScreen, GameObject.Find("PauseMenu").GetComponent<Animator>());
    screens.Add(Screens.HowToScreen, GameObject.Find("HowToMenu").GetComponent<Animator>());
    screens.Add(Screens.GameOverScreen, GameObject.Find("End Menu").GetComponent<Animator>());
  }

  void Start() {
    HideAllScreens();
    screens[currentScreenName].SetBool("visible", true);
  }

  void onScreenChange(SignalData data) {
    Screens screenName = data.get<Screens>("screenName");
    bool firstTime = data.get<bool>("firstTime");
    Debug.Log(screenName);
    if (currentScreenName == screenName) {
      return;
    }
    screens[currentScreenName].SetBool("visible", false);
    screens[screenName].SetBool("visible", true);
    currentScreenName = screenName;
    if (screenName == Screens.HowToScreen) {
      if (firstTime) {
        screens[screenName].SetBool("VisibleFirstTime", true);
      } else {
        screens[screenName].SetBool("VisibleFirstTime", false);
      }
    } else if (screenName == Screens.GameOverScreen) {
      GameOverUiInfo info = data.get<GameOverUiInfo>("screenStateInfo");
      prepareGameOverScreen(info);
    }
  }

  void prepareGameOverScreen(GameOverUiInfo info) {
    Text FinalScore = GameObject.Find("FinalScore").GetComponent<Text>();
    Text PersonalBest = GameObject.Find("PersonalBest").GetComponent<Text>();
    Text PersonalBestLabel = GameObject.Find("PersonalBestLabel").GetComponent<Text>();
    Button ShareButton = GameObject.Find("PersonalBest").GetComponent<Button>();

    FinalScore.text = info.scoreText;
    PersonalBest.text = info.personalBestText;
    PersonalBestLabel.text = info.personalBestLabelText;

    if (info.isPersonalBest)
      ShareButton.onClick.AddListener(info.shareButtonCallBack);

  }

  void HideAllScreens() {
    foreach (KeyValuePair<Screens, Animator> screen in screens) {
      screen.Value.SetBool("visible", false);
    }
  }

  void Update() {

  }
}
