using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float Speed;

    public int Score { get; set; }
    private Rigidbody2D player;
    private Vector2 moveInput;
    private Vector2 moveVelocity;

    void Start()
    {
        Score = 0;
        player = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        moveVelocity = moveInput.normalized * Speed;
    }

    private void FixedUpdate()
    {
        player.MovePosition(player.position + moveVelocity * Time.fixedDeltaTime);
    }
}
