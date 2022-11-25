using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject[] tetrisBLockPrefabs;

    private void Start()
    {
        SpawnBlock();
    }

    public void SpawnBlock()
    {
        Instantiate(tetrisBLockPrefabs[Random.Range(0, tetrisBLockPrefabs.Length)], transform.position, Quaternion.identity);
    }
}