using UnityEngine;

public class JosuePachecoBoss : HazardController
{
    [Header("Configuración de Movimiento")]
    public float moveSpeed = 8f; // Aumenté la velocidad base
    public float chargeSpeed = 12f;

    [Header("Configuración de Disparo")]
    public float fanShotAngle = 45f;
    public int fanShotCount = 5;
    public float fanShotDelay = 0.1f;

    [Header("Movimiento Circular")]
    public float circleRadius = 3f;
    public float circleSpeed = 2f;

    private bool isCharging = false;
    private bool isCircling = false;
    private bool isMoving = false;
    private Vector3 chargeTarget;
    private Vector3 moveDirection;
    private float moveDuration;
    private float moveTimer;
    private PlayerController player;
    private Vector3 circleCenter;
    private float circleAngle = 0f;
    private float circleDuration;
    private float circleTimer;

    public override void OnStart()
    {
        base.OnStart();
        player = FindObjectOfType<PlayerController>();
        circleCenter = transform.position;
    }

    void Update()
    {
        if (isMoving)
        {
            MoveUpdate();
        }
        if (isCircling)
        {
            CircleUpdate();
        }
    }

    public override void DoAction(string act, float amt)
    {
        if (act == "MoveLeft")
        {
            StartMove(Vector3.left, amt);
        }
        else if (act == "MoveRight")
        {
            StartMove(Vector3.right, amt);
        }
        else if (act == "MoveUp")
        {
            StartMove(Vector3.up, amt);
        }
        else if (act == "MoveDown")
        {
            StartMove(Vector3.down, amt);
        }
        else if (act == "ChargeAtPlayer")
        {
            ChargeAtPlayer(amt);
        }
        else if (act == "FanShotAtPlayer")
        {
            StartCoroutine(FanShotAtPlayerCoroutine());
        }
        else if (act == "StartCircleMove")
        {
            StartCircleMovement(amt);
        }
        else if (act == "StopCircleMove")
        {
            StopCircleMovement();
        }
        else if (act == "GoToCenter")
        {
            GoToCenter(amt);
        }
        else if (act == "WaveShot")
        {
            StartCoroutine(WaveShotCoroutine((int)amt));
        }
        else if (act == "SnakeShot")
        {
            StartCoroutine(SnakeShotCoroutine((int)amt));
        }
        else
        {
            base.DoAction(act, amt);
        }

    }

    // NUEVO: Sistema de movimiento mejorado
    private void StartMove(Vector3 direction, float duration)
    {
        if (!isMoving && !isCharging)
        {
            isMoving = true;
            moveDirection = direction.normalized;
            moveDuration = duration;
            moveTimer = 0f;
        }
    }

    private void MoveUpdate()
    {
        if (moveTimer < moveDuration)
        {
            transform.position += moveDirection * moveSpeed * Time.deltaTime;
            moveTimer += Time.deltaTime;
        }
        else
        {
            isMoving = false;
        }
    }

    // Carga hacia el jugador como un toro
    private void ChargeAtPlayer(float duration)
    {
        if (player != null && !isCharging && !isMoving)
        {
            isCharging = true;
            chargeTarget = player.transform.position;
            StartCoroutine(ChargeCoroutine(duration));
        }
    }

    private System.Collections.IEnumerator ChargeCoroutine(float duration)
    {
        float timer = 0f;
        Vector3 startPos = transform.position;

        while (timer < duration && Vector3.Distance(transform.position, chargeTarget) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, chargeTarget, chargeSpeed * Time.deltaTime);
            timer += Time.deltaTime;
            yield return null;
        }

