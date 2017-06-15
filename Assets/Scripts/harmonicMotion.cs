using UnityEngine;
using System.Collections;

public class harmonicMotion : MonoBehaviour {

	private float offset = -2;
	public float speed;
    private int multiplier = 1;
    private float Z, pos;
    public static float temp = 0;


	void Start () {
		pos = transform.position.z;
        temp = 0;
		Z = pos;
	}
	void Update () {
        Z = Z -  speed * Time.unscaledDeltaTime;
        if (Z < offset)
        {
            Z = pos;
        }
       // else if(Z>pos)
       // {
       //     multiplier = 1;
       // }
        transform.position = new Vector3(transform.position.x, transform.position.y, Z);
        GetComponent<Light>().spotAngle = 23.0f + temp;

    }
}