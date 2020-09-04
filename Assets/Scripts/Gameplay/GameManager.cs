using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI wavesText = null;
    [SerializeField] TextMeshProUGUI scoreText = null;
    [SerializeField] TextMeshProUGUI enemiesKilledText = null;

    public static GameManager instance;
    [HideInInspector]
    public float score = 0;

    [HideInInspector]
    public int kills = 0;

    public EnemySpawner enemySpawner = null;
    public Difficulty difficulty = Difficulty.Normal;

    [SerializeField] GameObject pauseMenu = null;

    [HideInInspector]
    public bool isPaused;

    [SerializeField] GameObject cleanKeyObj = null;

    public int customWaves = 1, customEnemiesPerWave = 1, customEnemyIncrement = 2;
    public float customTimeBTWEnemies = 1;

    [SerializeField] BuffSpawner buffSpawner = null;

    [HideInInspector]
    public bool themePlaying = false;

    [SerializeField] Animator mainCameraAnimator = null;

    [SerializeField] VideoPlayer cleaningCutscene = null;

    [SerializeField] Animator transition = null;

    public enum Difficulty
    {
        Normal,
        Hard,
        Custom
    }

    private void Start()
    {
        ChangeDifficulty();
    }

    private void Awake()
    {
        MakeSingleton();
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !isPaused && !cleaningCutscene.isPlaying)
            Pause();

        if (Input.GetKeyDown("r") && !isPaused)
            Restart();

        if (kills > 20 && Input.GetKeyDown(KeyCode.Space) && !isPaused)
        {
            Clean();
            CleanBodies();

            if (!cleaningCutscene.gameObject.activeSelf)
            {
                cleaningCutscene.gameObject.SetActive(true);
                cleaningCutscene.Play();
            }
        }

        CheckCleaningVideo();
    }

    private void MakeSingleton()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static void IncreaseScore(int points)
    {
        instance.score += points;
        instance.scoreText.text = instance.score.ToString();
        instance.scoreText.GetComponent<Animator>().SetTrigger("POP");
    }

    public static void UpdateWave(int wave)
    {
        instance.wavesText.text = "Wave " + wave;
        if (wave % 5 == 0)
            instance.buffSpawner.SpawnBuff();
    }
    public static void UpdateWave(string wave)
    {
        instance.wavesText.text = wave;
    }

    public static void IncreaseKills(int enemies)
    {
        instance.kills += enemies;
        instance.enemiesKilledText.text = "Kills: " + instance.kills;
        instance.enemiesKilledText.GetComponent<Animator>().SetTrigger("POP");

        if (instance.kills % 400 == 0)
            instance.CleanBodies();

        if (instance.kills > 20 && !instance.cleanKeyObj.activeSelf)
            instance.cleanKeyObj.SetActive(true);
    }

    public void ChangeDifficulty()
    {
        if (difficulty.Equals(Difficulty.Normal))
        {
            enemySpawner.waves = 20;
            enemySpawner.maxEnemiesPerWave = 3;
            enemySpawner.enemyIncrementPerWave = 1;
            enemySpawner.timeBTWEnemies = 1;
        }
        else if (difficulty.Equals(Difficulty.Hard))
        {
            enemySpawner.waves = 20;
            enemySpawner.maxEnemiesPerWave = 3;
            enemySpawner.enemyIncrementPerWave = 2;
            enemySpawner.timeBTWEnemies = 0.5f;
        }
        else
        {
            enemySpawner.waves = customWaves;
            enemySpawner.maxEnemiesPerWave = customEnemiesPerWave;
            enemySpawner.enemyIncrementPerWave = customEnemyIncrement;
            enemySpawner.timeBTWEnemies = customTimeBTWEnemies;
        }
    }
    public void SetDifficulty(int newDifficulty)
    {
        difficulty = (Difficulty)newDifficulty;
    }
    public void Pause()
    {
        pauseMenu.SetActive(true);

        if (Time.timeScale == 1)
        {
            Time.timeScale = 0;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            isPaused = true;
        }
        else
        {
            Time.timeScale = 1;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            isPaused = false;
        }
    }

    public void Restart()
    {
        transition.SetTrigger("transition");

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        IncreaseScore(-(int)score);
        IncreaseKills(-(int)kills);
        UpdateWave(1);

        ChangeDifficulty();

        cleanKeyObj.SetActive(false);

        enemySpawner.ResetSpawner();

        themePlaying = false;

        Debug.Log("Game Restarted");
    }

    public void CloseGame()
    {
        Application.Quit();
        Debug.Log("Game Closed");
    }

    public void Clean()
    {
        UndestructibleLoad[] allGameObjects = FindObjectsOfType<UndestructibleLoad>();

        foreach (UndestructibleLoad obj in allGameObjects)
        {
            Destroy(obj.gameObject);
        }
    }
    public void CleanBodies()
    {
        Deadbody[] allGameObjects = FindObjectsOfType<Deadbody>();

        foreach (Deadbody obj in allGameObjects)
        {
            Destroy(obj.gameObject);
        }
    }

    public void ShakeCamera()
    {
        mainCameraAnimator.SetTrigger("shake");
    }

    private void CheckCleaningVideo()
    {
        if (cleaningCutscene.gameObject.activeSelf && !cleaningCutscene.isPlaying)
        {
            cleaningCutscene.gameObject.SetActive(false);
            Time.timeScale = 1;
        }
        else if (cleaningCutscene.isPlaying && Time.time != 0)
            Time.timeScale = 0;
    }

    public void WinState()
    {
        AudioManager.instance.Play("cheer");
    }
}
