using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxOfRandomGun : MonoBehaviour
{
    [SerializeField] private Gun[] possibleWeapons;
    [SerializeField] private float lifeTimeSpawnGun;
    private Player player;
    private Bounds triggerZone;
    private Gun spawnGun;
    
    void Start()
    {
        player = FindObjectOfType<Player>();
        triggerZone = GetComponent<Collider2D>().bounds;
    }
    
    void Update()
    {
        if (spawnGun == null &&
            triggerZone.Contains(player.transform.position) && Input.GetKeyDown(KeyCode.E))
            Spawn();
    }

    private void Spawn()
    {
        spawnGun = Instantiate(possibleWeapons[Random.Range(0, possibleWeapons.Length)],
            transform.position, Quaternion.identity);
        Destroy(spawnGun.gameObject, lifeTimeSpawnGun);
    }
    
}
