using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class generator : MonoBehaviour {

    public static float gamespeed = 5;
    public static int Score;

    int GameState;
    Camera camera;
    float levelHeight;
    Vector3 levelGenPos;
    float ElapsedTime = 0;
    public GameObject[] levels;

    public Text Distance , FinalScore;
    GameObject Magnet;
    public GameObject MagnetPrefab;
    public GameObject StartMenu ,  EndMenu; 

	void Start () 
    {
        Score = 0;
        Distance = GameObject.Find("Distance").GetComponent<Text>();
        SetScore();
        camera = Camera.main;
        levelHeight = camera.orthographicSize+0.1f;
        levelGenPos = new Vector3(camera.transform.position.x, camera.transform.position.y - (1.5f*levelHeight));
        this.transform.position = new Vector3(transform.position.x, camera.transform.position.y + camera.orthographicSize - 0.05f);
        ToggleStartMenu(true);
        GameState = 0;
	}

	void Update () 
    {
        if(GameState == 0)
        {
            if(Input.GetKey(KeyCode.Space))
            {
                GameState = 1;
                ToggleStartMenu(false);
                Magnet = Instantiate(MagnetPrefab, new Vector3(camera.transform.position.x, camera.transform.position.y + camera.orthographicSize / 2), Quaternion.identity) as GameObject;
                spawnStart();
            }
        }
        else if (GameState == 1)
        {
            ElapsedTime += Time.deltaTime;
            if (ElapsedTime >= 3.0f && Time.timeScale <= 5.0f)
            {
                Time.timeScale += 0.075f;
                Debug.Log(Time.timeScale);
                ElapsedTime = 0;
            }
            if (Magnet == null)
            {
                Debug.Log("End");
                FinalScore.text = "Final Score : " + Score.ToString();
                ToggleEndMenu(true);
                GameState = 2;
            }
        }
        else if(GameState == 2)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                ToggleEndMenu(false);
                ResetGame();
            }
        }
	}
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            Destroy(other.gameObject);
            Debug.Log("End");
            FinalScore.text = "Final Score : " + Score.ToString();
            ToggleEndMenu(true);
            GameState = 2;
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {

        if(other.gameObject.tag=="level" && GameState == 1)
        {
            Destroy(other.gameObject);
            int random = Random.Range(0, levels.Length);
            GameObject lev = Instantiate(levels[random], levelGenPos, Quaternion.identity) as GameObject;
            lev.GetComponent<Rigidbody2D>().velocity = new Vector2(0,gamespeed);
            Score += 1;
            SetScore();
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

    void SetScore()
    {
        Distance.text = "Meters Fallen : " + Score.ToString();
    }

    void ToggleStartMenu(bool x)
    {
        StartMenu.SetActive(x);
    }

    void ToggleEndMenu(bool x)
    {
        EndMenu.SetActive(x);
    }

    void ResetGame()
    {
        Score = 0;
        SetScore();
        GameObject[] remaining = GameObject.FindGameObjectsWithTag("level");
        foreach(GameObject g in remaining)
        {
            Destroy(g);
        }
        Time.timeScale = 1.0f;
        GameState = 1;
        Magnet = Instantiate(MagnetPrefab, new Vector3(camera.transform.position.x, camera.transform.position.y + camera.orthographicSize / 2), Quaternion.identity) as GameObject;
        spawnStart();
    }
}
