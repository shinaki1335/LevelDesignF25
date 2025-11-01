using System.Collections;
using UnityEngine;

public class JosuePachecoBurstShooter : ActorController
{
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
            yield return new WaitForSeconds(0.1f);
        }
    }

    // Dispara continuamente por X segundos
    private IEnumerator ShootContinuousCoroutine(float duration)
    {
        float endTime = Time.time + duration;
        while (Time.time < endTime)
        {
            Shoot();
            yield return new WaitForSeconds(0.2f);  // Dispara cada 0.2 segundos
        }
    }
}