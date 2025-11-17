using System.Collections;
using UnityEngine;

public class JosuePachecoCentinelas : ActorController
{
    [Header("Configuración de Movimiento")]
    public float moveDistance = 3f;

    [Header("Configuración de Disparo")]
    public float burstFireRate = 0.1f;
    public float continuousFireRate = 0.2f;

    [Header("Disparo Ondulante")]
    public float swingWaveFrequency = 2f;   
    public float swingWaveAmplitude = 45f;   
    public float swingShotRate = 0.15f;      

    public override void DoAction(string act, float amt)
    {
        if (act == "ShootBurst") //Dispara bala explosiva
        {
            StartCoroutine(ShootBurstCoroutine((int)amt));
        }
        else if (act == "ShootContinuous") //Dispara continuamente por X segundos
        {
            StartCoroutine(ShootContinuousCoroutine(amt));
        }
        else if (act == "ShootGust") //Ráfagas de 3 balas, X veces
        {
            StartCoroutine(ShootGustsCoroutine((int)amt));
        }
        else if (act == "ShootStar") //Disparo estrella radial
        {
            StartCoroutine(ShootStarRadialCoroutine((int)amt));
        }
        else if (act == "SwingWaveShot") //Disparo ondulante/columpio
        {
            StartCoroutine(SwingWaveShotCoroutine(amt));
        }
        else if (act == "MoveLeft")
        {
            StartCoroutine(MoveLeftCoroutine(amt));
        }
        else if (act == "MoveRight")
        {
            StartCoroutine(MoveRightCoroutine(amt));
        }
        else if (act == "MoveUp")
        {
            StartCoroutine(MoveUpCoroutine(amt));
        }
        else if (act == "MoveDown")
        {
            StartCoroutine(MoveDownCoroutine(amt));
        }
        else
        {
            base.DoAction(act, amt);
        }
    }

    // NUEVO: Disparo ondulante/columpio
    private IEnumerator SwingWaveShotCoroutine(float duration)
    {
        float endTime = Time.time + duration;
        float startTime = Time.time;

        while (Time.time < endTime)
        {
            // Calcular el ángulo oscilante
            float baseAngle = transform.eulerAngles.z;
            float swingOffset = Mathf.Sin((Time.time - startTime) * swingWaveFrequency) * swingWaveAmplitude;
            float finalAngle = baseAngle + swingOffset;

            Vector3 rotation = new Vector3(0, 0, finalAngle);
            Shoot(null, transform.position, rotation);

            yield return new WaitForSeconds(swingShotRate);
        }
    }

    //Dispara bala explosiva 
    private IEnumerator ShootBurstCoroutine(int bulletCount)
    {
        if (AltProjectiles != null && AltProjectiles.Count > 0)
        {
            ProjectileController explosiveBullet = AltProjectiles[0];
            Shoot(explosiveBullet, transform.position, transform.rotation.eulerAngles);
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

    // Dispara ráfagas de 3 balas, X veces
    private IEnumerator ShootGustsCoroutine(int gustCount)
    {
        for (int i = 0; i < gustCount; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                Shoot();
                yield return new WaitForSeconds(0.05f);
            }
            yield return new WaitForSeconds(0.5f);
        }
    }

    // Disparo estrella radial
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

    // Movimiento hacia izquierda
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

    // Movimiento hacia derecha
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

    // Movimiento hacia arriba
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

    // Movimiento hacia abajo
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