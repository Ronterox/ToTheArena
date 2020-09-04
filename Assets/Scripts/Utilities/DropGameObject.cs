using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropGameObject : MonoBehaviour
{
    [SerializeField] GameObject[] drops = null;
    private void Start()
    {
        int random = Random.Range(0, drops.Length);
        Instantiate(drops[random], transform.position, Quaternion.Euler(0, 0, Random.Range(0f, 360f)));
    }
}
