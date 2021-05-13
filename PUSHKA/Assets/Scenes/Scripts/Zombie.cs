using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class Zombie : MonoBehaviour
{
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
            scoreManager.UpdateScore(KillPoints);
        }
    }

    private void FixedUpdate()
    {
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, Speed * Time.deltaTime);
    }

    public void TakeDamage(double damage) => Health -= damage;
}
