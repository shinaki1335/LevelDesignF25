using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
public class Border2 : HazardController
{
    public override void DoAction(string act, float amt = 0)
    {
        if (act == "Storm")
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
                // TEMPORARILY save the boss' transform
                Vector3 oldPos = transform.position;
                Quaternion oldRot = transform.rotation;

                // Pick a random X across the sprite's top edge
                float offset = Random.Range(-2f, 2f);   // <--- adjust based on sprite width
                Vector3 spawnPos = transform.position + new Vector3(offset, 0.5f, 0);

                // Move boss to this temporary spawn point
                transform.position = spawnPos;

                // Random downward storm angle
                float angle = Random.Range(-20f, 20f);
                transform.rotation = Quaternion.Euler(0, 0, 270f + angle);

                // Shoot normally using ActorController
                Shoot();

                // Restore boss transform instantly
                transform.position = oldPos;
                transform.rotation = oldRot;

                // Small delay inside gust
                yield return new WaitForSeconds(Random.Range(0.02f, 0.08f));
            }

            // Delay between gusts of bullets
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