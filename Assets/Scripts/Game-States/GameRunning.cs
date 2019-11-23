using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;


public abstract class GameRunning : State<GameStateMachine> {
  public Camera camera;
  float levelHeight;
  public LevelPiece firstPiece;
  public LevelPiece lastPiece;
  int numBlocks = 5;
  Transform transform;
  public int StraightCount = 32;
  public Vector3 levelGenPos;
  public GameRunning(GameStateMachine g) : base(g) {
    transform = ParentMachine.Component.transform;
    camera = Camera.main;
    levelHeight = 10;
    ParentMachine.Component.gameSpeed.value = 5;
    levelGenPos = new Vector3(transform.position.x, (transform.position.y - (numBlocks - 0.5f) * levelHeight) + 0.15f, 0);
  }

  public void spawnStart(int StartSPawnLimit) {
    var posx = this.transform.position.x;
    var posy = this.transform.position.y + 0.05f;
    float posfactor = 0.5f;
    int z;
    LevelPiece pc = new LevelPiece(ParentMachine.Component.levels[StraightCount - 1], new Vector3(posx, posy - posfactor * levelHeight, 0));
    Addlevel(pc);
    for (int i = 0; i < numBlocks; i++) {
      posfactor += 1.0f;
      if (i < StartSPawnLimit)
        z = StraightCount - 1;
      else
        z = UnityEngine.Random.Range(0, StraightCount - 1);
      pc = new LevelPiece(ParentMachine.Component.levels[z], new Vector3(posx, posy - posfactor * levelHeight, 0));
      Addlevel(pc);
    }
  }

  public void Addlevel(LevelPiece newPiece) {
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
      pc.piece.transform.Translate(Vector3.up * ParentMachine.Component.gameSpeed.value * Time.deltaTime);
      pc = pc.next;
    }
  }

  public abstract void SpawnLevel();

}
