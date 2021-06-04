using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float Speed;
    public float LifeTime;
    public float Distance;
    public double Damage;
    public LayerMask Target;

    void Start()
    {
        Invoke("Destroy", LifeTime);
    }

    void Update()
    {
        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, transform.up, Distance, Target);
        if (hitInfo.collider != null)
        {
            if (hitInfo.collider.CompareTag("Zombie"))
            {
                hitInfo.collider.GetComponent<Zombie>().TakeDamage(Damage);
            }
            Destroy();
        }
        transform.Translate(Vector2.right * Speed * Time.deltaTime);
    }
    
    

    private void Destroy()
    {
        Destroy(gameObject);
    }
}
