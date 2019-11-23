using UnityEngine;
using System.Collections;

public class BackgroundController : MonoBehaviour {

  public GameObject background;
  float cameraBottom;
  float CameraTop;

  public FloatValue gameSpeed;
  LevelPiece firstPiece, lastPiece;

  void Awake() {
    cameraBottom = Camera.main.transform.position.y - Camera.main.orthographicSize;
    CameraTop = Camera.main.transform.position.y + Camera.main.orthographicSize;
    var startPosition = CameraTop - background.GetComponent<SpriteRenderer>().sprite.bounds.extents.y * background.transform.localScale.y;
    firstPiece = lastPiece = new LevelPiece(background, new Vector3(0, startPosition, 0));
  }

  void Update() {
    advanceLelvels();
    checkBounds();
  }

  void advanceLelvels() {
    LevelPiece pc = firstPiece;
    while (pc != null) {
      pc.piece.transform.Translate(Vector3.up * gameSpeed.GetValue() * Time.deltaTime * 0.1f);
      pc = pc.next;
    }

  }

  void checkBounds() {
    var firstPieceSprite = firstPiece.piece.GetComponent<SpriteRenderer>().sprite;
    var lastPieceSprite = lastPiece.piece.GetComponent<SpriteRenderer>().sprite;
    var firstPieceBottom = firstPiece.piece.transform.position.y - firstPieceSprite.bounds.extents.y * firstPiece.piece.transform.localScale.y;
    var lastPieceBottom = lastPiece.piece.transform.position.y - lastPieceSprite.bounds.extents.y * lastPiece.piece.transform.localScale.y;
    if (lastPieceBottom >= cameraBottom) {
      var startPosition = lastPieceBottom - background.GetComponent<SpriteRenderer>().sprite.bounds.extents.y * background.transform.localScale.y;
      lastPiece.next = new LevelPiece(background, new Vector3(0, startPosition, 0));
      lastPiece = lastPiece.next;
    }
    if (firstPieceBottom >= CameraTop) {
      var op = firstPiece;
      GameObject.Destroy(firstPiece.piece);
      firstPiece = firstPiece.next;
      op.next = null;
    }

  }
}
