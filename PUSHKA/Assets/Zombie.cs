using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : MonoBehaviour
{
    public int KillPoints;
    public double Health;
    public float Speed;
    private Rigidbody2D rb;
    private Player player;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Health <= 0)
        {
            Destroy(gameObject);
            player.Score += KillPoints;
        }
        //var positionTarget = targetForZomb.GetComponent<Rigidbody2D>().position;
        //var difference = positionTarget - rb.position;
        //var moveVelocity = difference.normalized * Speed;
        //rb.MovePosition(rb.position + moveVelocity * Time.fixedDeltaTime);
    }

    public void TakeDamage(double damage) => Health -= damage;
}
