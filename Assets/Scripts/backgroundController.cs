using UnityEngine;
using System.Collections;

public class backgroundController {

	GameObject background; 
	public static backgroundController instance;
	float cameraBottom ;
	float CameraTop ;
	LevelPiece firstPiece,lastPiece;

	public static backgroundController getInstance() {
		if(instance == null) {
			instance = new backgroundController ();
		}
		return instance;
	}

	public backgroundController() {
		if (background == null) {
			background = Resources.Load<GameObject> ("background");
			cameraBottom = Camera.main.transform.position.y - Camera.main.orthographicSize;
			CameraTop = Camera.main.transform.position.y + Camera.main.orthographicSize;
			var startPosition = CameraTop - background.GetComponent<SpriteRenderer> ().sprite.bounds.extents.y*background.transform.localScale.y;
			firstPiece = lastPiece = new LevelPiece (background,new Vector3(0,startPosition,0));
		}
	}

	public void update (float gameSpeed) {
		advanceLelvels (gameSpeed);
		checkBounds ();
	} 

	 void advanceLelvels(float gameSpeed) {
		LevelPiece pc = firstPiece;
		while (pc != null) {
			pc.piece.transform.Translate(Vector3.up * gameSpeed * Time.deltaTime*0.1f);
			pc = pc.next;
		}

	}

	 void checkBounds() {
		var firstPieceSprite = firstPiece.piece.GetComponent<SpriteRenderer> ().sprite;
		var lastPieceSprite = lastPiece.piece.GetComponent<SpriteRenderer> ().sprite;
		var firstPieceBottom = firstPiece.piece.transform.position.y - firstPieceSprite.bounds.extents.y * firstPiece.piece.transform.localScale.y;
		var lastPieceBottom = lastPiece.piece.transform.position.y - lastPieceSprite.bounds.extents.y * lastPiece.piece.transform.localScale.y;
		if(lastPieceBottom >= cameraBottom) {
			var startPosition = firstPieceBottom - background.GetComponent<SpriteRenderer> ().sprite.bounds.extents.y*background.transform.localScale.y;
			lastPiece.next = new LevelPiece (background, new Vector3 (0, startPosition, 0));
			lastPiece = lastPiece.next;
		}
		if (firstPieceBottom >= CameraTop) {
			var op = firstPiece;
			GameObject.Destroy (firstPiece.piece);
			firstPiece = firstPiece.next;
			op.next = null;
		}

	}
}
