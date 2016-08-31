using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

    public class FSMgenerator : MonoBehaviour
    {
        public MainStateMachine fsm;

        [HideInInspector]
        public static string SCORE_KEY = "BestScore";
        public GameObject MagnetPrefab;
        public Spawner[] spawners;
        public GameObject[] powerups;
        public GameObject[] levels;
        public GameObject StartMenu, EndMenu ,InGameUi,PauseMenu;
        public Text  FinalScore;

        void Awake()
        {
            fsm = MainStateMachine.GetNewInstance(null, this);
        }

        void Start()
        {
            Camera camera = Camera.main;
            this.transform.position = new Vector3(transform.position.x, (camera.transform.position.y + camera.orthographicSize) - 0.05f);
            StartCoroutine(fsm.run());
        }

        void OnTriggerExit2D(Collider2D other)
        {
            fsm.TriggerExit2D(other);
        }
    }
