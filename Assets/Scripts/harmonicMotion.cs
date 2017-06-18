using UnityEngine;
using System.Collections;

public class harmonicMotion : MonoBehaviour {

	private float offset = 6.0f;
	public float speed;
    private int multiplier = 1;
    private float Z, pos;
    private Light spotLight;
    public static float temp = 0;


	void Start () {
		pos = transform.position.z;
        spotLight = GetComponent<Light>();
        temp = 0;
		Z = pos;
	}
    void Update()
    {
        if (pos - Z < offset)
        {
            Z = Z - speed * Time.deltaTime;
            transform.position = new Vector3(transform.position.x, transform.position.y, Z);
        }
        else
            Z = pos;
        spotLight.spotAngle = 23.0f + temp;
    }
}