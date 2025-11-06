using UnityEngine;

public class JosuePachecoSerpentProjectile : ProjectileController
{
    [Header("Configuración Serpiente")]
    public float waveFrequency = 3f;
    public float waveAmplitude = 1f;
    public float waveSpeed = 2f;

    private Vector3 initialDirection;
    private float spawnTime;
    private float initialAngle;

    public override void OnStart()
    {
        base.OnStart();
        spawnTime = Time.time;
        initialDirection = transform.right; // Dirección inicial
        initialAngle = transform.eulerAngles.z;
    }

    void Update()
    {
        // Movimiento base del proyectil
        transform.position += initialDirection * Speed * Time.deltaTime;

        // Aplicar movimiento ondulatorio
        float waveOffset = Mathf.Sin((Time.time - spawnTime) * waveSpeed) * waveAmplitude;
        Vector3 waveMovement = transform.up * waveOffset * waveFrequency * Time.deltaTime;
        transform.position += waveMovement;

        // Rotar para seguir la dirección de la onda (opcional)
        float currentAngle = initialAngle + waveOffset * 30f; // Ajusta este multiplicador
        transform.rotation = Quaternion.Euler(0, 0, currentAngle);
    }
}