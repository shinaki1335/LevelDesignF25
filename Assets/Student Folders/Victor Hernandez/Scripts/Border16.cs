using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
public class Border16: HazardController
{
    public override void DoAction(string act, float amt = 0)
    {
        if (act == "Storm2")
        {
            StartCoroutine(ShootStormCoroutine((int)amt));
        }
        else if (act == "Ring")
        {
            StartCoroutine(RingCoroutine((int)amt));
        }
        else
        {
            base.DoAction(act, amt);
        }
    }

    private IEnumerator ShootStormCoroutine(float duration)
    {
        float endTime = Time.time + duration;

        while (Time.time < endTime)
        {
            // How many bullets in this storm burst
            int gustSize = Random.Range(2, 5);

            for (int i = 0; i < gustSize; i++)
            {
                // Save the boss transform
                Vector3 oldPos = transform.position;
                Quaternion oldRot = transform.rotation;

                // Random X but using Y edge instead (firing left)
                float offset = Random.Range(-2f, 2f);
                Vector3 spawnPos = transform.position + new Vector3(0.5f, offset, 0);

                // Move boss to the spawn point temporarily
                transform.position = spawnPos;

                // RANDOM LEFT STORM ANGLE
                // 180° = left
                float angle = Random.Range(-20f, 20f);
                transform.rotation = Quaternion.Euler(0, 0, 0 + angle);

                // Shoot using ActorController
                Shoot();

                // Restore original transform
                transform.position = oldPos;
                transform.rotation = oldRot;

                // Delay inside gust
                yield return new WaitForSeconds(Random.Range(0.02f, 0.08f));
            }

            // Delay between gusts
            yield return new WaitForSeconds(Random.Range(0.15f, 0.35f));
        }
    }


    private System.Collections.IEnumerator RingCoroutine(int waveCount)
    {
        float bulletCount = 25;

        // Each bullet evenly spaced around 360 degrees
        float angleStep = 360f / bulletCount;

        Quaternion originalRotation = transform.rotation;

        for (int i = 0; i < bulletCount; i++)
        {
            float angle = i * angleStep;

            // Rotate to the firing angle
            transform.rotation = Quaternion.Euler(0, 0, angle);
            Shoot(); // uses ActorController's Shoot()
        }

        // Restore original rotation
        transform.rotation = originalRotation;

        yield return null; // one frame delay, can be adjusted
    }
}