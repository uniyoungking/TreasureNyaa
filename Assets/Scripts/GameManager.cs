using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // 나중에 제작
    public static GameManager instance;

    [SerializeField]
    private PlatformGenerator platformGenerator;

    public event Action onStartGame;
    public event Action onGameOver;    

    private Transform camTr;
    private PlayerController player;


    // 바닥 생성 높이
    private const float platformInteval = 2f;
    private float lastPlatformYPos = 3f;
    private float prevCamYpos;

    public bool IsPause { get; set; } = false;

    public int level = 1;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        InitializeComponent();
    }

    private void Start()
    {
        camTr = Camera.main.transform;
        prevCamYpos = camTr.transform.position.y;

        UIManager.instance.TimePause();
    }

    private void Update()
    {
        if (IsPause)
            return;

        CheckGeneratePlatform();
    }

    private void InitializeComponent()
    {
        player = FindObjectOfType<PlayerController>();
    }

    public void GameStart(int level)
    {
        UIManager.instance.TimeResume();

        this.level = level;

        for (int i = 0; i < 6; i++)
        {
            platformGenerator.GenerateBasicePlatformLine(lastPlatformYPos, level);
            lastPlatformYPos += platformInteval;
        }

        onStartGame?.Invoke();
    }

    public void GameOver()
    {
        UIManager.instance.TimeResume();
    }

    private void CheckGeneratePlatform()
    {
        if (camTr.transform.position.y > prevCamYpos + platformInteval)
        {
            platformGenerator.GenerateBasicePlatformLine(lastPlatformYPos, level);
            lastPlatformYPos += platformInteval;

            prevCamYpos += platformInteval;
        }
    }
}
