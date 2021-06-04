using UnityEngine;
public class Spawner : MonoBehaviour
{
    [SerializeField] private float deltaSpawnTime;
    [SerializeField] private float spawnTimeMultiplier;
    private float timePassed;
    public Zombie[] Zombeis; 
    public float MinSpawnRadius;
    public float MaxSpawnRadius;
    private ScoreManager scoreManager;

    private int amountOfZombies;

    private float currentTimeBetweenSpawn;
    public float TimeBetweenSpawn;

    void Start()
    {
        scoreManager = FindObjectOfType<ScoreManager>();
        amountOfZombies = Zombeis.Length;
    }

    private void FixedUpdate()
    {
        UpdateSpawnTimeIfTimePassed();
        Spawn();
    }

    private void UpdateSpawnTimeIfTimePassed()
    {
        if (timePassed >= deltaSpawnTime)
        {
            TimeBetweenSpawn *= spawnTimeMultiplier;
            timePassed = 0;
        }
        else
            timePassed += Time.deltaTime;
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
