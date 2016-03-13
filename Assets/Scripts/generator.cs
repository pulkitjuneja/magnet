using UnityEngine;
using UnityEngine.UI;
using System.Collections;

delegate IEnumerator State();
public delegate void SpeedChanged(float speed);
public class generator : MonoBehaviour {

    public static float gamespeed = 5;
    public static int Score;
    public int StraightCount ;
    public event SpeedChanged EventSpeedChanged;

    public static generator instance = null ;

    State Current;
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

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }
	IEnumerator Start () 
    {
        Score = 0;
        Distance = DistanceG.GetComponent<Text>();
        camera = Camera.main;
        levelHeight = 10;
        Debug.Log(camera.orthographicSize);
        this.transform.position = new Vector3(transform.position.x, (camera.transform.position.y + camera.orthographicSize)- 0.05f);
        levelGenPos = new Vector3(this.transform.position.x, (this.transform.position.y - (numBlocks-0.5f) * levelHeight)+0.15f, 0);
        spTime = Random.Range(SpawnMin,SpawnMax);
        Current = MenuState;
        while(true)
        {
            yield return StartCoroutine(Current());
        }
	}

    IEnumerator MenuState()
    {
        ToggleStartMenu(true);
        DistanceG.SetActive(false);
        while(Current == MenuState)
        {
             #if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBPLAYER
            if (Input.GetKey(KeyCode.Space))
            {
                Current = GameRunning;
                break;
            }
            #else
            if(Input.touchCount>0)
            {
                Current = GameRunning;
                break;
            }
            #endif
            yield return null;
        }
        ToggleStartMenu(false);
    }

    IEnumerator GameRunning()
    {
        SetScore();
        DistanceG.SetActive(true);
        Magnet = Instantiate(MagnetPrefab, new Vector3(camera.transform.position.x, camera.transform.position.y + camera.orthographicSize / 2), Quaternion.identity) as GameObject;
        spawnStart();
        while(Current == GameRunning)
        {
            ElapsedTime += Time.unscaledDeltaTime;
            SpawnTimer += Time.unscaledDeltaTime;
            if (ElapsedTime >= 1.0f && Time.timeScale <= 5.0f)
            {
                Time.timeScale += 0.1f;
                ElapsedTime = 0;
            }
            if (SpawnTimer >= spTime)
            {
                SpawnPickups();
                SpawnTimer = 0;
                spTime = Random.Range(SpawnMin, SpawnMax);
            }
            if (Magnet == null)
            {
                Current = GameOver;
                break;
            }
            yield return null;
        }
        DistanceG.SetActive(false);
        removesections();
    }

    IEnumerator GameOver()
    {
        FinalScore.text = "Final Score : " + Score.ToString();
        ToggleEndMenu(true);
        while(Current == GameOver)
        {
#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBPLAYER
            if (Input.GetKey(KeyCode.Space))
            {
                Current = GameRunning;
                break;
            }
#else
            if(Input.touchCount>0)
            {
                Current = GameRunning;
                break;
            }
#endif
            yield return null;
        }
        ToggleEndMenu(false);
        ResetGame();
    }

    void OnTriggerExit2D(Collider2D other)
    {

        if(other.gameObject.tag=="level" && Current == GameRunning)
        {
            EventSpeedChanged -= other.gameObject.GetComponent<ChangeSpeed>().changeSpeed;
            Destroy(other.gameObject);
            SpawnLevel();
            Score += 1;
            SetScore();
        }
    }
    void spawnStart()
    {
        var posx = this.transform.position.x;
        var posy = this.transform.position.y+0.05f;
        float posfactor = 0.5f;
        int z;
        GameObject lev;
        lev = Instantiate(levels[StraightCount -1], new Vector3(posx,posy-posfactor*levelHeight, 0), Quaternion.identity) as GameObject;
        lev.GetComponent<Rigidbody2D>().velocity = new Vector2(0, gamespeed);
        for(int i = 0 ; i<numBlocks-1 ; i++)
        {
            posfactor+= 1.0f;
            if (i < 5)
                z = StraightCount - 1;
            else
                z = Random.Range(0, StraightCount - 1);
            lev = Instantiate(levels[z], new Vector3(posx, posy - posfactor * levelHeight, 0), Quaternion.identity) as GameObject;
            lev.GetComponent<Rigidbody2D>().velocity = new Vector2(0, gamespeed);
            EventSpeedChanged += lev.GetComponent<ChangeSpeed>().changeSpeed;
        }
    }

    public void EndGame()
    {
        Current = GameOver;
    }

    void SpawnLevel()
    {
        int random = Random.Range(0,5),levno;
       // Debug.Log(random);
        if(random<4)
            levno = Random.Range(0, StraightCount);
        else
            levno = Random.Range(StraightCount, levels.Length -1);

        GameObject lev = Instantiate(levels[levno], levelGenPos, Quaternion.identity) as GameObject;
        lev.GetComponent<Rigidbody2D>().velocity = new Vector2(0, gamespeed);
        EventSpeedChanged += lev.GetComponent<ChangeSpeed>().changeSpeed;
    }
    void SetScore()
    {
        Distance.text = Score.ToString();
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
        Time.timeScale = 1.0f;
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
