using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GamePlay : GameRunning {
    public static int Score;
    GameObject Magnet;
    public Text Distance;
    float ElapsedTime = 0;
    float SpawnTimer = 0;
    public LevelPiece FirstPickup;
    public LevelPiece LastPickup;
    public Spawner[] spawners;
    bool Paused;
    Animator InGameUiAnimator;
    GameObject PauseMenu;
    public float SpawnMin = 8.0f, SpawnMax = 15.0f, spTime;
    public GamePlay (MainStateMachine m) : base (m) {
        Camera camera = Camera.main;
        spawners = ParentMachine.Component.spawners;
        var InGameUiObject = GameObject.Find ("InGameUi");
        PauseMenu = GameObject.Find ("Canvas").transform.Find ("PauseMenu").gameObject;
        Distance = InGameUiObject.GetComponentInChildren<Text> ();
        InGameUiAnimator = InGameUiObject.GetComponent<Animator> ();
        InGameUiAnimator.SetBool ("visible", true);
        Score = 0;
        Distance.text = Score.ToString ();
        var magnets = GameObject.FindGameObjectsWithTag ("Player");
        if (magnets.Length > 0) {
            Magnet = magnets[0];
        } else {
            Magnet = GameObject.Instantiate (ParentMachine.Component.MagnetPrefab, new Vector3 (camera.transform.position.x, camera.transform.position.y + camera.orthographicSize / 2), Quaternion.identity) as GameObject;
        }
        Magnet.GetComponent<Magnet> ().GameOverEvent += GameOver;
        Magnet.GetComponent<Magnet> ().ControlsDisabled = false;
        spTime = UnityEngine.Random.Range (SpawnMin, SpawnMax);
    }

    public void onPause () {
        Paused = true;
        InGameUiAnimator.SetBool ("visible", false);
        PauseMenu.SetActive (true);
        Time.timeScale = 0f;
    }
    public void onResume () {
        Time.timeScale = 1.0f;
        Paused = false;
        PauseMenu.SetActive (false);
        InGameUiAnimator.SetBool ("visible", true);
    }
    public override IEnumerator run () {
        spawnStart (2);
        while (ParentMachine.Current.GetType () == GetType ()) {
            if (Paused) {
                yield return null;
                continue;
            }
            AdvanceLevelPosition ();
            bgController.update (gamespeed);
            ElapsedTime += Time.unscaledDeltaTime;
            SpawnTimer += Time.unscaledDeltaTime;

            if (ElapsedTime >= 3.0f && gamespeed < 16.0f) {
                gamespeed += 0.5f;
                ElapsedTime = 0;
            }

            if (SpawnTimer >= spTime) {
                SpawnPickups ();
                SpawnTimer = 0;
                spTime = UnityEngine.Random.Range (SpawnMin, SpawnMax);
            }
            yield return null;
        }
        InGameUiAnimator.SetBool ("visible", false);
        PauseMenu.SetActive (false);
        ClearPowerups ();
        removesections ();
        if (Magnet != null) {
            Magnet.GetComponent<Magnet> ().GameOverEvent -= GameOver;
            // GameObject.Destroy(Magnet); //sucky fix have to improve
        }
        ResetGame ();
        while(InGameUiAnimator.IsInTransition(0)) {
            yield return null;
        }
    }
    void ClearPowerups () {
        Pickup[] ps = GameObject.FindObjectsOfType<Pickup> ();
        foreach (var p in ps) {
            GameObject.Destroy (p.gameObject);
        }
    }
    void SpawnPickups () {
        var spawner = spawners[UnityEngine.Random.Range (0, spawners.Length)];
        int r = UnityEngine.Random.Range (0, 11);
        int sp = r < 6 ? 0 : (r < 8 ? 1 : 2);
        spawner.Spawn (ParentMachine.Component.powerups[sp]);
    }
    public override void TriggerExit2D (Collider2D other) {

        if (other.gameObject.tag == "level") {
            LevelPiece op = firstPiece;
            if(op != null) {
                GameObject.Destroy (op.piece);
                //Debug.Log(op);
                firstPiece = op.next;
                op.next = null;
                SpawnLevel ();
            }
            Score += 1;
            Distance.text = Score.ToString ();
        }
    }

    public override void SpawnLevel () {
        int levno;
        int random = UnityEngine.Random.Range (0, 5);
        if (random < 4)
            levno = UnityEngine.Random.Range (0, StraightCount);
        else
            levno = UnityEngine.Random.Range (StraightCount, ParentMachine.Component.levels.Length - 1);
        LevelPiece p = new LevelPiece (ParentMachine.Component.levels[levno], lastPiece.Spawnpos ());
        Addlevel (p);
    }
    void GameOver () {
        ParentMachine.SetState (typeof (GameOverState), false, new object[] { ParentMachine, Score });
    }

    void ResetGame () {
        GamePlay.gamespeed = 5;
    }

}