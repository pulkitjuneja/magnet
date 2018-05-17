using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameOverState : State<MainStateMachine> {

    Animator EndMenu;
    Text FinalScore, PersonalBest, PersonalBestText;
    public GameOverState (MainStateMachine m, int Score) : base (m) {
        EndMenu = GameObject.Find ("Canvas").transform.Find ("End Menu").gameObject.GetComponent<Animator>();
        FinalScore = EndMenu.transform.Find ("FinalScore").GetComponent<Text> ();
        PersonalBest = EndMenu.transform.Find ("PersonalBest").GetComponent<Text> ();
        PersonalBestText = EndMenu.transform.Find ("PersonalBestText").GetComponent<Text> ();
        EndMenu.SetBool("visible",true);
        if (Score > PlayerPrefs.GetInt (FSMgenerator.SCORE_KEY)) {
            PlayerPrefs.SetInt (FSMgenerator.SCORE_KEY, Score);
            PlayerPrefs.Save ();
            prepareScreenForNewPersonalBest();
        } else {
            int personalBest = PlayerPrefs.GetInt (FSMgenerator.SCORE_KEY);
            prepareScreenForNormalScore(personalBest);
        }
        FinalScore.text = Score.ToString () + "m";
    }

    void prepareScreenForNewPersonalBest() {
        PersonalBestText.text = "New Personal Best";
        PersonalBest.text = "";
    }

    void prepareScreenForNormalScore (int personalBestScore) {
        PersonalBestText.text = "Personal Best";
        PersonalBest.text = personalBestScore.ToString() + "m";
    }

    public override IEnumerator run () {
        while (ParentMachine.Current.GetType () == GetType ()) {
            yield return null;
        }
        EndMenu.SetBool("visible",false);
    }

}