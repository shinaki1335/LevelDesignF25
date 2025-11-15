using System.Collections;
using UnityEngine;

public class JosuePachecoCentinelas : ActorController
{
    [Header("Configuración de Movimiento")]
    public float moveDistance = 3f; 

    [Header("Configuración de Disparo")]
    public float burstFireRate = 0.1f;
    public float continuousFireRate = 0.2f;

    public override void DoAction(string act, float amt)
    {
        if (act == "ShootBurst")
        {
            StartCoroutine(ShootBurstCoroutine((int)amt));
        }
        else if (act == "ShootContinuous") // ← DISPARO CONTINUO
        {
            StartCoroutine(ShootContinuousCoroutine(amt));
        }
        else if (act == "ShootGust")  // ← DISPARO RÁFAGA
        {
            StartCoroutine(ShootGustsCoroutine((int)amt));
        }
        else if (act == "ShootStar")  // ← DISPARO ESTRELLA RADIAL
        {
            StartCoroutine(ShootStarRadialCoroutine((int)amt));
        }
        else if (act == "MoveLeft")  // ← MOVIMIENTO IZQUIERDA
        {
            StartCoroutine(MoveLeftCoroutine(amt));
        }
        else if (act == "MoveRight")  // ← MOVIMIENTO DERECHA
        {
            StartCoroutine(MoveRightCoroutine(amt));
        }
        else if (act == "MoveUp")  // ← MOVIMIENTO ARRIBA
        {
            StartCoroutine(MoveUpCoroutine(amt));
        }
        else if (act == "MoveDown")  // ← MOVIMIENTO ABAJO
        {
            StartCoroutine(MoveDownCoroutine(amt));
        }
        else
        {
            base.DoAction(act, amt);
        }
    }

    //Dispara bala explosiva 
    private IEnumerator ShootBurstCoroutine(int bulletCount)
    {
        
        if (AltProjectiles != null && AltProjectiles.Count > 0)
        {
            ProjectileController explosiveBullet = AltProjectiles[0];

            
            Shoot(explosiveBullet, transform.position, transform.rotation.eulerAngles);

            Debug.Log($"Bala explosiva disparada. Total en ráfaga: {bulletCount}");
        }
        else
        {
            Debug.LogWarning("No se encontró bala explosiva en AltProjectiles[0]");
        }

        yield return null;
    }

    // Dispara continuamente por X segundos
    private IEnumerator ShootContinuousCoroutine(float duration)
    {
        float endTime = Time.time + duration;
        while (Time.time < endTime)
        {
            Shoot();
            yield return new WaitForSeconds(continuousFireRate);
        }
    }

    // Dispara ráfagas de 4 balas, X veces
    private IEnumerator ShootGustsCoroutine(int gustCount)
    {
        for (int i = 0; i < gustCount; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                Shoot();
                yield return new WaitForSeconds(0.1f); 
            }
            yield return new WaitForSeconds(0.5f); 
        }
    }

    // NUEVO: Disparo estrella radial (14 direcciones)
    private IEnumerator ShootStarRadialCoroutine(int bulletCount)
    {
        float angleStep = 360f / bulletCount;

        for (int i = 0; i < bulletCount; i++)
        {
            float angle = i * angleStep;
            
            Quaternion originalRotation = transform.rotation;
            transform.rotation = Quaternion.Euler(0, 0, angle);
            Shoot();
            transform.rotation = originalRotation;

            yield return null;
        }
    }

    // NUEVO: Movimiento hacia izquierda
    private IEnumerator MoveLeftCoroutine(float duration)
    {
        float endTime = Time.time + duration;
        Vector3 originalPos = transform.position;
        Vector3 targetPos = originalPos + Vector3.left * moveDistance;  

        while (Time.time < endTime)
        {
            transform.position = Vector3.Lerp(originalPos, targetPos, (Time.time - (endTime - duration)) / duration);
            yield return null;
        }
    }

    // NUEVO: Movimiento hacia derecha
    private IEnumerator MoveRightCoroutine(float duration)
    {
        float endTime = Time.time + duration;
        Vector3 originalPos = transform.position;
        Vector3 targetPos = originalPos + Vector3.right * moveDistance;  

        while (Time.time < endTime)
        {
            transform.position = Vector3.Lerp(originalPos, targetPos, (Time.time - (endTime - duration)) / duration);
            yield return null;
        }
    }

    // NUEVO: Movimiento hacia arriba
    private IEnumerator MoveUpCoroutine(float duration)
    {
        float endTime = Time.time + duration;
        Vector3 originalPos = transform.position;
        Vector3 targetPos = originalPos + Vector3.up * moveDistance; 

        while (Time.time < endTime)
        {
            transform.position = Vector3.Lerp(originalPos, targetPos, (Time.time - (endTime - duration)) / duration);
            yield return null;
        }
    }

    // NUEVO: Movimiento hacia abajo
    private IEnumerator MoveDownCoroutine(float duration)
    {
        float endTime = Time.time + duration;
        Vector3 originalPos = transform.position;
        Vector3 targetPos = originalPos + Vector3.down * moveDistance; 

        while (Time.time < endTime)
        {
            transform.position = Vector3.Lerp(originalPos, targetPos, (Time.time - (endTime - duration)) / duration);
            yield return null;
        }
    }
}