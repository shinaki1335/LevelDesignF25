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

        Debug.Log($"Bala explosiva creada. Velocidad: {Speed}, Dirección: {movementDirection}");
    }

    void Update()
    {
        if (hasExploded) return;

        // MOVIMIENTO MANUAL de la bala
        transform.position += movementDirection * Speed * Time.deltaTime;
        float distanceTraveled = Vector3.Distance(initialPosition, transform.position);
        if (distanceTraveled >= explosionDistance)
        {
            TriggerExplosion();
        }
    }

    private void TriggerExplosion() // Método para manejar la explosión
    {
        if (hasExploded) return;
        hasExploded = true;

        Debug.Log($"¡EXPLOSIÓN RADIAL! Posición: {transform.position}");

        if (normalBulletPrefab == null)
        {
            Debug.LogError("Error: normalBulletPrefab no asignado en el inspector");
            Destroy(gameObject);
            return;
        }

        CreateRadialBullets();
        Destroy(gameObject);
    }

    private void CreateRadialBullets() // Método para crear balas en patrón radial
    {
        float angleIncrement = 360f / radialBulletCount;

        for (int i = 0; i < radialBulletCount; i++)
        {
            // Calcular ángulo para esta bala
            float currentAngle = i * angleIncrement;
            Quaternion bulletDirection = Quaternion.Euler(0f, 0f, currentAngle);

            ProjectileController newBullet = Instantiate(
                normalBulletPrefab,
                transform.position,
                bulletDirection
            );

            newBullet.Speed = radialBulletSpeed;

            Debug.Log($"Bala radial {i} creada en ángulo {currentAngle}°");
        }
    }

    public override void HitWall(GameObject wall) // Sobrescribir para detectar colisión con paredes
    {
        if (!hasExploded)
        {
            Debug.Log("Bala explosiva golpeó pared, explotando...");
            TriggerExplosion();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (hasExploded) return;

        // Explotar si golpea al jugador
        if (other.CompareTag("Player"))
        {
            Debug.Log("Bala explosiva golpeó al jugador, explotando...");
            TriggerExplosion();
        }
    }
}