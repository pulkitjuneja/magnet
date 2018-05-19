using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MenuState : GameRunning {

    Animator startScreenAnimator;
    GameObject HowToScreen;
    bool isFirstTime = false;
    GameObject instructionsExitToMenu, instructionsNewGame;
    MainStateMachine stateMachine;
    public MenuState (MainStateMachine g) : base (g) {
        startScreenAnimator = GameObject.Find ("Start Menu").GetComponent<Animator> ();
        stateMachine = g ;
        // TODO : use animator instead of enabling the screen by default by always
        HowToScreen = GameObject.Find ("HowToMenu");
        instructionsExitToMenu = HowToScreen.transform.Find("ExitHowToMenu").gameObject;
        instructionsNewGame = HowToScreen.transform.Find("NewGame").gameObject;
        HowToScreen.SetActive (false);
        if (!PlayerPrefs.HasKey (FSMgenerator.SCORE_KEY)) {
            isFirstTime = true;
            PlayerPrefs.SetInt (FSMgenerator.SCORE_KEY, 0);
            PlayerPrefs.Save ();
        }
    }
    public override void TriggerExit2D (Collider2D other) {
        if (other.gameObject.tag == "level") {
            LevelPiece op = firstPiece;
            GameObject.Destroy (op.piece);
            firstPiece = op.next;
            op.next = null;
            SpawnLevel ();
        }
    }

    public override IEnumerator run () {
        var magnets = GameObject.FindGameObjectsWithTag ("Player");
        GameObject Magnet;
        if (magnets.Length > 0) {
            Magnet = magnets[0];
            Magnet.GetComponent<Magnet> ().reset (camera);
        } else {
            Magnet = GameObject.Instantiate (ParentMachine.Component.MagnetPrefab, new Vector3 (camera.transform.position.x, camera.transform.position.y + camera.orthographicSize / 2), Quaternion.identity) as GameObject;
        }
        Magnet.GetComponent<Magnet> ().ControlsDisabled = true;
        spawnStart (7);
        startScreenAnimator.SetBool ("visible", true);
        while (ParentMachine.Current.GetType () == GetType ()) {
            AdvanceLevelPosition ();
            bgController.update (gamespeed);
            yield return null;
        }
        removesections ();
        HowToScreen.SetActive(false);
        startScreenAnimator.SetBool ("visible", false);
    }

    public void navigateToPlay () {
        if(isFirstTime) {
            HowToScreen.SetActive(true);
            instructionsExitToMenu.SetActive(false);
            instructionsNewGame.SetActive(true);
            isFirstTime = false;
        } else {
            stateMachine.SetState (typeof (GamePlay), true, new object[] { MainStateMachine.instance });
            instructionsNewGame.SetActive(false);
            instructionsExitToMenu.SetActive(true);
        }
    }

    public void showHowTo () {
        startScreenAnimator.SetBool ("visible", false);
        HowToScreen.SetActive (true);
    }

    public void hideHowTo () {
        HowToScreen.SetActive (false);
        startScreenAnimator.SetBool ("visible", true);
    }

    public override void SpawnLevel () {
        int levno;
        levno = StraightCount - 1;
        LevelPiece lp = new LevelPiece (ParentMachine.Component.levels[levno], lastPiece.Spawnpos ());
        Addlevel (lp);
    }

}