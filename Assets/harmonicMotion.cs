using UnityEngine;
using System.Collections;

public class harmonicMotion : MonoBehaviour {

	public float offset;
	public float speed;
	private float X,Y,Z,pos;
	public GameObject magnet;

	void Start () {
		X = magnet.transform.position.x;
		Y = magnet.transform.position.y;
		pos = transform.position.z;
		Z = pos;
	}
	void Update () {
		if (pos - Z < offset) 
        {
			Z = Z - speed * Time.deltaTime;
			transform.position = new Vector3 (magnet.transform.position.x,magnet.transform.position.y, Z);
		} else
			Z = pos;
	}
}
