using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

class UiManager : MonoBehaviour {
    bool isSfxToggled = true;
    bool isMusicToggled = true;

    List<GameObject> musicButtons, SfxButtons;

    public Sprite sfxOn, sfxOff, musicOn, musicOff;
    void Start () {
        musicButtons = new List<GameObject> ();
        SfxButtons = new List<GameObject> ();
        Button[] buttons = GetComponentsInChildren<Button> (true);
        foreach (Button butt in buttons) {
            switch (butt.gameObject.name) {
                case "NewGame":
                    butt.onClick.AddListener (() => NewGameListener ());
                    break;
                case "Exit":
                    butt.onClick.AddListener (() => ExitListener ());
                    break;
                case "Pause":
                    butt.onClick.AddListener (() => PauseListener ());
                    break;
                case "Resume":
                    butt.onClick.AddListener (() => ResumeListener ());
                    break;
                case "ExitToMenu":
                    butt.onClick.AddListener (() => ExitToMenu ());
                    break;
                case "toggleMusic":
                    butt.onClick.AddListener (() => toggleMusic (butt));
                    musicButtons.Add (butt.gameObject);
                    break;
                case "toggleSfx":
                    butt.onClick.AddListener (() => toggleSfx (butt));
                    SfxButtons.Add (butt.gameObject);
                    break;
                case "ExitHowToMenu":
                    butt.onClick.AddListener (() => exitHowToMenu ());
                    break;
                case "toggleHowTo":
                    butt.onClick.AddListener (() => showHowToMenu ());
                    break;
                case "ShowLeaderboard":
                    butt.onClick.AddListener (() => ShowLeaderboard ());
                    break;
            }
        }
        Debug.Log (musicButtons.Count);
        Debug.Log (SfxButtons.Count);
    }

    public void NewGameListener () {
        if (MainStateMachine.instance.Current.GetType () == typeof (MenuState)) {
            (MainStateMachine.instance.Current as MenuState).navigateToPlay ();
        } else {
            MainStateMachine.instance.SetState (typeof (GamePlay), false, new object[] { MainStateMachine.instance });
        }
    }

    void ExitListener () {
        Application.Quit ();
    }

    void PauseListener () {
        (MainStateMachine.instance.Current as GamePlay).onPause ();
    }

    void ResumeListener () {
        (MainStateMachine.instance.Current as GamePlay).onResume ();
    }

    void ExitToMenu () {
        Time.timeScale = 1.0f;
        MainStateMachine.instance.SetState (typeof (MenuState), false, new object[] { MainStateMachine.instance });
    }

    void toggleMusic (Button butt) {
        isMusicToggled = !isMusicToggled;
        AudioManager.toggleMusic (isMusicToggled);
        musicButtons.ForEach ((GameObject gameObject) => {
            if (isMusicToggled) {
                gameObject.GetComponent<Image> ().overrideSprite = musicOn;
            } else {
                gameObject.GetComponent<Image> ().overrideSprite = musicOff;
            }
        });
    }

    void exitHowToMenu () {
        (MainStateMachine.instance.Current as MenuState).hideHowTo ();
    }

    void showHowToMenu () {
        (MainStateMachine.instance.Current as MenuState).showHowTo ();
    }

    void toggleSfx (Button butt) {
        isSfxToggled = !isSfxToggled;
        AudioManager.toggleSFX (isSfxToggled);
        SfxButtons.ForEach ((GameObject gameObject) => {
            if (isSfxToggled) {
                gameObject.GetComponent<Image> ().overrideSprite = sfxOn;
            } else {
                gameObject.GetComponent<Image> ().overrideSprite = sfxOff;
            }
        });
    }

    void ShowLeaderboard () {
        GooglePlayServiceHelper.instance.ShowLeaderboard ();
    }

}