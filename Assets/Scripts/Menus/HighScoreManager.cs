using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HighScoreManager : MonoBehaviour
{
    public int highestScore = 0;
    public int wave = 1;
    public int kills = 0;

    [SerializeField] Transform leaderboardObj = null;

    public static HighScoreManager instance = null;

    private void Awake()
    {
        MakeSingleton();
        LoadPlayerData();
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

    private void LoadPlayerData()
    {
        PlayerData data = SaveSystem.LoadScore();
        if (data != null)
        {
            highestScore = data.highestScore;
            wave = data.wave;
            kills = data.kills;

            UpdateLeaderBoard();
        }
    }

    public void CheckData()
    {
        if ((int)GameManager.instance.score > highestScore)
        {
            highestScore = (int)GameManager.instance.score;
            wave = GameManager.instance.enemySpawner.wavesCounter;
            kills = GameManager.instance.kills;

            SaveSystem.SaveScore(highestScore, wave, kills);

            UpdateLeaderBoard();
        }
    }

    private void UpdateLeaderBoard()
    {
        if (leaderboardObj != null)
        {
            leaderboardObj.Find("Score Text").Find("Actual Score").GetComponent<TextMeshProUGUI>().text = highestScore.ToString();
            leaderboardObj.Find("Wave Text").Find("Actual Wave").GetComponent<TextMeshProUGUI>().text = wave.ToString();
            leaderboardObj.Find("Kills Text").Find("Actual Kills").GetComponent<TextMeshProUGUI>().text = kills.ToString();
        }
    }
}
