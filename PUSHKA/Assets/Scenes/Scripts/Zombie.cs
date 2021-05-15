using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class Zombie : MonoBehaviour, IEnemy
{
    public float heatsPerSecond;
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
    
    void Start()
    {
        player = FindObjectOfType<Player>();
        rb = GetComponent<Rigidbody2D>();
        rbPlayer = player.GetComponent<Rigidbody2D>();
        scoreManager = FindObjectOfType<ScoreManager>();
    }

    void Update()
    {
        if (Health <= 0)
        {
            Destroy(gameObject);
            scoreManager.Score += KillPoints;
        }

        if (timePassed > AttackDelay)
        {
            AttackPlayer(damageByHand);
            timePassed = 0;
        }
        else
            timePassed += Time.deltaTime;
    }

    private void FixedUpdate()
    {
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, Speed * Time.deltaTime);
    }

    public void TakeDamage(double damage) => Health -= damage;
    public void AttackPlayer(double damage)
    {
        Physics2D.OverlapCircleAll(transform.position, attackRange)
            .Select(x => x.GetComponent<IPlayer>()).First(x => x != null).TakeDamage(damage);
    }
}