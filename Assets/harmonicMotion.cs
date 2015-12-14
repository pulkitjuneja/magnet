using UnityEngine;
using System.Collections;

public class harmonicMotion : MonoBehaviour {

	public float offset;
	public float speed;
	private float X,Y,Z,pos;

	void Start () {
		X = transform.position.x;
		Y = transform.position.y;
		pos = transform.position.z;
		Z = pos;
	}
	void Update () {
		if (pos - Z < offset) 
        {
			Z = Z - speed * Time.deltaTime;
			transform.position = new Vector3 (transform.position.x,transform.position.y, Z);
		} else
			Z = pos;
	}
}
