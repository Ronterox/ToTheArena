using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public int highestScore;
    public int wave;
    public int kills;

    public PlayerData(int newScore, int newWave, int newKills)
    {
        highestScore = newScore;
        wave = newWave;
        kills = newKills;
    }
}
