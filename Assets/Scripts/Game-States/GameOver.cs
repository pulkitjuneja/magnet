using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class GameOverState : State<MainStateMachine> {
    public GameOverState(MainStateMachine m, int Score) : base(m) {
        string NewBest = "";
        ParentMachine.Component.EndMenu.SetActive(true);
        if (Score > PlayerPrefs.GetInt(FSMgenerator.SCORE_KEY)) {
            PlayerPrefs.SetInt(FSMgenerator.SCORE_KEY, Score);
            PlayerPrefs.Save();
            NewBest = "New Best";
        }
        ParentMachine.Component.FinalScore.text = "You Fell\r\n" + Score.ToString() + "m\r\n" + NewBest;
    }

    public override IEnumerator run() {
        while (ParentMachine.Current.GetType() == GetType()) {
            yield return null;
        }
        ToggleEndMenu(false);
        ResetGame();
    }
    void ToggleEndMenu(bool x) {
        ParentMachine.Component.EndMenu.SetActive(x);
    }

    void ResetGame() {
        harmonicMotion.temp = 0;
        GamePlay.gamespeed = 5;
    }

}