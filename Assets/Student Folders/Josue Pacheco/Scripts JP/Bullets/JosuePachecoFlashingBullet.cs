using UnityEngine;

public class JosuePachecoFlashingBullet : ProjectileController
{
    private float timer;
    private float flashSpeed;

    void Start()
    {
        flashSpeed = Random.Range(0.3f, 0.8f);
        timer = Random.Range(0f, 0.4f);
    }

    void Update()
    {
        float scale = 0.05f + Mathf.PingPong(timer * flashSpeed, 1f);
        transform.localScale = Vector3.one * scale;
        timer += Time.deltaTime;
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        // Verificar si es el player
        if (other.CompareTag("Player"))
        {
            ActorController player = other.GetComponentInParent<ActorController>();
            if (player != null)
            {
                // Aplicar daño usando el sistema del HazardController
                OnHit(player);
            }
        }

        // Verificar si es pared
        if (other.CompareTag("Wall"))
        {
            HitWall(other.gameObject);
        }
    }
}