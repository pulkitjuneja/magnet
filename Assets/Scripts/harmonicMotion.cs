using UnityEngine;
using System.Collections;

public class harmonicMotion : MonoBehaviour {

	public float offset;
	public float speed;
	private float Z,pos;
	public static float temp;

	void Start () {
		pos = transform.position.z;
        temp = 0;
		Z = pos;
	}
	void Update () {
        if (pos - Z < offset)
        {
            Z = Z - speed * Time.unscaledDeltaTime;
            transform.position = new Vector3(transform.position.x, transform.position.y, Z);
        }
        else
            Z = pos;
		GetComponent<Light> ().spotAngle = 23.0f + temp;
	}
}