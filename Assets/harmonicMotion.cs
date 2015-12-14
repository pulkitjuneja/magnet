using UnityEngine;
using System.Collections;

public class harmonicMotion : MonoBehaviour {

	public float offset;
	public float speed;
	private float X,Y,Z,pos;
	// Use this for initialization
	void Start () {
		X = transform.position.x;
		Y = transform.position.y;
		pos = transform.position.z;
		Z = pos;
	}
	
	// Update is called once per frame
	void Update () {
		if (pos - Z < offset) {
			Z = Z - speed * Time.deltaTime;
			transform.position = new Vector3 (X, Y, Z);
		} else
			Z = pos;
	}
}
