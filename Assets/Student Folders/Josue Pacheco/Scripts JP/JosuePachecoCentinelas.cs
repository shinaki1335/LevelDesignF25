using System.Collections;
using UnityEngine;

public class JosuePachecoCentinelas : ActorController
{
    [Header("Configuración de Movimiento")]
    public float moveDistance = 3f;  // ← VARIABLE PÚBLICA para distancia

    [Header("Configuración de Disparo")]
    public float burstFireRate = 0.1f;
    public float continuousFireRate = 0.2f;

    public override void DoAction(string act, float amt)
    {
        if (act == "ShootBurst")
        {
            StartCoroutine(ShootBurstCoroutine((int)amt));
        }
        else if (act == "ShootContinuous")
        {
            StartCoroutine(ShootContinuousCoroutine(amt));
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

    // Dispara X balas en ráfaga
    private IEnumerator ShootBurstCoroutine(int bulletCount)
    {
        for (int i = 0; i < bulletCount; i++)
        {
            Shoot();
            yield return new WaitForSeconds(burstFireRate);
        }
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

    // NUEVO: Disparo estrella radial (14 direcciones)
    private IEnumerator ShootStarRadialCoroutine(int bulletCount)
    {
        float angleStep = 360f / bulletCount;

        for (int i = 0; i < bulletCount; i++)
        {
            float angle = i * angleStep;
            // Guardar rotación original
            Quaternion originalRotation = transform.rotation;

            // Rotar temporalmente para disparar en esa dirección
            transform.rotation = Quaternion.Euler(0, 0, angle);
            Shoot();

            // Restaurar rotación original
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