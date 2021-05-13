using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Zombie[] Zombeis; // может сделать target
    public float MinSpawnRadius;
    public float MaxSpawnRadius;

    private int amountOfZombies;

    private float currentTimeBetweenSpawn;
    public float TimeBetweenSpawn;

    void Start()
    {
        amountOfZombies = Zombeis.Length;
    }

    private void FixedUpdate()
    {
        Spawn();
    }

    private void Spawn()
    {
        if (currentTimeBetweenSpawn >= TimeBetweenSpawn)
        {
            Instantiate(Zombeis[Random.Range(0, amountOfZombies)],
                GetRandomPosition(transform.position), Quaternion.identity);
            currentTimeBetweenSpawn = 0;
        }
        else
            currentTimeBetweenSpawn += Time.deltaTime;
    }

    private Vector3 GetRandomPosition(Vector3 position) =>
        position + new Vector3(GetRandomOffset(), GetRandomOffset());

    private float GetRandomOffset() => 
        Random.Range(MinSpawnRadius, MaxSpawnRadius) * GetPositiveOrNegativeSign();

    private int GetPositiveOrNegativeSign() => Random.value < 0.5 ? -1 : 1;
}
