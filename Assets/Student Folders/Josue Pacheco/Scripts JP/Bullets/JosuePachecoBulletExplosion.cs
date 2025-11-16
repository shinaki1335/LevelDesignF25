using UnityEngine;

public class JosuePachecoBulletExplosion : ProjectileController
{
    [Header("Configuración Explosión")]
    public float explosionDistance = 5f;
    public int radialBulletCount = 8;
    public float radialBulletSpeed = 6f;

    [Header("Prefab de Bala Normal")]
    public ProjectileController normalBulletPrefab;

    private Vector3 initialPosition;
    private bool hasExploded = false;
    private Vector3 movementDirection;

    public override void OnStart()
    {
        base.OnStart();
        initialPosition = transform.position;
        movementDirection = transform.right;
    }

    void Update()
    {
        if (hasExploded) return;

        // Movimiento manual de la bala
        transform.position += movementDirection * Speed * Time.deltaTime;
        float distanceTraveled = Vector3.Distance(initialPosition, transform.position);
        if (distanceTraveled >= explosionDistance)
        {
            TriggerExplosion();
        }
    }

    private void TriggerExplosion() 
    {
        // Evitar múltiples explosiones
        if (hasExploded) return;
        hasExploded = true;

        if (normalBulletPrefab == null)
        {
            Destroy(gameObject);
            return;
        }

        CreateRadialBullets();
        Destroy(gameObject);
    }

    private void CreateRadialBullets() // Crea balas en un patrón radial
    {
        float angleIncrement = 360f / radialBulletCount;

        for (int i = 0; i < radialBulletCount; i++)
        {
            float currentAngle = i * angleIncrement;
            Quaternion bulletDirection = Quaternion.Euler(0f, 0f, currentAngle);

            // Instanciar la nueva bala
            ProjectileController newBullet = Instantiate(
                normalBulletPrefab,
                transform.position,
                bulletDirection
            );

            newBullet.Speed = radialBulletSpeed;
        }
    }

    public override void HitWall(GameObject wall)
    {
        if (!hasExploded)
        {
            TriggerExplosion();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (hasExploded) return;

        if (other.CompareTag("Player"))
        {
            TriggerExplosion();
        }
    }
}