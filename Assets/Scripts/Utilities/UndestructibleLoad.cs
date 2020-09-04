using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UndestructibleLoad : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
