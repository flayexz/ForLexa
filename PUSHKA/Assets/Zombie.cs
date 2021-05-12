using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : MonoBehaviour
{
    public int KillPoints;
    public double Health;
    public float Speed;
    private Rigidbody2D rb;
    private Rigidbody2D rbPlayer;
    public Player player;
    private Vector2 dir;
    private Vector2 moveV;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rbPlayer = player.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Health <= 0)
        {
            Destroy(gameObject);
            player.Score += KillPoints;
        }
        dir = rbPlayer.position - rb.position;
        moveV = dir.normalized * Speed;
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + moveV * Time.fixedDeltaTime);
    }

    public void TakeDamage(double damage) => Health -= damage;
}
