using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffSpawner : MonoBehaviour
{
    [SerializeField] Transform[] buffSpawns = null;

    [SerializeField] GameObject[] buffs = null;

    public void SpawnBuff()
    {
        int pos = Random.Range(0, buffSpawns.Length);
        int buffRandom = Random.Range(0, buffs.Length);
        Instantiate(buffs[buffRandom], buffSpawns[pos].position, Quaternion.identity);
    }
}
