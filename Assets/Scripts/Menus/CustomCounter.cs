using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CustomCounter : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI counterText = null;
    private float counter;

    public void IncreaseCounter(int value)
    {
        counter += value;
        if (counter < 0)
            counter = 0;

        counterText.text = counter.ToString();

        AudioManager.instance.Play("pop");
    }
    public void IncreaseCounter(float value)
    {
        counter += value;
        if (counter < 0)
            counter = 0;

        counterText.text = counter.ToString();

        AudioManager.instance.Play("pop");
    }

    public void ReduceCounter(int value)
    {
        counter -= value;
        if (counter < 0)
            counter = 0;

        counterText.text = counter.ToString();

        AudioManager.instance.Play("pop");
    }
    public void ReduceCounter(float value)
    {
        counter -= value;
        if (counter < 0)
            counter = 0;

        counterText.text = counter.ToString();

        AudioManager.instance.Play("pop");
    }

    public void SetCustomWaves()
    {
        GameManager.instance.customWaves = (int)counter;
    }
    public void SetCustomEnemyIncrement()
    {
        GameManager.instance.customEnemyIncrement = (int)counter;
    }
    public void SetCustomEnemiesQuantity()
    {
        GameManager.instance.customEnemiesPerWave = (int)counter;
    }
    public void SetCustomEnemiesSpawnTime()
    {
        GameManager.instance.customTimeBTWEnemies = counter;
    }
}
