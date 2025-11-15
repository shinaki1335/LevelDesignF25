using UnityEngine;


public class JosuePachecoBoss : HazardController
{
    [Header("Configuración de Movimiento")]
    public float moveSpeed = 8f;
    public float chargeSpeed = 12f;

    [Header("Configuración de Disparo")]
    public float fanShotAngle = 45f;
    public int fanShotCount = 5;
    public float fanShotDelay = 0.1f;

    [Header("Movimiento Circular")]
    public float circleRadius = 3f;
    public float circleSpeed = 2f;
    private float currentCircleRadius = 0f;
    private bool isTransitioning = false;

    [Header("Disparo Serpiente")]
    public float snakeFrequency = 3f;
    public float snakeAmplitude = 1f;
    public float snakeWaveSpeed = 2f;
    public float snakeShotDelay = 0.1f;

    [Header("Disparo Giratorio")]
    public float spinShotRate = 0.1f;
    public float spinRotationSpeed = 180f;

    [Header("Disparo Aleatorio")]
    public float randomShotAccuracy = 30f;
    public float randomShotDelay = 0.3f;


    private Transform spriteChild;
    private float currentSpinAngle = 0f;
    private bool isSpinning = false;

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
        FindSpriteChild();
    }

    private void FindSpriteChild()
    {

        foreach (Transform child in transform)
        {
            if (child.GetComponent<SpriteRenderer>() != null)
            {
                spriteChild = child;
                Debug.Log("Sprite child encontrado: " + spriteChild.name);
                break;
            }
        }

        if (spriteChild == null)
        {
            GameObject spriteObj = new GameObject("Sprite");
            spriteObj.transform.SetParent(transform);
            spriteObj.transform.localPosition = Vector3.zero;
            spriteObj.AddComponent<SpriteRenderer>();
            spriteChild = spriteObj.transform;
            Debug.Log("Sprite child creado");
        }
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
        if (spriteChild != null)
        {
            spriteChild.rotation = Quaternion.identity;
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
        else if (act == "ChargeAtPlayer") // Carga hacia el jugador
        {
            ChargeAtPlayer(amt);
        }
        else if (act == "FanShotAtPlayer") // Disparo en abanico hacia el jugador
        {
            if (amt == -1)
                StartCoroutine(FanShotAtPlayerCoroutine());
            else
                StartCoroutine(FanShotAtFixedAngleCoroutine(amt)); // Disparo en abanico en ángulo fijo
        }
        else if (act == "StartCircleMove") // Movimiento circular con transición de radio
        {
            StartCircleMovement(amt);
        }
        else if (act == "StopCircleMove") // Detener movimiento circular
        {
            StopCircleMovement();
        }
        else if (act == "GoToCenter") // Ir al centro de la escena
        {
            GoToCenter(amt);
        }
        else if (act == "WaveShot") // Disparo en olas circulares
        {
            StartCoroutine(WaveShotCoroutine((int)amt));
        }
        else if (act == "SnakeShot") // Disparo serpiente/ondulado
        {
            StartCoroutine(SnakeShotCoroutine((int)amt));
        }
        else if (act == "SpawnFlashingBullets") // Balas parpadeantes estáticas
        {
            StartCoroutine(SpawnFlashingBulletsCoroutine(amt));
        }
        else if (act == "InwardSpiralShot") // Espiral hacia el boss
        {
            StartCoroutine(InwardSpiralCoroutine(amt));
        }
        else if (act == "StartSpinShot") // Disparo giratorio
        {
            StartSpinShot(amt);
        }
        else if (act == "StopSpinShot") // Detener disparo giratorio
        {
            StopSpinShot();
        }
        else if (act == "RandomShotsAtPlayer") // Disparos aleatorios seguidos apuntando aproximadamente al jugador
        {
            StartCoroutine(RandomShotsAtPlayerCoroutine((int)amt));
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
    private System.Collections.IEnumerator FanShotAtPlayerCoroutine()
    {
        if (player != null)
        {
            Vector3 playerDirection = (player.transform.position - transform.position).normalized;
            float baseAngle = Mathf.Atan2(playerDirection.y, playerDirection.x) * Mathf.Rad2Deg;

            // Calcular ángulos para el disparo en abanico
            float angleStep = fanShotAngle / (fanShotCount - 1);
            float startAngle = baseAngle - (fanShotAngle / 2);

            
            for (int i = 0; i < fanShotCount; i++)
            {
                float currentAngle = startAngle + (angleStep * i);
                Vector3 rotation = new Vector3(0, 0, currentAngle);
                Shoot(null, transform.position, rotation);
            }

            
            yield return null;
        }
    }

    // Disparo en abanico/escopeta en ángulo fijo
    private System.Collections.IEnumerator FanShotAtFixedAngleCoroutine(float angle)
    {
        float baseAngle = angle;

        // Calcular ángulos para el disparo en abanico
        float angleStep = fanShotAngle / (fanShotCount - 1);
        float startAngle = baseAngle - (fanShotAngle / 2);

        for (int i = 0; i < fanShotCount; i++)
        {
            float currentAngle = startAngle + (angleStep * i);
            Vector3 rotation = new Vector3(0, 0, currentAngle);
            Shoot(null, transform.position, rotation);
        }

        yield return null;
    }

    // Movimiento circular con transición de radio
    private void StartCircleMovement(float duration)
    {
        if (!isCircling)
        {
            isCircling = true;
            isTransitioning = true;
            circleCenter = transform.position;
            circleAngle = 0f;
            circleDuration = duration;
            circleTimer = 0f;
            currentCircleRadius = 0f; 
        }
    }

    private void StopCircleMovement()
    {
        isCircling = false;
    }

    private void CircleUpdate()
    {
        if (circleTimer < circleDuration && isCircling)
        {
            circleAngle += circleSpeed * Time.deltaTime;
            circleTimer += Time.deltaTime;

            // Transición suave del radio
            if (isTransitioning)
            {
                currentCircleRadius = Mathf.Lerp(0f, circleRadius, circleTimer / 1f); 
                if (currentCircleRadius >= circleRadius * 0.95f)
                {
                    isTransitioning = false;
                    currentCircleRadius = circleRadius;
                }
            }

            float x = circleCenter.x + Mathf.Cos(circleAngle) * currentCircleRadius;
            float y = circleCenter.y + Mathf.Sin(circleAngle) * currentCircleRadius;

            transform.position = new Vector3(x, y, transform.position.z);
        }
        else if (isCircling)
        {
            isCircling = false;
            isTransitioning = false;
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

        transform.position = centerPos; 
    }

    // Disparo en olas circulares
    private System.Collections.IEnumerator WaveShotCoroutine(int waveCount)
    {
        int bulletsPerWave = 18;
        float waveDelay = 1.0f;  

        for (int wave = 0; wave < waveCount; wave++)
        {
            
            float angleStep = 360f / bulletsPerWave;

            for (int i = 0; i < bulletsPerWave; i++)
            {
                float angle = i * angleStep;
                Vector3 rotation = new Vector3(0, 0, angle);
                Shoot(null, transform.position, rotation);
            }

            
            yield return new WaitForSeconds(waveDelay);
        }
    }

    // Disparo serpiente/ondulado con dirección controlada
    private System.Collections.IEnumerator SnakeShotCoroutine(int bulletCount)
    {
        
        ProjectileController serpentPrefab = null;
        if (AltProjectiles != null && AltProjectiles.Count > 0)  
        {
            serpentPrefab = AltProjectiles[0]; // Primer proyectil alternativo
        }

        for (int i = 0; i < bulletCount; i++)
        {
            float baseAngle = transform.eulerAngles.z;
            Vector3 rotation = new Vector3(0, 0, baseAngle);
            Shoot(serpentPrefab, transform.position, rotation);

            yield return new WaitForSeconds(snakeShotDelay);
        }
    }

    // Balas parpadeantes estáticas
    private System.Collections.IEnumerator SpawnFlashingBulletsCoroutine(float duration)
    {
        Vector3[] positions = new Vector3[]
        {
        new Vector3(-4, 3, 0), new Vector3(4, 3, 0),
        new Vector3(-2, 2, 0), new Vector3(2, 2, 0),
        new Vector3(-4, 0, 0), new Vector3(4, 0, 0),
        new Vector3(-2, -2, 0), new Vector3(2, -2, 0),
        new Vector3(-4, -3, 0), new Vector3(4, -3, 0)
        };

        ProjectileController flashingPrefab = (AltProjectiles != null && AltProjectiles.Count > 1)
            ? AltProjectiles[1] : DefaultProjectile;

        System.Collections.Generic.List<GameObject> bullets = new System.Collections.Generic.List<GameObject>();

        foreach (Vector3 pos in positions)
        {
            GameObject bullet = Instantiate(flashingPrefab.gameObject, pos, Quaternion.identity);
            bullets.Add(bullet);
        }

        yield return new WaitForSeconds(duration);

        foreach (GameObject bullet in bullets)
        {
            if (bullet != null) Destroy(bullet);
        }
    }

    // Espiral hacia el boss
    private System.Collections.IEnumerator InwardSpiralCoroutine(float duration)
    {
        float endTime = Time.time + duration;
        float angle = 0f;
        float radius = 6.5f;

        ProjectileController spiralPrefab = (AltProjectiles != null && AltProjectiles.Count > 2)
            ? AltProjectiles[2] : DefaultProjectile;

        while (Time.time < endTime)
        {
            // Calcular posición en el borde
            float x = Mathf.Cos(angle * Mathf.Deg2Rad) * radius;
            float y = Mathf.Sin(angle * Mathf.Deg2Rad) * radius;
            Vector3 spawnPos = transform.position + new Vector3(x, y, 0);

            // Crear bala y asignar objetivo
            GameObject bulletObj = Instantiate(spiralPrefab.gameObject, spawnPos, Quaternion.identity);
            JosuePachecoInwardBullet bullet = bulletObj.GetComponent<JosuePachecoInwardBullet>();
            if (bullet != null)
            {
                bullet.SetTarget(transform.position); 
            }

            angle += 30f; // Espaciado entre balas

            yield return new WaitForSeconds(0.13f);
        }
    }

    // Iniciar disparo giratorio
    private void StartSpinShot(float duration)
    {
        if (!isSpinning)
        {
            isSpinning = true;
            StartCoroutine(SpinShotCoroutine(duration));
        }
    }

    // Detener disparo giratorio
    private void StopSpinShot()
    {
        if (isSpinning)
        {
            // Reactivar collider al detener manualmente
            Collider2D bossCollider = GetComponent<Collider2D>();
            if (bossCollider != null)
                bossCollider.enabled = true;
        }
        isSpinning = false;
    }

    // Disparo giratorio en 4 direcciones
    private System.Collections.IEnumerator SpinShotCoroutine(float duration)
    {
        float endTime = Time.time + duration;
        Collider2D bossCollider = GetComponent<Collider2D>();
        if (bossCollider != null)
            bossCollider.enabled = false;

        while (Time.time < endTime && isSpinning)
        {
            // Disparar en 4 direcciones equidistantes
            for (int i = 0; i < 4; i++)
            {
                float currentAngle = currentSpinAngle + (i * 90f);
                Vector3 rotation = new Vector3(0, 0, currentAngle);
                Shoot(null, transform.position, rotation);
            }

            // Rotar todas las direcciones (el objeto padre rota, pero no el sprite)
            currentSpinAngle += spinRotationSpeed * spinShotRate;

            yield return new WaitForSeconds(spinShotRate);
        }
        if (bossCollider != null)
            bossCollider.enabled = true;

        isSpinning = false;
    }

    // Disparos aleatorios seguidos apuntando aproximadamente al jugador
    private System.Collections.IEnumerator RandomShotsAtPlayerCoroutine(int shotCount)
    {
        for (int i = 0; i < shotCount; i++)
        {
            if (player != null)
            {
                // Calcular dirección base hacia el jugador
                Vector3 playerDirection = (player.transform.position - transform.position).normalized;
                float baseAngle = Mathf.Atan2(playerDirection.y, playerDirection.x) * Mathf.Rad2Deg;

                // Añadir aleatoriedad al ángulo
                float randomOffset = Random.Range(-randomShotAccuracy, randomShotAccuracy);
                float finalAngle = baseAngle + randomOffset;

                Vector3 rotation = new Vector3(0, 0, finalAngle);
                Shoot(null, transform.position, rotation);
            }

            yield return new WaitForSeconds(randomShotDelay);
        }
    }
}