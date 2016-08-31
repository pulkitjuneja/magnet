using UnityEngine;
using System.Collections;

public class LevelPiece {

    public LevelPiece next;
    public GameObject piece;
    private Sprite sprite;

    public LevelPiece(GameObject piece , Vector3 pos)
    {
        this.piece  = GameObject.Instantiate(piece, pos, Quaternion.identity) as GameObject;
        next = null;
    }
    public Vector3 Spawnpos()
    {
        return new Vector3(piece.transform.position.x, piece.transform.position.y - 10.0f, piece.transform.position.z);
    }
}
