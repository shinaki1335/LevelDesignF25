using UnityEngine;

public class JosuePachecoInwardBullet : ProjectileController
{
    private Vector3 targetPosition;
    private float spiralIntensity = 60f;

    public void SetTarget(Vector3 target)
    {
        targetPosition = target;
    }

    void Update()
    {

        Vector3 direction = (targetPosition - transform.position).normalized;

        // Aplicar movimiento en espiral
        float spiral = Mathf.Sin(Time.time * 3f) * spiralIntensity * Time.deltaTime;
        direction = Quaternion.Euler(0, 0, spiral) * direction;

        transform.position += direction * Speed * Time.deltaTime;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
    void OnTriggerEnter2D(Collider2D other) // Detectar colisiones
    {

        if (other.CompareTag("Player")) 
        {
            ActorController player = other.GetComponentInParent<ActorController>();
            if (player != null)
            {
  
                OnHit(player);
            }
        }

        if (other.CompareTag("Wall"))
        {
            HitWall(other.gameObject);
        }
    }
}