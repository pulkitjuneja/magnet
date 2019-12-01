using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelPieceController : MonoBehaviour {

  public LevelPiece firstPiece;
  public LevelPiece lastPiece;
  public FloatValue gameSpeed;
  public GameObject[] levels;
  public Signal stateChangedSignal;
  public Signal levelCompleteSignal;

  float levelHeight = 10;
  int onScreenLevelCount = 5;
  int emptyLevelIndex = 32;
  GameStates currentState;

  void Start() {
    stateChangedSignal.addListener(onStateChanged);
  }

  void onStateChanged(SignalData data) {
    removesections();
    GameStates newState = data.get<GameStates>("newStateType");
    if (newState == GameStates.PlayingState) {
      spawnInitialLevels(2);
    }
    currentState = newState;
  }

  void spawnInitialLevels(int emptyLevelCount) {
    var posX = this.transform.position.x;
    var posY = this.transform.position.y + 0.05f;
    float spawnOffset = 0.5f;
    int random;
    LevelPiece pc = new LevelPiece(levels[emptyLevelIndex - 1], new Vector3(posX, posY - spawnOffset * levelHeight, 0));
    Addlevel(pc);
    for (int i = 0; i < onScreenLevelCount; i++) {
      spawnOffset += 1.0f;
      if (i < emptyLevelCount)
        random = emptyLevelIndex - 1;
      else
        random = UnityEngine.Random.Range(0, emptyLevelIndex - 1);
      pc = new LevelPiece(levels[random], new Vector3(posX, posY - spawnOffset * levelHeight, 0));
      Addlevel(pc);
    }
  }

  void Addlevel(LevelPiece newPiece) {
    if (lastPiece == null) {
      lastPiece = newPiece;
      firstPiece = newPiece;
    } else {
      lastPiece.next = newPiece;
      lastPiece = newPiece;
    }
  }

  public void removesections() {
    GameObject[] remaining = GameObject.FindGameObjectsWithTag("level");
    foreach (GameObject g in remaining) {
      GameObject.Destroy(g);
    }
    firstPiece = lastPiece = null;
  }

  public void AdvanceLevelPosition() {
    LevelPiece pc = firstPiece;
    while (pc != null) {
      pc.piece.transform.Translate(Vector3.up * gameSpeed.value * Time.deltaTime);
      pc = pc.next;
    }
  }

  public void spawnNextLevel() {
    int levNo;
    int random = UnityEngine.Random.Range(0, 5);
    if (random < 4)
      levNo = UnityEngine.Random.Range(0, emptyLevelIndex);
    else
      levNo = UnityEngine.Random.Range(emptyLevelIndex, levels.Length - 1);
    LevelPiece p = new LevelPiece(levels[levNo], lastPiece.getNextSpawnPosition());
    Addlevel(p);
  }

  void Update() {
    AdvanceLevelPosition();
  }

  void OnTriggerExit2D(Collider2D other) {
    if (other.gameObject.tag == "level") {
      LevelPiece op = firstPiece;
      GameObject.Destroy(op.piece);
      firstPiece = op.next;
      op.next = null;
      spawnNextLevel();
      levelCompleteSignal.fire(null);
    }
  }
}