        isCharging = false;
    }

    // Disparo en abanico/escopeta hacia el jugador
    // Disparo en abanico/escopeta (todas las balas simultáneas)
    private System.Collections.IEnumerator FanShotAtPlayerCoroutine()
    {
        if (player != null)
        {
            Vector3 playerDirection = (player.transform.position - transform.position).normalized;
            float baseAngle = Mathf.Atan2(playerDirection.y, playerDirection.x) * Mathf.Rad2Deg;

            // Calcular ángulos para el disparo en abanico
            float angleStep = fanShotAngle / (fanShotCount - 1);
            float startAngle = baseAngle - (fanShotAngle / 2);

            // Disparar TODAS las balas al mismo tiempo
            for (int i = 0; i < fanShotCount; i++)
            {
                float currentAngle = startAngle + (angleStep * i);
                Vector3 rotation = new Vector3(0, 0, currentAngle);

                // Disparar inmediatamente sin esperar
                Shoot(null, transform.position, rotation);
            }

            // Pequeña espera para no bloquear otras corrutinas
            yield return null;
        }
    }

    // Movimiento circular con duración limitada
    private void StartCircleMovement(float duration)
    {
        if (!isCircling)
        {
            isCircling = true;
            circleCenter = transform.position;
            circleAngle = 0f;
            circleDuration = duration;
            circleTimer = 0f;
        }
    }

    private void StopCircleMovement()
    {
        isCircling = false;
    }

    private void CircleUpdate()
    {
        if (circleTimer < circleDuration)
        {
            circleAngle += circleSpeed * Time.deltaTime;
            circleTimer += Time.deltaTime;

            float x = circleCenter.x + Mathf.Cos(circleAngle) * circleRadius;
            float y = circleCenter.y + Mathf.Sin(circleAngle) * circleRadius;

            transform.position = new Vector3(x, y, transform.position.z);

            Vector3 direction = new Vector3(-Mathf.Sin(circleAngle), Mathf.Cos(circleAngle), 0);
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
        else
        {
            isCircling = false;
        }
    }

    // Va al centro de la escena (0,0)
    private void GoToCenter(float duration)
    {
        StartCoroutine(GoToCenterCoroutine(duration));
    }

    private System.Collections.IEnumerator GoToCenterCoroutine(float duration)
    {
        Vector3 startPos = transform.position;
        Vector3 centerPos = Vector3.zero;
        float timer = 0f;

        while (timer < duration)
        {
            transform.position = Vector3.Lerp(startPos, centerPos, timer / duration);
            timer += Time.deltaTime;
            yield return null;
        }

        transform.position = centerPos; // Asegurar que queda exactamente en (0,0)
    }

    // Disparo en olas circulares
    private System.Collections.IEnumerator WaveShotCoroutine(int waveCount)
    {
        int bulletsPerWave = 18; // Balas por ola
        float waveDelay = 1.0f;  // Delay entre olas

        for (int wave = 0; wave < waveCount; wave++)
        {
            // Disparar una ola completa (360°)
            float angleStep = 360f / bulletsPerWave;

            for (int i = 0; i < bulletsPerWave; i++)
            {
                float angle = i * angleStep;
                Vector3 rotation = new Vector3(0, 0, angle);
                Shoot(null, transform.position, rotation);
            }

            // Esperar antes de la siguiente ola
            yield return new WaitForSeconds(waveDelay);
        }
    }

    // Disparo serpiente/ondulado
    private System.Collections.IEnumerator SnakeShotCoroutine(int bulletCount)
    {
        float frequency = 3f;    // Frecuencia de la onda
        float amplitude = 1f;    // Altura de la onda
        float waveSpeed = 2f;    // Velocidad de la onda

        for (int i = 0; i < bulletCount; i++)
        {
            // Calcular el ángulo basado en una función de onda (seno)
            float waveOffset = Mathf.Sin(Time.time * waveSpeed + i * 0.5f) * amplitude;
            float baseAngle = waveOffset * frequency; // Convierte la onda a ángulo

            Vector3 rotation = new Vector3(0, 0, baseAngle);
            Shoot(null, transform.position, rotation);

            yield return new WaitForSeconds(0.1f); // Delay entre balas
        }
    }
}