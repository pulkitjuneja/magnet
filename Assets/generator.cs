using UnityEngine;
using System.Collections;

public class generator : MonoBehaviour {

    public static float gamespeed = 5;
    public static int Score;
    
    Camera camera;
    float levelHeight;
    public GameObject Magnet;
    Vector3 levelGenPos;
    float ElapsedTime = 0;
    public GameObject[] levels ;

	void Start () 
    {
        Score = 0;
        camera = Camera.main;
        Magnet = Instantiate(Magnet, new Vector3(camera.transform.position.x, camera.transform.position.y + camera.orthographicSize/2), Quaternion.identity) as GameObject;
        levelHeight = camera.orthographicSize+0.1f;
        levelGenPos = new Vector3(camera.transform.position.x, camera.transform.position.y - (1.5f*levelHeight));
        this.transform.position = new Vector3(transform.position.x, camera.transform.position.y + camera.orthographicSize - 0.05f);
        spawnStart();
	}

	void Update () 
    {
        ElapsedTime += Time.deltaTime;
        if(ElapsedTime>=3.0f)
        {
            Time.timeScale += 0.1f; 
            Debug.Log(Time.timeScale);
            ElapsedTime = 0;
        }
	}

    void OnTriggerExit2D(Collider2D other)
    {

        if(other.gameObject.tag=="level")
        {
            Destroy(other.gameObject);
            int random = Random.Range(0, levels.Length);
            GameObject lev = Instantiate(levels[random], levelGenPos, Quaternion.identity) as GameObject;
            lev.GetComponent<Rigidbody2D>().velocity = new Vector2(0,gamespeed);
            Score += 1;
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
