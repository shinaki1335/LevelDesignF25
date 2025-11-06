using UnityEngine;

public class JosuePachecoInwardBullet : ProjectileController
{
    private Vector3 targetPosition;
    private float spiralSpeed = 180f; // Velocidad de rotación en espiral
    private float currentAngle;

    public void SetTarget(Vector3 target)
    {
        targetPosition = target;
        // Calcular ángulo inicial hacia el objetivo
        Vector3 direction = (targetPosition - transform.position).normalized;
        currentAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
    }

    void Update()
    {
        if (targetPosition != Vector3.zero)
        {
            // Movimiento en espiral hacia el objetivo
            currentAngle += spiralSpeed * Time.deltaTime;

            // Calcular dirección con rotación espiral
            Vector3 direction = (targetPosition - transform.position).normalized;
            float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            // Combinar dirección hacia objetivo + movimiento espiral
            float finalAngle = targetAngle + Mathf.Sin(Time.time * 2f) * 30f;
            Vector3 moveDirection = Quaternion.Euler(0, 0, finalAngle) * Vector3.right;

            transform.position += moveDirection * Speed * Time.deltaTime;
        }
    }
}