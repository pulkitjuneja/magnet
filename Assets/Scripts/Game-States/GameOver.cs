using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class GameOverState : State<MainStateMachine> {

    GameObject EndMenu ; 
    Text FinalScore ;
    public GameOverState(MainStateMachine m, int Score) : base(m) {
        string NewBest = "";
        EndMenu = GameObject.Find("Canvas").transform.FindChild("End Menu").gameObject ;
        FinalScore = EndMenu.transform.FindChild("FinalScore").GetComponent<Text>();
        EndMenu.SetActive(true);
        if (Score > PlayerPrefs.GetInt(FSMgenerator.SCORE_KEY)) {
            PlayerPrefs.SetInt(FSMgenerator.SCORE_KEY, Score);
            PlayerPrefs.Save();
            NewBest = "New Best";
        }
        FinalScore.text = "You Fell\r\n" + Score.ToString() + "m\r\n" + NewBest;
    }

    public override IEnumerator run() {
        while (ParentMachine.Current.GetType() == GetType()) {
            yield return null;
        }
        ToggleEndMenu(false);
        ResetGame();
    }
    void ToggleEndMenu(bool visible) {
        EndMenu.SetActive(visible);
    }

    void ResetGame() {
        harmonicMotion.temp = 0;
        GamePlay.gamespeed = 5;
    }

}