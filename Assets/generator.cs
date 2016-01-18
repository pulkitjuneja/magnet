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
    float SpawnTimer = 0;
    public GameObject[] levels;

    public Text Distance , FinalScore;
    public GameObject DistanceG;
    int numBlocks = 7;
    public float SpawnMin = 3.0f, SpawnMax = 8.0f , spTime;
    GameObject Magnet;
    public GameObject MagnetPrefab;
    public Spawner[] spawners;
    public GameObject[] powerups;
    public GameObject StartMenu ,  EndMenu; 

	void Start () 
    {
        Score = 0;
        Distance = DistanceG.GetComponent<Text>();
        SetScore();
        camera = Camera.main;
        levelHeight = 10;
        this.transform.position = new Vector3(transform.position.x, (camera.transform.position.y + camera.orthographicSize*camera.aspect)- 0.05f);
        levelGenPos = new Vector3(this.transform.position.x, (this.transform.position.y - (numBlocks-0.5f) * levelHeight)+0.15f, 0);
        ToggleStartMenu(true);
        DistanceG.SetActive(false);
        GameState = 0;
        spTime = Random.Range(SpawnMin,SpawnMax);
	}

	void Update () 
    {
        if(GameState == 0)
        {
            if(Input.GetKey(KeyCode.Space))
            {
                GameState = 1;
                ToggleStartMenu(false);
                DistanceG.SetActive(true);
                Magnet = Instantiate(MagnetPrefab, new Vector3(camera.transform.position.x, camera.transform.position.y + camera.orthographicSize / 2), Quaternion.identity) as GameObject;
                spawnStart();
            }
        }
        else if (GameState == 1)
        {
            ElapsedTime += Time.unscaledDeltaTime;
            SpawnTimer += Time.unscaledDeltaTime;
            if (ElapsedTime >= 3.0f && Time.timeScale <= 5.0f)
            {
                Time.timeScale += 0.1f;
                ElapsedTime = 0;
            }
            if(SpawnTimer>=spTime)
            {
                SpawnPickups();
                SpawnTimer = 0;
                spTime = Random.Range(SpawnMin, SpawnMax);
            }
            if (Magnet == null)
            {
                FinalScore.text = "Final Score : " + Score.ToString();
                DistanceG.SetActive(false);
                ToggleEndMenu(true);
                GameState = 2;
                removesections();
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
    void OnTriggerExit2D(Collider2D other)
    {

        if(other.gameObject.tag=="level" && GameState == 1)
        {
            Destroy(other.gameObject);
            int random = Random.Range(0, levels.Length-1);
            GameObject lev = Instantiate(levels[random], levelGenPos, Quaternion.identity) as GameObject;
            lev.GetComponent<Rigidbody2D>().velocity = new Vector2(0,gamespeed);
            Score += 1;
            SetScore();
        }
    }

    /*void spawnStart()
    {
        int z = Random.Range(0, levels.Length);
        GameObject lev;
        lev = Instantiate(levels[levels.Length-1], new Vector3(camera.transform.position.x,camera.transform.position.y-levelHeight/2,0), Quaternion.identity) as GameObject;
        lev.GetComponent<Rigidbody2D>().velocity = new Vector2(0, gamespeed);
        lev = Instantiate(levels[levels.Length-1], new Vector3(camera.transform.position.x, camera.transform.position.y + levelHeight/2, 0), Quaternion.identity) as GameObject;
        lev.GetComponent<Rigidbody2D>().velocity = new Vector2(0, gamespeed);
        lev = Instantiate(levels[z],new Vector3(camera.transform.position.x, camera.transform.position.y - 1.5f*levelHeight, 0) , Quaternion.identity) as GameObject;
        lev.GetComponent<Rigidbody2D>().velocity = new Vector2(0, gamespeed);
        lev = Instantiate(levels[z], new Vector3(camera.transform.position.x, camera.transform.position.y - 2.5f * levelHeight, 0), Quaternion.identity) as GameObject;
        lev.GetComponent<Rigidbody2D>().velocity = new Vector2(0, gamespeed);
        lev = Instantiate(levels[z], new Vector3(camera.transform.position.x, camera.transform.position.y - 3.5f * levelHeight, 0), Quaternion.identity) as GameObject;
        lev.GetComponent<Rigidbody2D>().velocity = new Vector2(0, gamespeed);
        lev = Instantiate(levels[z], levelGenPos, Quaternion.identity) as GameObject;
        lev.GetComponent<Rigidbody2D>().velocity = new Vector2(0, gamespeed);
    }*/
    void spawnStart()
    {
        var posx = this.transform.position.x;
        var posy = this.transform.position.y+0.05f;
        float posfactor = 0.5f;
        int z;
        GameObject lev;
        lev = Instantiate(levels[levels.Length - 1], new Vector3(posx,posy-posfactor*levelHeight, 0), Quaternion.identity) as GameObject;
        lev.GetComponent<Rigidbody2D>().velocity = new Vector2(0, gamespeed);
        for(int i = 0 ; i<numBlocks-1 ; i++)
        {
            posfactor+= 1.0f;
            if (i < 5)
                z = levels.Length - 1;
            else
                z = Random.Range(0, levels.Length - 1);
            lev = Instantiate(levels[z], new Vector3(posx, posy - posfactor * levelHeight, 0), Quaternion.identity) as GameObject;
            lev.GetComponent<Rigidbody2D>().velocity = new Vector2(0, gamespeed);
        }
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
        harmonicMotion.temp = 0;
        DistanceG.SetActive(true);
        SetScore();
        Time.timeScale = 1.0f;
        GameState = 1;
        Magnet = Instantiate(MagnetPrefab, new Vector3(camera.transform.position.x, camera.transform.position.y + camera.orthographicSize / 2), Quaternion.identity) as GameObject;
        spawnStart();
    }

    void removesections()
    {
        GameObject[] remaining = GameObject.FindGameObjectsWithTag("level");
        foreach (GameObject g in remaining)
        {
            Destroy(g);
        }
    }

    void SpawnPickups()
    {
        var spawner = spawners[Random.Range(0, spawners.Length)];
        spawner.Spawn(powerups[Random.Range(0,powerups.Length)]); 
    }
}
