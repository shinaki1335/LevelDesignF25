using UnityEngine;

public class JosuePachecoInwardBullet : ProjectileController
{
    private Vector3 targetPosition;
    private float spiralIntensity = 50f;

    public void SetTarget(Vector3 target)
    {
        targetPosition = target;
    }

    void Update()
    {
        // Dirección hacia el objetivo
        Vector3 direction = (targetPosition - transform.position).normalized;

        // Añadir movimiento espiral con seno
        float spiral = Mathf.Sin(Time.time * 3f) * spiralIntensity * Time.deltaTime;
        direction = Quaternion.Euler(0, 0, spiral) * direction;

        transform.position += direction * Speed * Time.deltaTime;

        // Rotar sprite hacia dirección
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}