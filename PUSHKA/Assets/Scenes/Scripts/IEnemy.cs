public interface IEnemy
{ 
    public void TakeDamage(double damage);
    public void AttackPlayer(double damage, IPlayer target);
}