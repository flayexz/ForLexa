using UnityEngine;

public class BoxOfRandomGun : MonoBehaviour
{
    [SerializeField] private AudioSource weaponBuySound;
    [SerializeField] private Gun[] possibleWeapons;
    [SerializeField] private float lifetimeSpawnedGunInBox;
    [SerializeField] private int costOfGeneratingNewWeapons;
    private Player player;
    private Bounds triggerZone;
    private Gun spawnedGun;
    private ScoreManager scoreOfPlayer;
    
    void Start()
    {
        player = FindObjectOfType<Player>();
        triggerZone = GetComponent<Collider2D>().bounds;
        scoreOfPlayer = FindObjectOfType<ScoreManager>();
    }
    
    void Update()
    {
        if (CheckSpawnConditions())
        {
            Spawn();
            weaponBuySound.Play();
            scoreOfPlayer.Score -= costOfGeneratingNewWeapons;
        }
    }

    private void Spawn()
    {
        spawnedGun = Instantiate(possibleWeapons[Random.Range(0, possibleWeapons.Length)],
            transform.position, Quaternion.identity);
        Destroy(spawnedGun.gameObject, lifetimeSpawnedGunInBox);
    }

    private bool CheckSpawnConditions()
    {
        return spawnedGun == null && triggerZone.Contains(player.transform.position) && 
               scoreOfPlayer.Score >= costOfGeneratingNewWeapons && Input.GetKeyDown(KeyCode.F);
    }
    
}
