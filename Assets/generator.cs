using UnityEngine;
using System.Collections;

public class generator : MonoBehaviour {

    public static float gamespeed = 5.0f;
    
    Camera camera;
    float levelHeight;
    public GameObject Magnet;
    Vector3 levelGenPos;
    public GameObject[] levels ;

	void Start () 
    {
        camera = Camera.main;
        Magnet = Instantiate(Magnet, new Vector3(camera.transform.position.x, camera.transform.position.y + camera.orthographicSize/2), Quaternion.identity) as GameObject;
        levelHeight = camera.orthographicSize+0.1f;
        levelGenPos = new Vector3(camera.transform.position.x, camera.transform.position.y - (1.5f*levelHeight));
        this.transform.position = new Vector3(transform.position.x, camera.transform.position.y + camera.orthographicSize);
        spawnStart();
	}

	void Update () {	
	}

    void OnTriggerExit2D(Collider2D other)
    {

        if(other.gameObject.tag=="level")
        {
            Destroy(other.gameObject);
            int random = Random.Range(0, levels.Length);
            GameObject lev = Instantiate(levels[random], levelGenPos, Quaternion.identity) as GameObject;
            lev.GetComponent<Rigidbody2D>().velocity = new Vector2(0,gamespeed);
        }

    }

    void spawnStart()
    {
        int z = Random.Range(0, levels.Length);
        GameObject lev;
        lev = Instantiate(levels[levels.Length-1], new Vector3(camera.transform.position.x,camera.transform.position.y-levelHeight/2,0), Quaternion.identity) as GameObject;
        lev.GetComponent<Rigidbody2D>().velocity = new Vector2(0, gamespeed);
        lev = Instantiate(levels[levels.Length-1], new Vector3(camera.transform.position.x, camera.transform.position.y + levelHeight/2, 0), Quaternion.identity) as GameObject;
        lev.GetComponent<Rigidbody2D>().velocity = new Vector2(0, gamespeed);
        lev = Instantiate(levels[z], levelGenPos, Quaternion.identity) as GameObject;
        lev.GetComponent<Rigidbody2D>().velocity = new Vector2(0, gamespeed);
    }
}
