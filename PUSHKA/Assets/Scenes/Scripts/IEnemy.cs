using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemy
{ 
    public void TakeDamage(double damage);
    public void AttackPlayer(double damage);
}