using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SocialPlatforms.Impl;
using Random = System.Random;

public class Zombie : MonoBehaviour, IEnemy
{
    public float heatsPerSecond;
    [SerializeField] private MedicinePack medicinePack;
    [SerializeField] private double chanceToDropMedicinePack;
    private float AttackDelay => 1 / heatsPerSecond;
    private float timePassed;
    public float attackRange;
    public double damageByHand;
    public int KillPoints;
    public double Health;
    public float Speed;
    private Rigidbody2D rb;
    private Rigidbody2D rbPlayer;
    private Player player;
    private ScoreManager scoreManager;
    private Collider2D triggerZone;
    private bool isReload => timePassed < AttackDelay;
    private NavMeshAgent agent;
    
    void Start()
    {
        player = FindObjectOfType<Player>();
        rb = GetComponent<Rigidbody2D>();
        rbPlayer = player.GetComponent<Rigidbody2D>();
        scoreManager = FindObjectOfType<ScoreManager>();
        triggerZone = GetComponents<Collider2D>().First(x => x.isTrigger);
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    void Update()
    {
        if (Health <= 0)
        {
            Destroy(gameObject);
            if (medicinePack != null && new Random().NextDouble() <= chanceToDropMedicinePack)
                Instantiate(medicinePack,transform.position,Quaternion.identity);
            scoreManager.Score += KillPoints;
        }

        if (isReload)
            timePassed += Time.deltaTime;
    }

    private void FixedUpdate()
    { 
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position,
            Speed * Time.deltaTime);
    }

    public void TakeDamage(double damage) => Health -= damage;
    
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.TryGetComponent<IPlayer>(out IPlayer pl))
        {
            if (!isReload)
            {
                AttackPlayer(damageByHand, pl);
                timePassed = 0;
            }
        }
    }

    public void AttackPlayer(double damage, IPlayer target) => target.TakeDamage(damage);
}