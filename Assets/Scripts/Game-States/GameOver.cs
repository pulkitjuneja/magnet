using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameOverState : State<MainStateMachine> {

    Animator EndMenu;
    Text FinalScore,
    PersonalBest,
    PersonalBestText;

    int currentScore,
    personalBestScore;
    public GameOverState (MainStateMachine m, int Score) : base (m) {
        EndMenu = GameObject.Find ("Canvas").transform.Find ("End Menu").gameObject.GetComponent<Animator> ();
        FinalScore = EndMenu.transform.Find ("FinalScore").GetComponent<Text> ();
        PersonalBest = EndMenu.transform.Find ("PersonalBest").GetComponent<Text> ();
        PersonalBestText = EndMenu.transform.Find ("PersonalBestText").GetComponent<Text> ();
        currentScore = Score;
    }

    void PrepareScreenForNewPersonalBest () {
        PersonalBestText.text = "New Personal Best";
        PersonalBest.text = "";
    }

    void PrepareScreenForNormalScore (int personalBestScore) {
        PersonalBestText.text = "Personal Best";
        PersonalBest.text = personalBestScore.ToString () + "m";
    }

    void FetchScoreAndPrepareUI () {
        personalBestScore = PlayerPrefs.GetInt (FSMgenerator.SCORE_KEY);
        if (currentScore > personalBestScore) {
            PlayerPrefs.SetInt (FSMgenerator.SCORE_KEY, currentScore);
            GooglePlayServiceHelper.instance.AddScoreToLeaderboard (GPGSIds.leaderboard_magnetic, (long) currentScore);
            PlayerPrefs.Save ();
            PrepareScreenForNewPersonalBest ();
        } else {
            PrepareScreenForNormalScore (personalBestScore);
        }
        FinalScore.text = currentScore.ToString () + "m";
    }

    public override IEnumerator run () {
        EndMenu.SetBool ("visible", true);
        FetchScoreAndPrepareUI ();
        while (ParentMachine.Current.GetType () == GetType ()) {
            yield return null;
        }
        EndMenu.SetBool ("visible", false);
    }

}