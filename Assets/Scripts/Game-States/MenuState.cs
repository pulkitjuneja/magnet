using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class MenuState : GameRunning {

    Animator StartScreenAnimator ;
    public MenuState(MainStateMachine g) : base(g) {
        StartScreenAnimator = GameObject.Find("Start Menu").GetComponent<Animator>();
        if (!PlayerPrefs.HasKey(FSMgenerator.SCORE_KEY)) {
            PlayerPrefs.SetInt(FSMgenerator.SCORE_KEY, 0);
            PlayerPrefs.Save();
        }
        var magnets = GameObject.FindGameObjectsWithTag("Player");
        GameObject Magnet;
        if (magnets.Length > 0) {
            Magnet = magnets[0];
        }
        else {
            Magnet = GameObject.Instantiate(ParentMachine.Component.MagnetPrefab, new Vector3(camera.transform.position.x, camera.transform.position.y + camera.orthographicSize / 2), Quaternion.identity) as GameObject;
        }
    }
    public override void TriggerExit2D(Collider2D other) {
        if (other.gameObject.tag == "level") {
            LevelPiece op = firstPiece;
            GameObject.Destroy(op.piece);
            firstPiece = op.next;
            op.next = null;
            SpawnLevel();
        }
    }

    public override IEnumerator run() {
        spawnStart(7);
        StartScreenAnimator.SetBool("visible",true);
        while (ParentMachine.Current.GetType() == GetType()) {
            AdvanceLevelPosition();
			bgController.update (gamespeed);
            yield return null;
        }
        removesections();
        StartScreenAnimator.SetBool("visible",false);
    }

    public override void SpawnLevel() {
        int levno;
        levno = StraightCount - 1;
        LevelPiece lp = new LevelPiece(ParentMachine.Component.levels[levno], lastPiece.Spawnpos());
        Addlevel(lp);
    }

}