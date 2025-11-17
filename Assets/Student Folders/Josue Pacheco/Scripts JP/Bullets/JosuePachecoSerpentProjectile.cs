using UnityEngine;

public class JosuePachecoSerpentProjectile : ProjectileController
{
    private Vector3 initialDirection;
    private float spawnTime;
    private float initialAngle;
    private JosuePachecoBoss boss; 

    public override void OnStart() 
    {
        base.OnStart();
        spawnTime = Time.time;
        initialDirection = transform.right;
        initialAngle = transform.eulerAngles.z;

        boss = FindObjectOfType<JosuePachecoBoss>();
    }

    void Update()
    {
        transform.position += initialDirection * Speed * Time.deltaTime; 

        float frequency = boss != null ? boss.snakeFrequency : 8f;
        float amplitude = boss != null ? boss.snakeAmplitude : 2f;
        float speed = boss != null ? boss.snakeWaveSpeed : 5f;

        // Calcular el movimiento ondulatorio
        float waveOffset = Mathf.Sin((Time.time - spawnTime) * speed) * amplitude;
        Vector3 waveMovement = transform.up * waveOffset * frequency * Time.deltaTime;
        transform.position += waveMovement;

        float currentAngle = initialAngle + waveOffset * 30f;
        transform.rotation = Quaternion.Euler(0, 0, currentAngle);
    }
    void OnTriggerEnter2D(Collider2D other)
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