using UnityEngine;

public class MedicinePack : MonoBehaviour
{
    [SerializeField] private double additionalHp;
    private Player player;
    private Bounds triggerZone;
    void Start()
    {
        player = FindObjectOfType<Player>();
        triggerZone = GetComponent<Collider2D>().bounds;
    }
    
    void Update()
    {
        if (triggerZone.Contains(player.transform.position) && Input.GetKeyDown(KeyCode.E))
        { 
            player.Heal(additionalHp);
            Destroy(gameObject);
        }
    }
}
