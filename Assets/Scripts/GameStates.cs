using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;


    public abstract class GameRunning: State<MainStateMachine>
    {
        public Camera camera;
        float levelHeight;
        public LevelPiece firstPiece;
        public LevelPiece lastPiece;
        public static float gamespeed = 5;
        int numBlocks = 5;
        Transform transform;
        public int StraightCount = 32;
        public Vector3 levelGenPos;
        public GameRunning(MainStateMachine g)
            : base(g) 
        {
            transform = ParentMachine.Component.transform;
            camera = Camera.main;
            levelHeight = 10;
            levelGenPos = new Vector3(transform.position.x, (transform.position.y - (numBlocks - 0.5f) * levelHeight) + 0.15f, 0);
        }

        //public void spawnStart(int StraightSpawnLimit)
        //{
        //    var posx = this.transform.position.x;
        //    var posy = this.transform.position.y + 0.05f;
        //    float posfactor = 0.5f;
        //    int z;
        //    GameObject lev;
        //    lev = GameObject.Instantiate(ParentMachine.Component.levels[StraightCount - 1], new Vector3(posx, posy - posfactor * levelHeight, 0), Quaternion.identity) as GameObject;
        //    int i;
        //    lev.GetComponent<Rigidbody2D>().velocity = new Vector2(0, gamespeed);
        //    for (i = 0; i < numBlocks - 1; i++)
        //    {
        //        posfactor += 1.0f;
        //        if (i < StraightSpawnLimit)
        //            z = StraightCount - 1;
        //        else
        //            z = UnityEngine.Random.Range(0, StraightCount - 1);
        //        lev = GameObject.Instantiate(ParentMachine.Component.levels[z], new Vector3(posx, posy - posfactor * levelHeight, 0), Quaternion.identity) as GameObject;
        //        lev.GetComponent<Rigidbody2D>().velocity = new Vector2(0, gamespeed);
        //    }
        //}

        public void spawnStart(int StartSPawnLimit)
        {
            var posx = this.transform.position.x;
            var posy = this.transform.position.y + 0.05f;
            float posfactor = 0.5f;
            int z ; 
            LevelPiece pc = new LevelPiece(ParentMachine.Component.levels[StraightCount - 1],new Vector3(posx,posy-posfactor*levelHeight,0));
            Addlevel(pc);
            for(int i = 0 ; i<numBlocks;i++)
            {
                posfactor += 1.0f;
                if (i < StartSPawnLimit)
                    z = StraightCount - 1;
                else
                    z = UnityEngine.Random.Range(0, StraightCount - 1);
                pc = new LevelPiece(ParentMachine.Component.levels[z], new Vector3(posx, posy - posfactor * levelHeight, 0));
                Addlevel(pc);
            }
        }

        public void Addlevel(LevelPiece newPiece)
        {
         if (lastPiece == null)
         {
             lastPiece = newPiece;
             firstPiece = newPiece;
         }
         else
         {
             lastPiece.next = newPiece;
             lastPiece = newPiece;
         }
        }
        
        public void removesections()
        {
            GameObject[] remaining = GameObject.FindGameObjectsWithTag("level");
            foreach (GameObject g in remaining)
            {
                GameObject.Destroy(g);
            }
            firstPiece = lastPiece = null;
        }

        public void AdvanceLevelPosition()
        {
            LevelPiece pc = firstPiece;
            while(pc!=null)
            {
                pc.piece.transform.Translate(Vector3.up * gamespeed * Time.deltaTime);
                pc = pc.next;
            }
        }

        public abstract void SpawnLevel();

    }


    class MenuState : GameRunning
    {
        public MenuState(MainStateMachine g) : base(g) 
        {
            if(!PlayerPrefs.HasKey(FSMgenerator.SCORE_KEY))
            {
                PlayerPrefs.SetInt(FSMgenerator.SCORE_KEY, 0);
                PlayerPrefs.Save();
            }
        }
        public override void TriggerExit2D(Collider2D other)
        {
            if(other.gameObject.tag == "level")
            {
                LevelPiece op = firstPiece;
                GameObject.Destroy(op.piece);
                firstPiece = op.next;
                op.next = null;
                SpawnLevel();
            }
        }

        public override IEnumerator run()
        {
            spawnStart(7);
            ParentMachine.Component.StartMenu.SetActive(true);
            while(ParentMachine.Current.GetType()==GetType())
            {
                AdvanceLevelPosition();
                yield return null;
            }
            removesections();
            ParentMachine.Component.StartMenu.SetActive(false);
        }

        public override void SpawnLevel()
        {
            int levno;
            levno = StraightCount - 1;
            LevelPiece lp = new LevelPiece(ParentMachine.Component.levels[levno],lastPiece.Spawnpos());
            Addlevel(lp);
        }
    }



    class GamePlay:GameRunning
    {
        public static int Score;
        GameObject Magnet;
        public Text Distance;
        float ElapsedTime = 0;
        float SpawnTimer = 0;
        public LevelPiece FirstPickup;
        public LevelPiece LastPickup;
        public Spawner[] spawners;
        bool Paused;
        public float SpawnMin = 8.0f, SpawnMax = 16.0f, spTime;
        public GamePlay(MainStateMachine m) : base(m) {
            Camera camera = Camera.main;
            spawners = ParentMachine.Component.spawners;
            ParentMachine.Component.InGameUi.SetActive(true);
            Distance = ParentMachine.Component.InGameUi.GetComponentInChildren<Text>();
            Score = 0;
            Distance.text = Score.ToString();
            if (Magnet == null)
            {
                Magnet = GameObject.Instantiate(ParentMachine.Component.MagnetPrefab, new Vector3(camera.transform.position.x, camera.transform.position.y + camera.orthographicSize / 2), Quaternion.identity) as GameObject;
                Magnet.GetComponent<Magnet>().GameOverEvent += GameOver;
            }
            spTime = UnityEngine.Random.Range(SpawnMin, SpawnMax);
        }

        public void onPause()
        {
            Time.timeScale = 0f;
            Paused = true;
            ParentMachine.Component.InGameUi.SetActive(false);
            ParentMachine.Component.PauseMenu.SetActive(true);
        }
        public void onResume()
        {
            Time.timeScale = 1.0f;
            Paused = false;
            ParentMachine.Component.PauseMenu.SetActive(false);
            ParentMachine.Component.InGameUi.SetActive(true);
        }
        public override IEnumerator run()
        {
            spawnStart(2);
            while (ParentMachine.Current.GetType() == GetType())
            {
                if (Paused)
                {
                    yield return null;
                    continue;
                }
                AdvanceLevelPosition();
                ElapsedTime += Time.unscaledDeltaTime;
                SpawnTimer += Time.unscaledDeltaTime;

                if (ElapsedTime >= 3.0f && gamespeed<12.0f)
                {
                    gamespeed +=0.5f;
                    ElapsedTime = 0;
                }

                if (SpawnTimer >= spTime)
                {
                    SpawnPickups();
                    SpawnTimer = 0;
                    spTime = UnityEngine.Random.Range(SpawnMin, SpawnMax);
                }

                yield return null;
            }
            ParentMachine.Component.InGameUi.SetActive(false);
            ParentMachine.Component.PauseMenu.SetActive(false);
            if(Magnet!=null)
            {
                Magnet.GetComponent<Magnet>().GameOverEvent -= GameOver;
                GameObject.Destroy(Magnet);
            }
            removesections();
            ClearPowerups();
        }
        void ClearPowerups()
        {
            Pickup[] ps = GameObject.FindObjectsOfType<Pickup>();
            foreach (var p in ps)
            {
                GameObject.Destroy(p.gameObject);
            }
        }
        void SpawnPickups()
        {
            var spawner = spawners[UnityEngine.Random.Range(0, spawners.Length)];
            int r = UnityEngine.Random.Range(0, 11);
            int sp = r < 6 ? 0 : ( r < 8 ? 1 : 2 );
            spawner.Spawn(ParentMachine.Component.powerups[sp]);
        }
        public override void TriggerExit2D(Collider2D other)
        {

            if (other.gameObject.tag == "level")
            {
                LevelPiece op = firstPiece;
                GameObject.Destroy(op.piece);
                firstPiece = op.next;
                op.next = null;
                SpawnLevel();
                Score += 1;
                Distance.text = Score.ToString();
            }
        }

        public override void SpawnLevel()
        {
            int levno;
            int random = UnityEngine.Random.Range(0, 5);
            if (random < 4)
                levno = UnityEngine.Random.Range(0, StraightCount);
            else
                levno = UnityEngine.Random.Range(StraightCount, ParentMachine.Component.levels.Length - 1);
            LevelPiece p = new LevelPiece(ParentMachine.Component.levels[levno], lastPiece.Spawnpos());
            Addlevel(p);
        }
         void GameOver()
        {
            ParentMachine.SetState(typeof(GameOverState), false, new object[] {ParentMachine, Score});
        }
    }



  public class GameOverState:State<MainStateMachine>
  {
      public GameOverState(MainStateMachine m,int Score): base(m)
      {
          string NewBest = "";
          ParentMachine.Component.EndMenu.SetActive(true);
          if(Score > PlayerPrefs.GetInt(FSMgenerator.SCORE_KEY))
          {
              PlayerPrefs.SetInt(FSMgenerator.SCORE_KEY, Score);
              PlayerPrefs.Save();
              NewBest = "New Best";
          }
          ParentMachine.Component.FinalScore.text = "You Fell\r\n"  + Score.ToString() + "m\r\n"  +NewBest; 
      }

      public override IEnumerator run()
      {
          while (ParentMachine.Current.GetType()==GetType())
          {
              yield return null;
          }
          ToggleEndMenu(false);
          ResetGame();
      }
      void ToggleEndMenu(bool x)
      {
         ParentMachine.Component.EndMenu.SetActive(x);
      }

      void ResetGame()
      {   
          harmonicMotion.temp = 0;
          GamePlay.gamespeed = 5;
      }

  }
