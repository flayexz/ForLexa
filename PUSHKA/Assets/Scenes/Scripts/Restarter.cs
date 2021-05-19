using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Restarter : MonoBehaviour
{
    private Player player;
    void Start()
    {
        player = FindObjectOfType<Player>();
    }
    
    void Update()
    {
        if (player.Health <= 0)
            Application.LoadLevel(0);
    }
}
